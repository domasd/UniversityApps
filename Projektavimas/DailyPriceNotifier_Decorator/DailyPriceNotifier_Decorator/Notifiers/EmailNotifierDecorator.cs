using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DailyPriceNotifier_Decorator.Notifiers
{
    public class EmailNotifierDecorator : ExternalNotifierDecorator
    {

        private string _host => ConfigurationManager.AppSettings["MailHost"];
        private int _port => Int32.Parse(ConfigurationManager.AppSettings["MailHostPort"]);
        private int _timeout = Int32.Parse(ConfigurationManager.AppSettings["MailSendTimeout"]);

        public string FromEmail = ConfigurationManager.AppSettings["MailAddress"];
        public string ToEmail = ConfigurationManager.AppSettings["PersonalMail"];

        private string userName => ConfigurationManager.AppSettings["MailAddress"];
        private string psw => ConfigurationManager.AppSettings["MailPsw"];

        private ICredentialsByHost _mailCredentials;

        public EmailNotifierDecorator(INotifier notifier) : base(notifier)
        {
            _mailCredentials = new NetworkCredential()
            {
                UserName = userName,
                Password = psw
            };

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

        public override void Notify()
        {
            base.Notify();
            SendNotificationToEmail(base.Message);
            base.OnSuccessAppendLog("EmailNotifier");
        }

        public override int LoggedCount()
        {
            return base.LoggedCount() + 1;
        }

    }
}
