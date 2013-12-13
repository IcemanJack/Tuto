using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Web.Mvc;
using Tuto.DataLayer;

namespace Tuto.Web.Controllers
{
    public class CIController : Controller
    {
        public virtual ActionResult Index()
        {
            try
            {
                var context = new TutoContext();
                SqlConnection.ClearAllPools();
                Database.SetInitializer(new DatabaseInitializer());

                //context.Database.Initialize(true);
            }
            catch (Exception ex)
            {
                return this.Content(ex.Message + "<br /><br />" + ex.StackTrace);
            }

            return this.Content("Ok <a href=\"\\\">Go Home, you're drunk!</a> ");
        }

    }
}
