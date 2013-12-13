using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Notifications.Manager;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.ModelUtilities;
using Tuto.Web.Config;
using Tuto.Web.ViewModels.HelpRequest;

namespace Tuto.Web.Controllers
{
    public partial class HelpRequestController : DefaultController
    {
        public HelpRequestController(WebAppLaunchContext context) : base(context)
        {
            this.setAccessType(PageAccessType.TYPE_HELPED_OR_TUTOR);
        }

        public HelpRequestController() : this(new WebAppLaunchContext())
        {}

        [HttpGet]
        public virtual ActionResult index()
        {
            return this.RedirectToAction("list", "HelpRequest");
        }

        [HttpGet]
        public virtual ActionResult list()
        {
            if (!this.isUserAllowed()) return this.kickUser();

            var loggedInUser = this.getLoggedInUser();

            if (loggedInUser.isTutor())
            {
                var viewModel = ((Tutor) loggedInUser).helpRequests
                    .Select(Mapper.Map<TutorHelpRequestListEntryViewModel>).ToList();

                return View("Tutor_List", viewModel);
            }
            else // isHelped
            {
                var helped = loggedInUser as Helped;

                var helpRequestViewModel = new HelpedHelpRequestListViewModel
                {
                    allHelpRequests = Mapper.Map<List<HelpedHelpRequestListViewModel.HelpRequestViewModel>>(helped.helpRequests),
                    assignedHelpRequests = Mapper.Map<List<HelpedHelpRequestListViewModel.AssignedRequestViewModel>>(helped.helpRequests.Where(x => x.getState() != HelpRequestState.NOT_ASSIGNED && x.getState() != HelpRequestState.FINISHED).ToList())
                };

                return View("Helped_List", helpRequestViewModel);
            }
        }

        [HttpGet]
        public virtual ActionResult confirm(int id)
        {
            if (!this.isUserAllowed()) return this.kickUser();

            var repository = this.appContext.getRepository();

            var helpRequest = repository.getById<HelpRequest>(id);
            if (helpRequest != null)
            {
                var loggedInUser = this.getLoggedInUser();
                if (loggedInUser.isHelped())
                {
                    helpRequest.helpedHasConfirmed = true;
                }
                else
                {
                    helpRequest.tutorHasConfirmed = true;
                }
                repository.update(helpRequest);
            }

            return this.RedirectToAction("list", "HelpRequest");
        }

        [HttpGet]
        public virtual ActionResult refuse(int id)
        {
            if (!this.isUserAllowed()) return this.kickUser();

            var repository = this.appContext.getRepository();

            var helpRequest = repository.getById<HelpRequest>(id);
            if (helpRequest != null)
            {
                helpRequest.tutor = null;
                if (helpRequest.individualSession != null)
                {
                    repository.delete<IndividualSession>(helpRequest.individualSession.id);
                    helpRequest.individualSession = null; 
                }
                repository.update(helpRequest);
            }

            return this.RedirectToAction("list", "HelpRequest");
        }

        [HttpGet]
        public virtual ActionResult report(int id)
        {
            if (!this.isLoggedIn()) return this.kickUser();

            if (this.appContext.getRepository().single<IndividualSession>(x => x.helpRequestId == id) != null)
            {   
                var showViewModel = new ReportToDoViewModel {helpRequestId = id};
                return View("Report", showViewModel);
            }
            return this.RedirectToAction("list", "HelpRequest");
        }

        [HttpPost]
        public virtual ActionResult report(ReportToDoViewModel ReportToDoViewModel)
        {
            if (!this.isLoggedIn())
            {
                return this.kickUser();
            }

            if (!this.ModelState.IsValid)
            {
                return View("Report", ReportToDoViewModel);
            }

            var helpedRequest = this.appContext.getRepository()
                .single<HelpRequest>(x => x.id == ReportToDoViewModel.helpRequestId);
            var individualSession = helpedRequest.individualSession;

            if (this.getLoggedInUser().isTutor())
            {
                individualSession.tutorMessage = ReportToDoViewModel.message;
                this.appContext.getRepository().update(individualSession);
            }

            if (this.getLoggedInUser().isHelped())
            {
                individualSession.helpedMessage = ReportToDoViewModel.message;
                this.appContext.getRepository().update(individualSession);
            }

            return this.RedirectToAction("list", "HelpRequest");
        }

        [HttpGet]
        public virtual ActionResult details(int id)
        {
            var listDetails = this.appContext.getRepository().getById<HelpRequest>(id);

            if (listDetails != null)
            {
                var listDetailsViewModel = Mapper.Map<HelpRequestListDetailsViewModel>(listDetails);
                return View(listDetailsViewModel);
            }
            return this.HttpNotFound();
        }

        [HttpGet]
        public virtual ActionResult create()
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            // load courses list
            var showViewModel = new HelpRequestAddViewModel
            {
                availableCourseList = this.appContext.getRepository().getAll<Course>().ToArray()
            };

            return View("Create", showViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult create(HelpRequestAddViewModel helpRequestView)
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            helpRequestView.availableCourseList = this.appContext.getRepository().getAll<Course>().ToArray();
            // validate modelstate
            if (!this.ModelState.IsValid)
            {
                return View("Create", helpRequestView);
            }

            // validate course id
            if (helpRequestView.courseId < 0)
            {
                this.ModelState.AddModelError("ErrorInvalidCourseId", Resources.Resources.ErrorCreateHelpRequest);
                return View("Create", helpRequestView);
            }

            // validate received json syntax
            if (!ScheduleUtilities.validateScheduleJsonString(helpRequestView.scheduleJson))
            {
                this.ModelState.AddModelError("InvalidScheduleJson", "Invalid schedule syntax");
                return View("Create", helpRequestView);
            }

            // create model
            var toCreateHelpRequest = Mapper.Map<HelpRequest>(helpRequestView);
            toCreateHelpRequest.helped = (Helped) this.getLoggedInUser();
            toCreateHelpRequest.course = this.appContext.getRepository().getById<Course>(helpRequestView.courseId);
            toCreateHelpRequest.tutor = null;
            this.appContext.getRepository().add(toCreateHelpRequest);

            this.ViewBag.HelpRequestCreateSuccess = true;
            this.ModelState.Clear(); // clear the model state
            
            // notification
            var newHelpRequestManagerAlert =
                new HelpRequestToAssignAlert().getBuilder()
                    .setConcernedHelpRequest(toCreateHelpRequest)
                    .setConcernedManager(this.appContext.getConfiguration().mainManager)
                    .getNotification();
            this.appContext.getRepository().add(newHelpRequestManagerAlert);

            // Reset view model
            helpRequestView = new HelpRequestAddViewModel();
            helpRequestView.availableCourseList = this.appContext.getRepository().getAll<Course>().ToArray();
            
           return View("Create", helpRequestView);
        }


    }
}