using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Notifications.Tutor;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;
using Tuto.Web.Controllers.Utilities;
using Tuto.Web.Utilities;
using Tuto.Web.ViewModels;
using WebGrease.Css.Extensions;

namespace Tuto.Web.Controllers.Manager
{
    public class GroupSessionMgrController : DefaultController
    {
        public GroupSessionMgrController(WebAppLaunchContext context) : base(context)
        {
            this.setAccessType(PageAccessType.TYPE_MANAGER);
        }

        public GroupSessionMgrController() : this(new WebAppLaunchContext()) { }

        public virtual ActionResult index()
        {
            return this.list();
        }

        public virtual ActionResult editDefaultSchedule()
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            var currentManager = this.getLoggedInUser() as DataLayer.Models.Users.Manager;

            var groupSessionScheduleViewModel = new GroupSessionScheduleViewModel { jsonGroupSessions = GroupSessionUtilities.getJsonFromDefaultGroupSessions(currentManager.defaultGroupSessions) };
            return View("EditDefaultSchedule", groupSessionScheduleViewModel);
        }

        [HttpPost]
        public virtual ActionResult editDefaultSchedule(GroupSessionScheduleViewModel groupSessionScheduleViewModel)
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            
            var currentManager = this.getLoggedInUser() as DataLayer.Models.Users.Manager;
            var repository = this.appContext.getRepository();

            currentManager.defaultGroupSessions.ForEach(x => this.appContext.getRepository().delete<DefaultGroupSession>(x.id));
            currentManager.defaultGroupSessions = GroupSessionUtilities.getDefaultGroupSessionsFromJson(groupSessionScheduleViewModel.jsonGroupSessions, repository);
            currentManager.hasDefaultSchedule = true;

            repository.update(currentManager);

            this.ViewData["success"] = true;
            return View("EditDefaultSchedule", groupSessionScheduleViewModel);
        }

        public virtual ActionResult editCurrentSchedule()
        {
            var currentManager = this.getLoggedInUser() as DataLayer.Models.Users.Manager;
            if (!currentManager.hasDefaultSchedule) return this.kickUser();

            var currentGroupSessionScheduleViewModel = new GroupSessionScheduleViewModel { jsonGroupSessions = GroupSessionUtilities.getJsonFromDefaultGroupSessions(currentManager.defaultGroupSessions) };
            return View("EditCurrentSchedule", currentGroupSessionScheduleViewModel);
        }

        [HttpPost]
        public virtual ActionResult editCurrentSchedule(GroupSessionScheduleViewModel groupSessionScheduleViewModel)
        {
            var currentManager = this.getLoggedInUser() as DataLayer.Models.Users.Manager;
            var currentWeekSchedule = currentManager.getCurrentGroupSessionWeekSchedule();
            var repository = this.appContext.getRepository();

            currentWeekSchedule.groupSessions.ForEach(x =>  repository.delete<AssignedGroupSession>(x.id));
            currentWeekSchedule.groupSessions.Clear();

            var defaultGroupSessions = GroupSessionUtilities.getDefaultGroupSessionsFromJson(groupSessionScheduleViewModel.jsonGroupSessions, repository);
            foreach (var defaultGroupSession in defaultGroupSessions)
            {
                var assignedGroupSession = new AssignedGroupSession()
                {
                    place = defaultGroupSession.place,
                    startScheduleBlock = defaultGroupSession.startScheduleBlock
                };

                currentWeekSchedule.groupSessions.Add(assignedGroupSession);
            }

            repository.update(currentWeekSchedule);

            this.ViewData["success"] = true;
            return View("EditDefaultSchedule", groupSessionScheduleViewModel);
        }

        public ActionResult list()
        {
            var currentManager = this.getLoggedInUser() as DataLayer.Models.Users.Manager;

            if (!currentManager.hasDefaultSchedule)
            {
                return this.View("GroupSessionList", new GroupSessionListViewModel { assignedGroupSessions = Enumerable.Empty<AssignedGroupSession>().ToArray() });
            }

            var currentWeekSchedule = currentManager.getCurrentGroupSessionWeekSchedule();

            if (currentWeekSchedule == null || currentWeekSchedule.weekStartDay.Date < GroupSessionWeekSchedule.getNextWeekStartDay().Date)
            {
                var groupSessionWeekSchedule = new GroupSessionWeekSchedule
                {
                    manager = currentManager,
                    weekStartDay = GroupSessionWeekSchedule.getNextWeekStartDay()
                };

                foreach (var defaultGroupSession in currentManager.defaultGroupSessions)
                {
                    var assignedGroupSession = new AssignedGroupSession()
                    {
                        place = defaultGroupSession.place,
                        startScheduleBlock = defaultGroupSession.startScheduleBlock
                    };

                    groupSessionWeekSchedule.groupSessions.Add(assignedGroupSession);
                }

                currentManager.groupSessionWeekSchedules.Add(groupSessionWeekSchedule);

                this.appContext.getRepository().update(currentManager);

                return this.View("GroupSessionList", new GroupSessionListViewModel { assignedGroupSessions = groupSessionWeekSchedule.groupSessions });
            }
            else
            {
                return this.View("GroupSessionList", new GroupSessionListViewModel { assignedGroupSessions = currentManager.getCurrentGroupSessionWeekSchedule().groupSessions });   
            }
        }

        [HttpPost]
        public virtual ActionResult ajax_AssignTutor(int sessionId, int tutorId)
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            var repository = this.appContext.getRepository();

            var session = repository.getById<AssignedGroupSession>(sessionId);
            var tutor = repository.getById<Tutor>(tutorId);

            if (session == null || tutor == null) return this.Content("Error");

            session.tutor = tutor;
            repository.update(session);

            // notification
            var groupSessionNotification = new AssignedToGroupSessionAlert().getBuilder().setConcernedGroupSession(session).getNotification();
            repository.add(groupSessionNotification);

            return this.Content("Success");
        }

        [HttpPost]
        public virtual ActionResult ajax_GetTutorsAvailableAt(TutorAvailableAtViewModel tutorAvailableAtViewModel)
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            var repository = this.appContext.getRepository();

            // get available tutors
            var availableTutors = UsersUtilities.TutorsUtilities.getListOfAvailableTutorsAt(repository, tutorAvailableAtViewModel);
            var availableTutorsArray = Mapper.Map<List<GroupSessionAssignTutorViewModel>>(availableTutors).ToArray();

            return this.Json(availableTutorsArray, JsonRequestBehavior.AllowGet);
        }
    }
}