using System.Net.Mail;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Utilities;

namespace Tuto.Web.Config
{
    public class WebAppConfiguration
    {
        public const string MAIN_MANAGER_EMAIL = "fbertrand@mail.com";

        public static class SmtpEmailConfiguration
        {
            public const string SEND_EMAIL_ADDRESS = "tutocalinours@gmail.com";
            public const string SEND_EMAIL_NAME = "Calinours Administrateur";
            public const string SMTP_PASSWORD = "calinours123";
            public const string SMTP_HOSTNAME = "smtp.gmail.com";
            public const int SMTP_PORT = 587;

            public static MailAddress SEND_FROM_ADDRESS = new MailAddress(SEND_EMAIL_ADDRESS, SEND_EMAIL_NAME);
        }

        // specifies the main Manager of the system

        // Default configuration of the application (can be overrided on application launch)
        static WebAppConfiguration()
        {
            defaultHoursPerSession = 2;
        }

        public WebAppConfiguration()
        {
            this.mainManager = null;
            mailSender = new EmailSender()
                            .getBuilder()
                            .loadSmtpConfigurationFromAppSettings()
                            .getSender();
        }

        public static int defaultHoursPerSession { get; set; }
        public EmailSender mailSender { get; set; }
        public Manager mainManager { get; set; }
    }
}