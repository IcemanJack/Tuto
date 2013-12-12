using System.Web.Mvc;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;

namespace Tuto.Web.Controllers
{
    public class GroupSessionController : DefaultController
    {
        public GroupSessionController(WebAppLaunchContext context) : base(context)
        {
            this.setAccessType(PageAccessType.TYPE_MANAGER);
        }

        public GroupSessionController(): this(new WebAppLaunchContext())
        { }

        public virtual ActionResult tutorList()
        {
            var tutor = this.getLoggedInUser() as Tutor;
            if (tutor == null) return this.kickUser();

            return View("Tutor_List", tutor.groupSessions);
        }

        public virtual ActionResult tutorRefuseGroupSession(int id)
        {
            var tutor = this.getLoggedInUser() as Tutor;
            if (tutor == null) return this.kickUser();

            var repository = this.appContext.getRepository();

            var groupSession = repository.getById<AssignedGroupSession>(id);
            groupSession.tutor = null;

            repository.update(groupSession);

            return this.tutorList();
        }
    }
}
