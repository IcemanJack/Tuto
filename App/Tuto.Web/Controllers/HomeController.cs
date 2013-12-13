using System.Web.Mvc;
using AutoMapper;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;
using Tuto.Web.ViewModels.HelpRequest;

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
    }
}
