using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;



namespace DailyPriceNotifier_ExtensionObejcts.Notifiers
{
    public class EmailNotifierExtension : IExternalNotifierExtension
    {

        private string _host => ConfigurationManager.AppSettings["MailHost"];
        private int _port => Int32.Parse(ConfigurationManager.AppSettings["MailHostPort"]);
        private int _timeout = Int32.Parse(ConfigurationManager.AppSettings["MailSendTimeout"]);

        public string FromEmail = ConfigurationManager.AppSettings["MailAddress"];
        public string ToEmail = ConfigurationManager.AppSettings["PersonalMail"];

        private string userName => ConfigurationManager.AppSettings["MailAddress"];
        private string psw => ConfigurationManager.AppSettings["MailPsw"];

        private ICredentialsByHost _mailCredentials;

        private Notifier _baseNotifier;

        public EmailNotifierExtension(Notifier notifier)
        {
            _mailCredentials = new NetworkCredential()
            {
                UserName = userName,
                Password = psw
            };

            _baseNotifier = notifier;

        }

        private void SendNotificationToEmail(string message)
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
            mailMessage.Subject = "Price notification!";
            mailMessage.Body = message;

            // await client.SendMailAsync(message);
            client.Send(mailMessage);
        }

        public void Notify()
        {
            SendNotificationToEmail(_baseNotifier.Message);
            _baseNotifier.OnSuccessAppendLog("EmailNotifier");
        }

 

    }
}
