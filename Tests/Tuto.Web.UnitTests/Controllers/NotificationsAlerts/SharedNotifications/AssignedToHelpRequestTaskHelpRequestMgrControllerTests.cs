using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Notifications.Shared;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers.Manager;
using Tuto.Web.ViewModels;

namespace Tuto.Web.UnitTests.Controllers.NotificationsAlerts.SharedNotifications
{
    [TestClass]
    public class AssignedToHelpRequestTaskHelpRequestMgrControllerTests : ConcreteControllersNotificationsBaseTests
    {
        private HelpRequestMgrController controller;

        [TestInitialize]
        public void assignedToHelpRequestTaskHelpRequestMgrControllerTestsInit()
        {
            TestsUtilities.bypassAppAuthentification(this.appContext, this.appContext.getConfiguration().mainManager);

            this.controller = new HelpRequestMgrController(this.appContext);
        }

        [TestMethod]
        public void an_assigned_to_help_request_task_should_be_created_upon_successful_tutor_to_helprequest_assignment()
        {
            // Arange
            var fakedHelpRequest = this.fixture.Create<HelpRequest>();
            var fakedTutor = this.fixture.Create<Tutor>();
            var fakedAssignementViewModel = this.fixture.Create<ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel>();
            var expectedReturnedTask =
                new AssignedToHelpRequestTask().getBuilder().setConcernedHelpRequest(fakedHelpRequest).getNotification();

            fakedAssignementViewModel.weekPickStart = "2050/05/05";

            this.appContext.getRepository().getById<HelpRequest>(-1).ReturnsForAnyArgs(fakedHelpRequest);
            this.appContext.getRepository().getById<Tutor>(-1).ReturnsForAnyArgs(fakedTutor);

            // Act
            var returnedViewModel = this.controller.assign(fakedAssignementViewModel);

            // Assert
            Assert.IsNotNull(returnedViewModel);
            this.appContext.getRepository().Received().add(
                Arg.Is<AssignedToHelpRequestTask>(x => x.concernedHelpRequest == expectedReturnedTask.concernedHelpRequest)
            );
        }

        [TestMethod]
        public void an_assigned_to_help_request_task_should_not_be_created_upon_failed_tutor_to_helprequest_assignment()
        {
            // Arrange
            var fakedAssignementViewModel = this.fixture.Create<ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel>();
            var assignmentFailed = false;
            this.controller.ModelState.AddModelError("UnitTest", "Consciously invalidating the controller view model");
            
            // Act
            try
            {
                this.controller.assign(fakedAssignementViewModel);
            }
            catch (Exception)
            {
                assignmentFailed = true;
            }
            
            // Assert
            Assert.IsTrue(assignmentFailed);
            this.appContext.getRepository().DidNotReceive().add(Arg.Any<AssignedToHelpRequestTask>());
        }
    }
}
