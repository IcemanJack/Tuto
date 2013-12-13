using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using iTextSharp.text;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;
using Tuto.Web.ViewModels.GroupSession;

namespace Tuto.Web.Controllers
{
    public class GroupSessionController : DefaultController
    {
        public GroupSessionController(WebAppLaunchContext context) : base(context)
        {
            this.setAccessType(PageAccessType.TYPE_TUTOR);
        }

        public GroupSessionController(): this(new WebAppLaunchContext())
        { }

        public virtual ActionResult tutorList()
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            var tutor = this.getLoggedInUser() as Tutor;
            
            var tutorListViewModel = Mapper.Map<List<GroupSessionListViewModel>>(tutor.groupSessions);

            return View("Tutor_List", tutorListViewModel);
        }

        public virtual ActionResult tutorRefuseGroupSession(int id)
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            var tutor = this.getLoggedInUser() as Tutor;

            var repository = this.appContext.getRepository();

            var groupSession = repository.getById<AssignedGroupSession>(id);
            groupSession.tutor = null;

            repository.update(groupSession);

            return this.tutorList();
        }
    }
}
