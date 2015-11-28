using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DailyPriceNotifier_ExtensionObejcts.Notifiers
{
    public class NirvanaNotifierExtension : IExternalNotifierExtension
    {

        private string _host => ConfigurationManager.AppSettings["MailHost"];
        private int _port => Int32.Parse(ConfigurationManager.AppSettings["MailHostPort"]);
        private int _timeout = Int32.Parse(ConfigurationManager.AppSettings["MailSendTimeout"]);

        public string FromEmail = ConfigurationManager.AppSettings["MailAddress"];
        public string ToEmail = ConfigurationManager.AppSettings["NirvanaMail"];

        private string userName => ConfigurationManager.AppSettings["MailAddress"];
        private string psw => ConfigurationManager.AppSettings["MailPsw"];

        private ICredentialsByHost _mailCredentials;

        private Notifier _baseNotifier;

        public NirvanaNotifierExtension(Notifier notifier) 
        {
            _mailCredentials = new NetworkCredential()
            {
                UserName = userName,
                Password = psw
            };

            _baseNotifier = notifier;
        }

        public void Notify()
        {
            this.SendEmailNirvana(_baseNotifier.Message);
            AddComponentNameToMessage();
            _baseNotifier.OnSuccessAppendLog("NirvanaNotifier");
        }

        private void SendEmailNirvana(string msg)
        {

            SmtpClient client = new SmtpClient(_host, _port)
            {
                Credentials = _mailCredentials,
                Timeout = _timeout
            };

            MailAddress fromAddress = new MailAddress(FromEmail);

            // Set destinations for the e-mail message.
            MailAddress toAddress = new MailAddress(ToEmail);

            // Specify the message content.
            MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
            mailMessage.Subject = "Check prices!";
            mailMessage.Body = msg;

            // await client.SendMailAsync(message);
            client.Send(mailMessage);
        }

        private void AddComponentNameToMessage()
        {
            _baseNotifier.Message += " (task created in Nirvana)";
        }
    }
}
