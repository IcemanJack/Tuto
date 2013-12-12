using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tuto.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Manager HelpRequests 
            routes.MapRoute(
                name: "manager_helprequest",
                url: "Manager/HelpRequests/{action}/{id}",
                defaults: new { controller = "HelpRequestMgr", action = "list", id = UrlParameter.Optional },
                namespaces: new[] { "Tuto.Web.Controllers.Manager" }
            );

            // Manager Tutors 
            routes.MapRoute(
                name: "manager_tutors",
                url: "Manager/Tutors/{action}/{id}",
                defaults: new { controller = "TutorsMgr", action = "list", id = UrlParameter.Optional },
                namespaces: new[] { "Tuto.Web.Controllers.Manager" }
            );

            // Manager GroupSession 
            routes.MapRoute(
                name: "manager_groupsessions",
                url: "Manager/GroupSessions/{action}/{id}",
                defaults: new { controller = "GroupSessionMgr", action = "list", id = UrlParameter.Optional },
                namespaces: new[] { "Tuto.Web.Controllers.Manager" }
            );

            // Manager Reports
            routes.MapRoute(
                name: "manager_reports",
                url: "Manager/Reports/{action}/{id}",
                defaults: new { controller = "ReportsMgr", action = "show", id = UrlParameter.Optional },
                namespaces: new[] { "Tuto.Web.Controllers.Manager" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                namespaces:new[] { "Tuto.Web.Controllers" }
            );
        }
    }
}