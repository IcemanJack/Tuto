using System;
using System.Net;
using System.Net.Mail;
using Tuto.Web.Config;

namespace Tuto.Web.Utilities
{
    public class EmailSender
    {
        protected SmtpClient smtp;
        protected MailAddress fromAddress;

        public EmailSender()
        {
            this.smtp = null;
            this.fromAddress = null;
        }

        public EmailSenderBuilder getBuilder()
        {
            return new EmailSenderBuilder(this);
        }

        public bool sendMail(MailMessage mailToSend)
        {
            if (this.smtp == null)
            {
                throw new NotAConfigredEmailSenderInstanceException();
            }

            try
            {
                mailToSend.From = this.fromAddress;
                this.smtp.Send(mailToSend);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public class EmailSenderBuilder
        {
            private readonly EmailSender buildInstance;

            public EmailSenderBuilder(EmailSender buildInstance)
            {
                this.buildInstance = buildInstance;
            }

            public EmailSenderBuilder loadSmtpConfigurationFromAppSettings()
            {
                this.buildInstance.fromAddress = new MailAddress(WebAppConfiguration.SmtpEmailConfiguration.SEND_EMAIL_ADDRESS, WebAppConfiguration.SmtpEmailConfiguration.SEND_EMAIL_NAME);
                this.buildInstance.smtp = new SmtpClient()
                {
                    Host = WebAppConfiguration.SmtpEmailConfiguration.SMTP_HOSTNAME,
                    Port = WebAppConfiguration.SmtpEmailConfiguration.SMTP_PORT,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(this.buildInstance.fromAddress.Address, WebAppConfiguration.SmtpEmailConfiguration.SMTP_PASSWORD)
                };

                return this;
            }

            public EmailSender getSender()
            {
                if (this.buildInstance.smtp == null)
                {
                    this.loadSmtpConfigurationFromAppSettings();
                }

                return this.buildInstance;
            }
        }

        public class NotAConfigredEmailSenderInstanceException : Exception
        {
            public NotAConfigredEmailSenderInstanceException() 
                : base("This emailSender instance isn't configured properly... please use the built-in builder to configure the instance.")
            { }
        }
    }
}