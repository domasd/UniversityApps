using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMAP_App.Imap
{
    class ImapClientWSSL : IDisposable
    {

        private static System.Net.Sockets.TcpClient _tcpc;
        private static System.Net.Security.SslStream _ssl;
        private static int _bytes;
        static byte[] buffer;

        private const int msgBytesCount = 1024;


        private Enum.ImapClientState state;

        public Enum.ImapClientState State
        {
            get { return this.state; }
        }

        public ImapClientWSSL()
        {
            state = Enum.ImapClientState.NotConnected;
        }


        public async Task<ImapCommandResult> ConnectAsync(string host, int port)
        {
            Enum.ImapCommandState commandState;

            try
            {
                Task connect;
                if (state == Enum.ImapClientState.Connected) throw new Exception("Already connected");
                connect = new Task(() =>
                {
                    _tcpc = new System.Net.Sockets.TcpClient(host, port);
                    _ssl = new System.Net.Security.SslStream(_tcpc.GetStream());
                    _ssl.AuthenticateAsClient(host);
                });
                connect.Start();
                await connect;


                String response;
                response = receiveNwriteAsync("").Result; //Just receiving, so no awaiting (Should be fast as this operation to server is not CPU intensive)
                if (response.Contains("OK")) commandState = Enum.ImapCommandState.Ok; else commandState = Enum.ImapCommandState.No;
                if (commandState == Enum.ImapCommandState.Ok) state = Enum.ImapClientState.Connected;



            }
            catch (SocketException)
            {
                throw new AuthenticationException("Wrong information specified, unable to connect to the server");
            }
            catch (Exception)
            {
                throw;
            }

            return new ImapCommandResult() { CommandState = commandState };
        }

        public async Task<ImapCommandResult> LoginAsync(string username, string password)
        {
            if (state == Enum.ImapClientState.Authorized)
            {
                throw new AuthenticationException("Already logged in");
            }

            Enum.ImapCommandState commandState = Enum.ImapCommandState.Undefined;
            Task<string> login = receiveNwriteAsync("$ LOGIN  " + username + " " + password + "  \r\n");
            string received = await login;
            received = received.TrimEnd('\0');

            if (received.StartsWith("$ OK") && received.Contains("Logged in"))
            {
                commandState = Enum.ImapCommandState.Ok;
                state = Enum.ImapClientState.Authorized;
            }
            else if (received.StartsWith("$ NO")) commandState = Enum.ImapCommandState.No;
            else if (received.StartsWith("$ BAD")) commandState = Enum.ImapCommandState.Bad;
            return new ImapCommandResult { CommandState = commandState };
        }

        public async Task<ImapCommandResult> SelectFolderAsync(string folderName)
        {
            if (state != Enum.ImapClientState.Authorized)
            {
                throw new AuthenticationException("Not logged in");
            }

            Enum.ImapCommandState commandState = Enum.ImapCommandState.Undefined;
            Task<string> select = receiveNwriteAsync(string.Format("$ SELECT {0} \r\n", folderName));
            string received = await select;
            string toSend = string.Empty;
            received = received.TrimEnd('\0');

            if (received.Contains("Select completed"))
            {
                commandState = Enum.ImapCommandState.Ok;
                Regex exists = new Regex("\\d+\\sEXISTS");
                toSend = exists.Match(received).Value;
            }
            else if (received.Contains("NO Client tried to access nonexistent namespace"))
            {
                commandState = Enum.ImapCommandState.No;

            }

            return new ImapCommandResult { CommandState = commandState, text = toSend };
        }

        public async Task<ImapCommandResult> CloseAsync()
        {
            Enum.ImapCommandState commandState = Enum.ImapCommandState.Undefined;
            Task<string> closeTask = receiveNwriteAsync("$ CLOSE \r\n");
            await closeTask;
            commandState = Enum.ImapCommandState.Ok;
            return new ImapCommandResult { CommandState = commandState };
        }

        private async Task<ImapCommandResult> LogoutAsync()
        {
            Enum.ImapCommandState commandState = Enum.ImapCommandState.Undefined;
            Task<string> logoutTask = receiveNwriteAsync("$ LOGOUT \r\n");
            await logoutTask;
            commandState = Enum.ImapCommandState.Ok;
            return new ImapCommandResult { CommandState = commandState };
        }

        public async Task<ImapCommandResult> EmailListAsync(string folderName)
        {
            StringBuilder toSend = new StringBuilder();
            Enum.ImapCommandState commandState = Enum.ImapCommandState.Undefined;

            ImapCommandResult resultFromSelect = await SelectFolderAsync(folderName);
            if (resultFromSelect.CommandState == Enum.ImapCommandState.No)
            {
                throw new Exception("Could not select specified folder");
            }

            string command = string.Format("$ FETCH 1:* BODY.PEEK[HEADER.FIELDS (From Subject Date)]\r\n");
            Task<string> emailListTask = receiveNwriteAsync(command);
            string received = await emailListTask;
            received = received.TrimEnd('\0');


            if (received.StartsWith("$ BAD"))
            {
                commandState = Enum.ImapCommandState.Bad;
            }
            else
            {
                string fromPattern = "\\w*\\.*\\w*@\\w+\\.*\\w+";
                string subjectPattern = "Subject:.+";
                string datePattern = "Date:.+";

                string[] emails = received.Split('*').Where(x => x.Length > 1).ToArray();
                for (int i = 0; i < emails.Length; i++)
                {
                    string from = "From: " + Regex.Match(emails[i], fromPattern).Value.TrimEnd('\r'),
                        subject = Regex.Match(emails[i], subjectPattern).Value.TrimEnd('\r'),
                        date = Regex.Match(emails[i], datePattern).Value.TrimEnd('\r');

                    toSend.AppendLine(string.Format("[{0}]", i));
                    toSend.AppendLine(from).AppendLine(subject).AppendLine(date);
                    toSend.AppendLine();
                }
                commandState = Enum.ImapCommandState.Ok;
            }
            return new ImapCommandResult { CommandState = commandState, text = toSend.ToString() };
        }

        public async Task<ImapCommandResult> GetEmailTextAsync(int sequence)
        {
            ImapCommandResult imapCommandResult = new ImapCommandResult();
            imapCommandResult.CommandState = Enum.ImapCommandState.Undefined;
            string command = string.Format("$ FETCH {0} BODY[Text]\r\n", sequence);


            Task<ImapCommandResult> getTextTask = getAndConcatMsgAsync(command, "OK Fetch completed.\r\n");
            imapCommandResult = await getTextTask;

            return imapCommandResult;
        }

        private async Task<ImapCommandResult> getAndConcatMsgAsync(string command, string EndValue)
        {
            StringBuilder sb = new StringBuilder();
            ImapCommandResult imapCommandResult;

            Task<string> emailListTask = receiveNwriteAsync(command);
            string firstReceived = await emailListTask;
            firstReceived = firstReceived.TrimEnd('\0');

            sb.Append(firstReceived);
            string firstLine = firstReceived.Split('\n')[0];

            if (firstLine.StartsWith("$ BAD"))
            {
                imapCommandResult.CommandState = Enum.ImapCommandState.Bad;
                imapCommandResult.text = "Bad - specified email does not exist";
                return imapCommandResult;
            }

            int byteAmount;
            Int32.TryParse(Regex.Match(firstLine, "{\\d*}").Value.TrimEnd('}').TrimStart('{'), out byteAmount);

            while (!sb.ToString().EndsWith(EndValue))
            {
                Task<string> task = receiveNwriteAsync("");
                string received = await task;
                received = received.TrimEnd('\0');
                sb.Append(received);
            }

            imapCommandResult.CommandState = Enum.ImapCommandState.Ok;
            imapCommandResult.text = sb.ToString();

            return imapCommandResult;
        }

        public async Task<ImapCommandResult> ExpungeAsync()
        {
            ImapCommandResult result = new ImapCommandResult();
            result.CommandState = Enum.ImapCommandState.Undefined;

            Task<string> expungeTask = receiveNwriteAsync("$ EXPUNGE\r\n");
            string received = await expungeTask;
            received = received.TrimEnd('\0');

            if (received.Contains("OK EXPUNGE completed")) result.CommandState = Enum.ImapCommandState.Ok;

            return result;
        }

        public async Task<ImapCommandResult> SetDeleteFlag(int sequence)
        {
            ImapCommandResult result = new ImapCommandResult();
            result.CommandState = Enum.ImapCommandState.Undefined;
            string command = string.Format("$ STORE {0} +FLAGS (\\Deleted)\r\n", sequence);

            Task<string> storeTask = receiveNwriteAsync(command);
            string received = await storeTask;
            received = received.TrimEnd('\0');

            if (received.Contains("OK Store completed")) result.CommandState = Enum.ImapCommandState.Ok;
            return result;

        }


        public async Task<ImapCommandResult> ListFolders()
        {
            string toReturn;

            Enum.ImapCommandState commandState = Enum.ImapCommandState.Undefined;
            string command = string.Format("$ LIST \"{0}\" \"{1}\" \r\n", "", "*");

            Task<string> listAll = receiveNwriteAsync(command);
            string received = await listAll;
            received = received.TrimEnd('\0');

            //CPU intensive
            toReturn = await Task<string>.Run(() =>
            {
                StringBuilder sbResponse = new StringBuilder();
                try
                {
                    Regex regexForListCompleted = new Regex(@"(OK List completed.)");

                    if (regexForListCompleted.IsMatch(received)) commandState = Enum.ImapCommandState.Ok;
                    else return null;

                    IEnumerable<string> line = received.Split('\n').Take(1);
                    string delimiter = line.First().Split(' ').ToArray()[3].Trim('"');

                    Regex regexFolderName = new Regex("(\\w*\\" + delimiter + "*\\w+)+\\\r"); // Escaped \ - added +1
                    MatchCollection matches = regexFolderName.Matches(received);

                    foreach (Match match in matches.Cast<Match>())
                    {
                        string folderName = match.Value.TrimEnd('\r');
                        int count = GetFolderUnseenCountAsync(folderName).Result;
                        sbResponse.AppendLine(folderName + string.Format(" [Unseen: {0}]", count));
                    }

                }
                catch
                {
                    throw;
                }
                return sbResponse.ToString();
            });

            if (toReturn == null) commandState = Enum.ImapCommandState.Undefined;

            return new ImapCommandResult() { text = toReturn, CommandState = commandState };
        }

        private static async Task<int> GetFolderUnseenCountAsync(string folderName)
        {
            int count;
            string command = "$ STATUS " + folderName + " (UNSEEN)\r\n";
            Regex regex = new Regex("(UNSEEN\\s\\d+)");

            Task<string> countTask = receiveNwriteAsync(command);
            string response = await countTask;
            response = response.TrimEnd('\0');
            string unSeen = regex.Match(response).Value.Split(' ')[1];
            Int32.TryParse(unSeen, out count);

            return count;
        }

        private static async Task<string> receiveNwriteAsync(string command)
        {

            try
            {
                if (command != "")
                {
                    if (_tcpc.Connected)
                    {
                        byte[] bytesArray;
                        bytesArray = Encoding.UTF8.GetBytes(command);
                        Task write = _ssl.WriteAsync(bytesArray, 0, bytesArray.Length);
                        // Sending
                        await write;
                    }
                    else
                    {
                        throw new ApplicationException("TCP CONNECTION DISCONNECTED");
                    }
                }
                _ssl.Flush();

                buffer = new byte[msgBytesCount];

                Task<int> bytesTask = _ssl.ReadAsync(buffer, 0, msgBytesCount);
                _bytes = await bytesTask;

                string recMessage = Encoding.Default.GetString(buffer);
                return recMessage;

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public void Dispose()
        {
            LogoutAsync();
            _ssl.Close();
            _ssl.Dispose();
            _tcpc.Close();

        }

        public string JustDoTheCommand(string command)
        {
            Task<string> recTask = receiveNwriteAsync(command);
            return recTask.Result;
        }
    }
}
