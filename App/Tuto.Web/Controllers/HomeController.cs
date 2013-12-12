using System.Web.Mvc;
using AutoMapper;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;
using Tuto.Web.ViewModels;

namespace Tuto.Web.Controllers
{
    public partial class HomeController : DefaultController
    {
        public HomeController(WebAppLaunchContext context) : base(context)
        {
            this.setAccessType(PageAccessType.TYPE_USER);
        }

        public HomeController() : this(new WebAppLaunchContext())
        {
        }

        public virtual ActionResult index()
        {
            if (!this.isLoggedIn()) return this.kickUser();

            var homeView = Mapper.Map<HomeViewModel>(this.getLoggedInUser());
            return View("Index", homeView);
        }

        private void getTasks()
        {
            var user = this.getLoggedInUser();
            if (user.isHelped())
            {
                var helped = user as Helped;
            }
            else if (user.isTutor())
            {
                var tutor = user as Tutor;
            }
            else // user is manager
            {
                var manager = user as DataLayer.Models.Users.Manager;
            }
        }


    }
}
