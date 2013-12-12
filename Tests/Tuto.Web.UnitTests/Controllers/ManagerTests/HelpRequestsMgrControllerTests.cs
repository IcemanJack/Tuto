using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers.Manager;
using Tuto.Web.ViewModels;

namespace Tuto.Web.UnitTests.Controllers.ManagerTests.HelpRequestsMgr
{
    [TestClass]
    public class HelpRequestMgrControllerTests : ManagerComponentsBaseTest
    {
        private const int individualSessionId = 1;
        private const string reportMessage = "Cool report message";
        private HelpRequestMgrController controller;

        [TestInitialize]
        public void helpRequestMgrTestInit()
        {
            this.controller = new HelpRequestMgrController(this.appContext);
        }

        /* =======================================
         *  List Action Unit tests
         * ====================================== */

        [TestMethod]
        public void list_action_should_return_list_view()
        {
            // Arrange

            // Act
            var viewResult = this.controller.list() as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            viewResult.AssertViewRendered().ForView("HelpRequestList");
        }

        [TestMethod]
        public void list_action_should_render_valid_entries()
        {
            // Arrange
            const int numOfEntries = 10;
            var helpRequests = this.fixture.CreateMany<HelpRequest>(numOfEntries);
            this.appContext.getRepository().getAll<HelpRequest>(null).ReturnsForAnyArgs(helpRequests.AsQueryable());
            var expectedToBeConfirmedHelpRequests = Mapper.Map<List<ManagerHelpRequestsListViewModel.ToBeConfirmedHelpRequestViewModel>>(helpRequests);

            // Act
            var viewResult = this.controller.list() as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);

            var viewModelObtained = viewResult.ViewData.Model as ManagerHelpRequestsListViewModel;
            Assert.IsNotNull(viewModelObtained);

            Assert.IsNotNull(viewResult.ViewData.Model);
            Assert.AreEqual(numOfEntries, viewModelObtained.toBeConfirmedSystemHelpRequests.Count);
            Assert.AreEqual(numOfEntries, viewModelObtained.confirmedSystemHelpRequests.Count);
            Assert.AreEqual(numOfEntries, viewModelObtained.finishedSystemHelpRequests.Count);
        }

        /* =======================================
         *  Assign Action unit tests
         * ====================================== */

        [TestMethod]
        public void assign_action_should_return_assign_view()
        {
            // Arrange
            var randomHelpRequest = this.fixture.Create<HelpRequest>();
            this.appContext.getRepository().getById<HelpRequest>(343).ReturnsForAnyArgs(randomHelpRequest);

            // Act
            ActionResult assignResult = this.controller.assign(3);

            // Assert
            Assert.IsNotNull(assignResult);
            assignResult.AssertViewRendered().ForView("HelpRequestAssign");
        }

        [TestMethod]
        public void assign_action_with_invalid_helprequest_id_should_fail()
        {
            // Arrange
            // Act
            bool http404Called = false;
            try
            {
                ActionResult assignResult = this.controller.assign(3434343);
                http404Called = false;
            }
            catch (FileNotFoundException)
            {
                http404Called = true;
            }

            // Assert
            Assert.IsTrue(http404Called, "The controller page didn't returned a 404 http error on invalid help request id");
        }

        [TestMethod]
        public void assign_action_with_valid_helprequest_should_render_correct_helprequest_data()
        {
            // Arrange
            var randomHelpRequest = this.fixture.Create<HelpRequest>();
            this.appContext.getRepository().getById<HelpRequest>(343).ReturnsForAnyArgs(randomHelpRequest);

            // Act
            var assignResult = this.controller.assign(232) as ViewResult;

            // Assert
            Assert.IsNotNull(assignResult);
            var resultViewModel = assignResult.Model as ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel;
            Assert.IsNotNull(resultViewModel);
            Assert.AreEqual(randomHelpRequest.id, resultViewModel.helpRequestId);
        }

        [TestMethod]
        public void assign_action_with_valid_helprequest_should_render_correct_available_tutor_list()
        {
            // Arrange
            var randomHelpRequest = this.fixture.Create<HelpRequest>();
            List<Tutor> randomTutors = this.fixture.CreateMany<Tutor>(10).ToList();
            this.appContext.getRepository().getAll<Tutor>(x => true).ReturnsForAnyArgs(randomTutors.AsQueryable());
            this.appContext.getRepository().getById<HelpRequest>(343).ReturnsForAnyArgs(randomHelpRequest);

            // Act
            var assignResult = this.controller.assign(343) as ViewResult;

            // Assert
            Assert.IsNotNull(assignResult);
            var viewModelResult = assignResult.Model as ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel;
            Assert.IsNotNull(viewModelResult);

            // make sure every tutor is the same
            var toCheckTutorsViewModel = Mapper.Map<List<ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel.TutorListEntryViewModel>>(randomTutors);
            Assert.AreEqual(toCheckTutorsViewModel.Count, viewModelResult.availableTutors.Count);
        }

