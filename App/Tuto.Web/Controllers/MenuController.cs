using System.Collections.Generic;
using System.Web.Mvc;
using Tuto.DataLayer;
using Tuto.Web.Config;
using Tuto.Web.ViewsUtilities;

namespace Tuto.Web.Controllers
{
    public class MenuController : DefaultController
    {
        private static readonly List<MenuItem> HELPED_MENU_ITEMS = new List<MenuItem>();
        private static readonly List<MenuItem> TUTOR_MENU_ITEMS = new List<MenuItem>();
        private static readonly List<MenuItem> MANAGER_MENU_ITEMS = new List<MenuItem>();

        static MenuController()
        {
            var homeMenuItem = new MenuItem()
            {
                name = Resources.Resources.DisplayHomeMenuItem,
                description = Resources.Resources.DisplayHomeDescriptionMenuItem,
                icon = "awe-home",
                controller = "Home",
                action = "index"
            };

            var helpedHelpRequestMenuItem = new MenuItem()
            {
                name = Resources.Resources.DisplayHelpRequestMenuItem,
                description = Resources.Resources.DisplayHelpRequestDescriptionMenuItem,
                icon = "awe-tasks",
                controller = "HelpRequest",
                action = "list"
            };

            var tutorIndividualSessionMenuItem = new MenuItem()
            {
                name = Resources.Resources.DisplayIndividualSessionMenuItem,
                description = Resources.Resources.DisplayIndividualSessionDescriptionMenuItem,
                icon = "awe-tasks",
                controller = "HelpRequest",
                action = "list"
            };

            var tutorListMenuItem = new MenuItem()
            {
                name = Resources.Resources.DisplayTutorListMenuItem,
                description = Resources.Resources.DisplayTutorListDescriptionMenuItem,
                icon = "awe-user",
                controller = "Manager/Tutors",
                action = "list"
            };

            var individualSessionManagerMenuItem = new MenuItem()
            {
                name = Resources.Resources.DisplayIndividualSessionManagerMenuItem,
                description = Resources.Resources.DisplayIndividualSessionManagerDescriptionMenuItem,
                icon = "awe-tasks",
                controller = "Manager/HelpRequests",
                action = "list"
            };

            var groupSessionManagerMenuItem = new MenuItem()
            {
                name = Resources.Resources.DisplayGroupSessionMenuItem,
                description = Resources.Resources.DisplayGroupSessionDescriptionMenuItem,
                icon = "awe-group",
                controller = "Manager/GroupSessions",
                action = "list"
            };

            var groupSessionTutorMenuItem = new MenuItem()
            {
                name = GroupSessionTutorListResources.MenuItemName,
                description = GroupSessionTutorListResources.MenuItemDescription,
                icon = "awe-group",
                controller = "GroupSession",
                action = "tutorList"
            };

            var reportsManagerMenuItem = new MenuItem()
            {
                name = "Rapports",
                description = GroupSessionTutorListResources.MenuItemDescription,
                icon = "awe-group",
                controller = "Manager/Reports",
                action = "show"
            };



            HELPED_MENU_ITEMS.Add(homeMenuItem);
            HELPED_MENU_ITEMS.Add(helpedHelpRequestMenuItem);

            TUTOR_MENU_ITEMS.Add(homeMenuItem);
            TUTOR_MENU_ITEMS.Add(tutorIndividualSessionMenuItem);
            TUTOR_MENU_ITEMS.Add(groupSessionTutorMenuItem);

            MANAGER_MENU_ITEMS.Add(homeMenuItem);
            MANAGER_MENU_ITEMS.Add(individualSessionManagerMenuItem);
            MANAGER_MENU_ITEMS.Add(tutorListMenuItem);
            MANAGER_MENU_ITEMS.Add(groupSessionManagerMenuItem);
            MANAGER_MENU_ITEMS.Add(reportsManagerMenuItem);
        }

        public MenuController(WebAppLaunchContext context) : base(context)
        {
        }

        public MenuController() : base(new WebAppLaunchContext())
        {
        }

        [ChildActionOnly]
        public PartialViewResult buildMenu()
        {
            var currentUser = this.getLoggedInUser();
            if (currentUser == null) return this.PartialView("_Menu", TUTOR_MENU_ITEMS);

            this.ViewData["user_name"] = currentUser.name + " " + currentUser.lastName;
            if (currentUser.isHelped())
            {
                this.ViewData["user_role"] = Resources.Resources.DisplayHelped;
                return this.PartialView("_Menu", HELPED_MENU_ITEMS);
            }
            else if (currentUser.isTutor())
            {
                this.ViewData["user_role"] = Resources.Resources.DisplayTutor;
                return this.PartialView("_Menu", TUTOR_MENU_ITEMS);
            }
            else
            {
                this.ViewData["user_role"] = "Administrateur";
                return this.PartialView("_Menu", MANAGER_MENU_ITEMS);
            }
 
        }
    }
}
