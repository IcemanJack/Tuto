using System.Web.Mvc;

namespace Tuto.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult index()
        {
            return this.RedirectToAction("general", "Error");
        }

        public ActionResult general()
        {
            return this.View("General");
        }

        public ActionResult http404()
        {
            return this.View("Http404");
        }
    }
}