        [TestMethod]
        public void post_assign_action_with_invalid_viewmodel_should_fail_and_rerender_assign_page()
        {
            // Arrange
            var invalidViewModel = this.fixture.Create<ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel>();
            invalidViewModel.helpRequestId = -1; // will always be invalid
            invalidViewModel.weekPickStart = "08/11/2013";
            invalidViewModel.weekPickEnd = "08/11/2013";
            this.controller.ModelState.AddModelError("UnitTest", "test"); // will automatically invalidate modelstate
            this.appContext.getRepository().getById<HelpRequest>(-1).Returns(this.fixture.Create<HelpRequest>()); // to make sure that the assign(int) method wont fail

            // Act
            var postAssignResult = this.controller.assign(invalidViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(postAssignResult);
            Assert.IsFalse(postAssignResult.ViewData.ModelState.IsValid);
            postAssignResult.AssertViewRendered().ForView("HelpRequestAssign");
        }

        [TestMethod]
        public void post_assign_with_valid_viewmodel_should_assign_the_helprequest_and_redirect_to_list_view()
        {
            // Arrange
            var validViewModel = this.fixture.Create<ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel>();
            validViewModel.weekPickStart = "2014/01/11"; // must be in the future
            validViewModel.weekPickEnd = "2014/01/16"; // must be in the future
            validViewModel.assignWeekDay = WeekDay.MONDAY; // monday of the 11th of august week is the aug. 12
            validViewModel.assignStartHour = 8;

            var assignedTutor = this.fixture.Create<Tutor>();
            var randomHelpRequest = this.fixture.Create<HelpRequest>();

            randomHelpRequest.helped = this.fixture.Create<Helped>();
            randomHelpRequest.tutor = assignedTutor;

            var expectedCreatedIndividualSession = new IndividualSession()
            {
                date = DateTime.Parse("2014/01/06 8:0:0"),
                place = validViewModel.localName,
                helpRequest = randomHelpRequest
            };

            this.appContext.getRepository().getById<HelpRequest>(343).ReturnsForAnyArgs(randomHelpRequest);
            this.appContext.getRepository().getById<Tutor>(343).ReturnsForAnyArgs(assignedTutor);

            // Act
            ActionResult postAssignResult = this.controller.assign(validViewModel);

            // Assert
            Assert.IsNotNull(postAssignResult);
            this.appContext.getRepository().Received().update(Arg.Is<HelpRequest>(randomHelpRequest));
            this.appContext.getRepository().Received().add(Arg.Is<IndividualSession>(x => x.date == expectedCreatedIndividualSession.date && 
                                                                                        x.place == expectedCreatedIndividualSession.place && 
                                                                                        x.helpRequest.id == expectedCreatedIndividualSession.helpRequest.id));
            postAssignResult.AssertActionRedirect().ToAction("list");
        }

        /* =======================================
         *  Get GetTutorsAvailableAt (ajax method) unit tests
         * ====================================== */

        [TestMethod]
        public void get_available_tutors_with_invalid_helprequest_id_should_return_error()
        {
            // Arrange
            var invalidViewModel = this.fixture.Create<ManagerHelpRequestTutorAssignmentViewModels.GetTutorAvailableForGivenBlockViewModel>();
            invalidViewModel.helpRequestId = -1; // will always be invalid

            // Act
            var resultContent = this.controller.ajax_GetTutorsAvailableAt(invalidViewModel) as ContentResult;

            // Assert
            Assert.IsNotNull(resultContent);
            Assert.AreEqual("Error", resultContent.Content);
        }

        [TestMethod]
        public void get_available_tutors_should_return_the_available_tutors_for_a_given_schedule_block()
        {
            // Arrange
            var randomHelpRequest = this.fixture.Create<HelpRequest>();
            var randomCourse = this.fixture.Create<Course>();
            var otherRandomCourse = this.fixture.Create<Course>();
            randomHelpRequest.course = randomCourse;

            var testsTutors = this.fixture.CreateMany<Tutor>(10).ToList();
            testsTutors[0].scheduleBlocks.Add(new ScheduleBlock() { weekDay = WeekDay.MONDAY, startTime = 8 }); // supposed to match
            testsTutors[0].scheduleBlocks.Add(new ScheduleBlock() { weekDay = WeekDay.MONDAY, startTime = 9 });
            testsTutors[0].tutorAvailableForIndividualSession = true;
            testsTutors[0].coursesSkills.Add(randomCourse);

            testsTutors[1].scheduleBlocks.Add(new ScheduleBlock() { weekDay = WeekDay.MONDAY, startTime = 8 });
            testsTutors[1].scheduleBlocks.Add(new ScheduleBlock() { weekDay = WeekDay.MONDAY, startTime = 9 });
            testsTutors[1].tutorAvailableForIndividualSession = true;
            testsTutors[1].coursesSkills.Add(otherRandomCourse); // not supposed to match since the tutor isn't skilled in the appropriate course

            testsTutors[2].scheduleBlocks.Add(new ScheduleBlock() { weekDay = WeekDay.MONDAY, startTime = 9 }); // not supposed to match since the tutor is available on 9-11 am
            testsTutors[2].scheduleBlocks.Add(new ScheduleBlock() { weekDay = WeekDay.MONDAY, startTime = 10 });
            testsTutors[2].tutorAvailableForIndividualSession = true;
            testsTutors[2].coursesSkills.Add(randomCourse);

            testsTutors[3].scheduleBlocks.Add(new ScheduleBlock() { weekDay = WeekDay.FRIDAY, startTime = 8 }); // not supposed to match since the tutor is only available on fridays
            testsTutors[3].scheduleBlocks.Add(new ScheduleBlock() { weekDay = WeekDay.FRIDAY, startTime = 9 });
            testsTutors[3].tutorAvailableForIndividualSession = true;
            testsTutors[3].coursesSkills.Add(randomCourse);

            testsTutors[4].scheduleBlocks.Add(new ScheduleBlock() { weekDay = WeekDay.MONDAY, startTime = 8 }); // not supposed to match since the tutor isn't available for individual sessions tutoring
            testsTutors[4].scheduleBlocks.Add(new ScheduleBlock() { weekDay = WeekDay.MONDAY, startTime = 9 });
            testsTutors[4].tutorAvailableForIndividualSession = false;
            testsTutors[4].coursesSkills.Add(randomCourse);

            this.appContext.getRepository().getAll<Tutor>().ReturnsForAnyArgs(testsTutors.AsQueryable());
            this.appContext.getRepository().getById<HelpRequest>(343).ReturnsForAnyArgs(randomHelpRequest);

            var validViewModel = new ManagerHelpRequestTutorAssignmentViewModels.GetTutorAvailableForGivenBlockViewModel()
            {
                dayOfWeek = WeekDay.MONDAY,
                startHour = 8,
                endHour = 10,
                helpRequestId = -1
            };

            // Act
            var getAvailableTutorsResult = this.controller.ajax_GetTutorsAvailableAt(validViewModel) as JsonResult;

            // Assert
            Assert.IsNotNull(getAvailableTutorsResult);
            var returnedJsonData = getAvailableTutorsResult.Data as ManagerHelpRequestTutorAssignmentViewModels.TutorAvailableForGivenBlockViewModel[];
            Assert.IsNotNull(returnedJsonData);
            Assert.AreEqual(1, returnedJsonData.Count());
            Assert.AreEqual(testsTutors[0].id, returnedJsonData[0].tutorUserId);
        }

        /* =======================================
         *  Get StudentReport (ajax method) unit tests
         * ====================================== */

        private void arrangeReportMessageReturnForIndividualSessionId(int id, string message, HelpRequestMgrController.StudentType studentType)
        {
            var helpRequest = this.fixture.Create<HelpRequest>();
            helpRequest.id = id;
            // make sure the session is finished
            helpRequest.helpedHasConfirmed = true;
            helpRequest.tutorHasConfirmed = true;
            helpRequest.individualSession = this.fixture.Create<IndividualSession>();
            helpRequest.individualSession.date = DateTime.Now.Subtract(TimeSpan.FromDays(10));

            if (studentType == HelpRequestMgrController.StudentType.Tutor)
            {
                helpRequest.individualSession.tutorMessage = message;
            }
            else if (studentType == HelpRequestMgrController.StudentType.Helped)
            {
                helpRequest.individualSession.helpedMessage = message;
            }


            this.appContext.getRepository().getById<HelpRequest>(343).ReturnsForAnyArgs(helpRequest);
        }

        [TestMethod]
        public void get_studentReport_for_tutor_returns_the_report()
        {
            // Arrange
            this.arrangeReportMessageReturnForIndividualSessionId(individualSessionId, reportMessage, HelpRequestMgrController.StudentType.Tutor);

            // Act
            var jsonReport = this.controller.ajax_studentReport(individualSessionId, HelpRequestMgrController.StudentType.Tutor) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonReport);
            Assert.AreEqual(reportMessage, jsonReport.Data);
        }

        [TestMethod]
        public void get_studentReport_for_helped_returns_the_report()
        {
            // Arrange
            this.arrangeReportMessageReturnForIndividualSessionId(individualSessionId, reportMessage, HelpRequestMgrController.StudentType.Helped);

            // Act
            var jsonReport = this.controller.ajax_studentReport(individualSessionId, HelpRequestMgrController.StudentType.Helped) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonReport);
            Assert.AreEqual(reportMessage, jsonReport.Data);
        }

        [TestMethod]
        public void get_studentReport_for_inexesting_studentType_returns_empty_report()
        {
            // Arrange
            const int randomStudentType = 3;
            this.arrangeReportMessageReturnForIndividualSessionId(individualSessionId, reportMessage, (HelpRequestMgrController.StudentType) randomStudentType);

            // Act
            var jsonReport = this.controller.ajax_studentReport(individualSessionId, (HelpRequestMgrController.StudentType)randomStudentType) as JsonResult;

            // Assert
            Assert.IsNotNull(jsonReport);
            Assert.AreEqual("", jsonReport.Data);
        }

        
    }
}