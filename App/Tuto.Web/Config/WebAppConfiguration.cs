using Tuto.DataLayer.Models.Users;

namespace Tuto.Web.Config
{
    public class WebAppConfiguration
    {
        public const string MAIN_MANAGER_EMAIL = "fbertrand@mail.com";

        // specifies the main Manager of the system

        // Default configuration of the application (can be overrided on application launch)
        static WebAppConfiguration()
        {
            defaultHoursPerSession = 2;
        }

        public WebAppConfiguration()
        {
            this.mainManager = null;
        }

        public static int defaultHoursPerSession { get; set; }
        public Manager mainManager { get; set; }
    }
}