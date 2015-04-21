using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IMAP_App.Imap;
using Enum = IMAP_App.Imap.Enum;

namespace IMAP_App
{

    class Program
    {
        private const string username = "username@domain.com";
        private const string password = "password";
        private const string host = "barsta.vhost.lt";
        private const int port = 993;

        static StringBuilder sb = new StringBuilder();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
                using (ImapClientWSSL client = new ImapClientWSSL())
                {
                    //Connect to server
                    TryConnectAsync(host, port, client);

                    //Checking if authenticated
                    if (client.State == Enum.ImapClientState.Connected)
                        Console.WriteLine("Connected successfully - now attempting logging in");

                    //Logging on
                    TryLoginAsync(username, password, client);
                    if (client.State == Enum.ImapClientState.Authorized)
                        Console.WriteLine("Logged in successfully");



                    for (;;)
                    {
                        int selection;
                        string command;
                        Console.WriteLine("1 - List all folders");
                        Console.WriteLine("2 - Select folder");
                      //  Console.WriteLine("9 - Testing");
                      //  Console.WriteLine("10 - Testing - type your own command");
                        string input = Console.ReadLine();
                        Int32.TryParse(input, out selection);
                        switch (selection)
                        {
                            case 1:
                                TryListAllFoldersAsync(client);
                                break;
                            case 2:
                                Console.WriteLine("Type in the folder name to select:");
                                String folderName = Console.ReadLine();
                                SelectFolder(client, folderName);
                                break;
                            case 9:

                                command = string.Format("$ SELECT INBOX \r\n");
                                Console.WriteLine(client.JustDoTheCommand(command).TrimEnd('\0'));

                                break;
                            case 10:
                                command = Console.ReadLine() + "\r\n";
                                Console.WriteLine(client.JustDoTheCommand(command).TrimEnd('\0'));

                                break;
                            default:
                                Console.WriteLine("Choose something!");
                                break;

                        }
                    }
                }

             
            }
            catch (Exception ex)
            {
                criticalError(ex.Message);
            }

            Console.ReadKey();



        }

        private async static void SelectFolder(ImapClientWSSL client, string folderName)
        {
            try
            {
                Task<ImapCommandResult> listEmails = client.EmailListAsync(folderName);
                ImapCommandResult listEmailsResult = await LoadingWhileTask(listEmails);

                if (listEmailsResult.CommandState != Enum.ImapCommandState.Ok)
                {
                    Console.WriteLine("Probably bad folder specified");
                    return;
                }

                Console.WriteLine(listEmailsResult.text);

                while (true)
                {

                    Console.WriteLine("Type in the number of mail to read or -1 to go to previous menu");
                    string input = Console.ReadLine();
                    int selection;
                    Int32.TryParse(input, out selection);

                    if (selection < 0)
                    {
                        break;
                    }

                    ImapCommandResult result = await LoadingWhileTask(client.GetEmailTextAsync(selection + 1));
                    string received = result.text;
                    Console.WriteLine(received);

                    if (result.CommandState != Enum.ImapCommandState.Ok) break;

                    //email deletion
                    Console.WriteLine("Delete? Y or N");
                    input = Console.ReadLine();
                    if (input == "Y")
                    {
                        //Simple version of deleteing - just mark it deleted flag and expunge
                        //Mark
                        result = await LoadingWhileTask(client.SetDeleteFlag(selection + 1));
                        if (result.CommandState != Enum.ImapCommandState.Ok)
                        {
                            Console.WriteLine("Could not mark it as deleted");
                            break;
                        }
                        //Expunge
                        await LoadingWhileTask(client.ExpungeAsync());
                        Console.WriteLine("Deleted!");
                    }

                }

                await client.CloseAsync();
            }
            catch (Exception e)
            {
                criticalError(string.Format("Unknown error. Here are the details: \n {0}", e.Message));
            }

        }

        private async static void TryListAllFoldersAsync(ImapClientWSSL client)
        {
            try
            {
                Task<ImapCommandResult> list = client.ListFolders();
                ImapCommandResult results = await LoadingWhileTask(list);
                if (results.CommandState == Enum.ImapCommandState.Ok)
                {
                    Console.WriteLine(results.text);
                    string[] folders = results.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (var VARIABLE in folders)
                    {

                    }
                }
                else
                {
                    throw new ApplicationException("Can't list folders");
                }


            }
            catch (Exception e)
            {
                criticalError(string.Format("Unknown error. Here are the details: \n {0}", e.Message));
            }
        }

        private async static void TryLoginAsync(string usernameForLoging, string passwordForLoging, ImapClientWSSL client)
        {
            try
            {
                Task<ImapCommandResult> login = client.LoginAsync(usernameForLoging, passwordForLoging);
                ImapCommandResult results = await LoadingWhileTask(login);
                if (results.CommandState != Enum.ImapCommandState.Ok)
                {
                    throw new AuthenticationException("Failed to login. Check provided credentials");
                }
            }
            catch (AuthenticationException e)
            {
                criticalError(e.Message);
            }
            catch (Exception e)
            {
                criticalError(string.Format("Unknown error. Here are the details: \n {0}", e.Message));
            }
        }

        private async static void TryConnectAsync(string host, int port, ImapClientWSSL client)
        {
            try
            {
                Task<ImapCommandResult> connect = client.ConnectAsync(host, port);
                ImapCommandResult results = await LoadingWhileTask(connect);
            }
            catch (AuthenticationException ae)
            {
                criticalError("Could not connect to server. Please check host and port provided.");
            }
            catch (Exception e)
            {
                criticalError(string.Format("Unknown error. Here are the details: \n {0}", e.Message));
            }

        }


        static async Task<ImapCommandResult> LoadingWhileTask(Task<ImapCommandResult> task)
        {
            int count = 0;
            int currentPos;
            int newPos;
            Console.Write("Loading");
            while (!task.IsCompleted)
            {
                Thread.Sleep(500);
                count++;
                Console.Write('.');
                Thread.Sleep(500);
                if (count == 3)
                {
                    count = 0;
                    Console.SetCursorPosition(Console.CursorLeft - 3, Console.CursorTop);
                    Console.Write("   ");
                    Console.SetCursorPosition(Console.CursorLeft - 3, Console.CursorTop);
                }


            }
            Console.Clear();
            return await task;
        }

        private static void criticalError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: {0}", msg);
            Console.WriteLine("Program exits");
            Console.Read();
            System.Environment.Exit(0);
        }
    }

}
