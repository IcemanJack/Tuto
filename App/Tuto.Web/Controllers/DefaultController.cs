using System;
using System.Web.Mvc;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.ModelUtilities;
using Tuto.Web.Config;
using Tuto.Web.Mappers;

namespace Tuto.Web.Controllers
{
    public enum PageAccessType : int
    {
        TYPE_NONE = 0,
        TYPE_USER = 1,
        TYPE_TUTOR = 2,
        TYPE_MANAGER = 3,
        TYPE_HELPED_OR_TUTOR = 4
    };

    public partial class DefaultController : Controller
    {
        protected WebAppLaunchContext appContext;
        private PageAccessType currentPageAccessType = PageAccessType.TYPE_NONE;
        private RedirectToRouteResult redirectTo;

        protected DefaultController(WebAppLaunchContext context)
        {
            AutoMapperConfiguration.Configure();
            this.appContext = context;
            this.redirectTo = this.RedirectToAction("unauthorized", "Account");
        }

        protected void setAccessType(PageAccessType type)
        {
            this.currentPageAccessType = type;
        }

        protected void setRedirect(RedirectToRouteResult redirect)
        {
            this.redirectTo = redirect;
        }

        protected RedirectToRouteResult kickUser()
        {
            return this.redirectTo;
        }

        protected bool isUserAllowed(PageAccessType access)
        {
            // check user login details
            User loggedInUser = this.getLoggedInUser();
            if (loggedInUser == null)
            {
                return false;
            }

            //TODO : check user permission to display current page using account type in user model
            switch (access)
            {
                case PageAccessType.TYPE_USER:
                    return loggedInUser.isHelped();
                case PageAccessType.TYPE_TUTOR:
                    return loggedInUser.isTutor();
                case PageAccessType.TYPE_MANAGER: // when logged in as manager, we must be the mainManager absolutely
                    return loggedInUser.isManager() && loggedInUser.Equals(this.appContext.getConfiguration().mainManager);
                case PageAccessType.TYPE_HELPED_OR_TUTOR:
                    return loggedInUser.isHelped() || loggedInUser.isTutor();
                default:
                    return true;
            }
        }

        protected bool isLoggedIn()
        {
            try
            {
                return this.appContext.getHttpContext().Session["loggedIn"] != null && this.appContext.getHttpContext().Session["mail"] != null && this.appContext.getHttpContext().Session["password"] != null;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        protected User getLoggedInUser()
        {
            if (!this.isLoggedIn())
            {
                return null;
            }

            var mail = this.appContext.getHttpContext().Session["mail"].ToString();
            var password = this.appContext.getHttpContext().Session["password"].ToString();

            // check user in db (make sure the login is valid)
            var repo = this.appContext.getRepository();
            var loggedInUser = AccountLoginUtilities.getUserFromLogin(this.appContext.getRepository(), mail);

            return loggedInUser;
        }

        public void updateSessionCreditentials(String mail, String password)
        {
            this.appContext.getHttpContext().Session["mail"] = mail;
            this.appContext.getHttpContext().Session["password"] = password;
        }

        protected bool isUserAllowed()
        {
            return this.isUserAllowed(this.currentPageAccessType);
        }

        protected void setLoggedInUser(User userToLogin)
        {
            this.appContext.getHttpContext().Session["loggedIn"] = true;
            this.appContext.getHttpContext().Session["mail"] = userToLogin.mail;
            this.appContext.getHttpContext().Session["password"] = userToLogin.password;
        }

        protected RedirectToRouteResult disconnectLoggedInUser()
        {
            this.appContext.getHttpContext().Session.RemoveAll(); // reset session
            return this.RedirectToAction("login", "Account");
        }

        public class UserNotAllowedException : Exception
        { }
    }
}