using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Exceptions.HelpRequest;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Notifications.Shared;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;
using Tuto.Web.Controllers.Utilities;
using Tuto.Web.ViewModels.HelpRequestMgr;

namespace Tuto.Web.Controllers.Manager
{
    public class HelpRequestMgrController : DefaultController
    {
        public enum StudentType : int
        {
            Helped = 0,
            Tutor = 1
        }

        public HelpRequestMgrController(WebAppLaunchContext context) : base(context)
        {
            this.setAccessType(PageAccessType.TYPE_MANAGER);
        }

        public HelpRequestMgrController() : base(new WebAppLaunchContext())
        { }

        public virtual ActionResult list()
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            var managerHelpRequests = new ManagerHelpRequestsListViewModel();

            // to be assigned requests
            managerHelpRequests.toBeConfirmedSystemHelpRequests =
                Mapper.Map<List<ManagerHelpRequestsListViewModel.ToBeConfirmedHelpRequestViewModel>>(
                    this.appContext.getRepository()
                        .getAll<HelpRequest>(HelpRequest.TO_BE_ASSIGNED_HELPREQUEST_FILTER).ToList());

            // confirmed requests
            managerHelpRequests.confirmedSystemHelpRequests =
                Mapper.Map<List<ManagerHelpRequestsListViewModel.ConfirmedHelpRequestViewModel>>(
                    this.appContext.getRepository()
                        .getAll<HelpRequest>(HelpRequest.ASSIGNED_HELPREQUESTS_FILTER).ToList());

            // finished requests
            managerHelpRequests.finishedSystemHelpRequests =
                Mapper.Map<List<ManagerHelpRequestsListViewModel.FinishedHelpRequestViewModel>>(
                    this.appContext.getRepository()
                        .getAll<HelpRequest>(HelpRequest.FINISHED_HELPREQUESTS_FILTER).ToList());

            return View("HelpRequestList", managerHelpRequests);
        }

        public virtual ActionResult assign(int id)
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            var theHelpRequest = this.appContext.getRepository().getById<HelpRequest>(id);
            if (theHelpRequest == null)
            {
                throw new HelpRequestNotFoundException();
            }

            var assignPageViewModel = Mapper.Map<ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel>(theHelpRequest);
            var availableTutorsList =
                this.appContext.getRepository().getAll<Tutor>(t => t.tutorAvailableForIndividualSession == true).ToList();

            assignPageViewModel.availableTutors =
                Mapper.Map<List<ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel.TutorListEntryViewModel>>(availableTutorsList);

            return View("HelpRequestAssign", assignPageViewModel);
        }

        [HttpPost]
        public virtual ActionResult assign(ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel assignementData)
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            if (this.ModelState.IsValid)
            {
                var concernedHelpRequest = this.appContext.getRepository().getById<HelpRequest>(assignementData.helpRequestId);
                var assignedTutor = this.appContext.getRepository().getById<Tutor>(assignementData.assignTutorId);
                
                // create the individual session
                var assignedIndividualSession = new IndividualSession()
                {
                    date = assignementData.getSessionStartDateTime(),
                    place = assignementData.localName,
                    helpRequest = concernedHelpRequest
                };

                // assign the tutor to the help request
                concernedHelpRequest.tutor = assignedTutor;
                concernedHelpRequest.individualSession = assignedIndividualSession;

                this.appContext.getRepository().add(assignedIndividualSession);
                this.appContext.getRepository().update(concernedHelpRequest);

                // notification
                this.appContext.getRepository().add(new AssignedToHelpRequestTask().getBuilder().setConcernedHelpRequest(concernedHelpRequest).getNotification());

                return this.RedirectToAction("list");
            }
            else
            {
                return assign(assignementData.helpRequestId);
            }
        }

        [HttpPost]
        public virtual ActionResult ajax_GetTutorsAvailableAt(ManagerHelpRequestTutorAssignmentViewModels.GetTutorAvailableForGivenBlockViewModel blockParams)
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            var concernedHelpRequest = this.appContext.getRepository().getById<HelpRequest>(blockParams.helpRequestId);
            if (concernedHelpRequest == null)
            {
                return this.Content("Error");
            }

            var repository = this.appContext.getRepository();
            
            // get available tutors
            var availableTutors = UsersUtilities.TutorsUtilities.getListOfAvailableTutorsAt(repository, blockParams.dayOfWeek, blockParams.startHour, blockParams.endHour)
                                    .Where(
                                        t => t.coursesSkills.Contains(concernedHelpRequest.course) && 
                                            t.tutorAvailableForIndividualSession
                                    ).ToList() as List<Tutor>;

            var availableTutorsList = Mapper.Map<List<ManagerHelpRequestTutorAssignmentViewModels.TutorAvailableForGivenBlockViewModel>>(availableTutors).ToArray();

            return this.Json(availableTutorsList, JsonRequestBehavior.AllowGet);
        }

        // enum used into studentReport ajax method

        public virtual ActionResult ajax_studentReport(int helpRequestId, StudentType studentType)
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            string report = "";
            var concernedHelpRequest = this.appContext.getRepository().getById<HelpRequest>(helpRequestId);
            if (concernedHelpRequest == null)
            {
                throw new HelpRequestNotFoundException();
            }

            if (concernedHelpRequest.getState() != HelpRequestState.FINISHED)
            {
                throw new InvalidHelpRequestStateException(HelpRequestState.FINISHED);
            }

            if (studentType == StudentType.Tutor)
            {
                report = concernedHelpRequest.individualSession.tutorMessage;
            }
            else if (studentType == StudentType.Helped)
            {
                report = concernedHelpRequest.individualSession.helpedMessage;
            }

            return this.Json(report, JsonRequestBehavior.AllowGet);
        }
    }
}
