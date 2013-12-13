using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Notifications.Manager;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers;
using Tuto.Web.ViewModels.HelpRequest;

namespace Tuto.Web.UnitTests.Controllers.NotificationsAlerts.ManagerNotifications
{
    [TestClass]
    public class HelpRequestToAssignAlertHelpRequestControllerTests : ConcreteControllersNotificationsBaseTests
    {
        protected HelpRequestController controller;

        [TestInitialize]
        public void helpRequestToAssignAlertHelpRequestControllerTestsInit()
        {
            var dummyHelped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, dummyHelped);

            this.controller = new HelpRequestController(this.appContext);
        }

        [TestMethod]
        public void a_help_request_to_assign_alert_should_be_created_upon_sucessfull_helprequest_creation()
        {
            // Arrange
            var fakedHelpRequestCreationViewModel = this.fixture.Create<HelpRequestAddViewModel>();

            fakedHelpRequestCreationViewModel.courseId = 2;
            fakedHelpRequestCreationViewModel.scheduleJson = "[{\"start\":\"08:00 am\",\"end\":\"11:00 am\",\"day\":1}]";
            
            // Act
            var returnedViewModel = this.controller.create(fakedHelpRequestCreationViewModel);

            // Assert
            Assert.IsNotNull(returnedViewModel);
            this.appContext.getRepository().Received().add(
                Arg.Is<HelpRequestToAssignAlert>(x => x.helpRequest.comment == fakedHelpRequestCreationViewModel.comment)
            );
        }

        [TestMethod]
        public void a_help_request_to_assign_alert_should_not_be_created_upon_helprequest_creation_failure()
        {
            // Arrange
            this.controller.ModelState.AddModelError("UnitTest", "Invalidating the viewmodel");
            var fakedCreationViewModel = this.fixture.Create<HelpRequestAddViewModel>();

            // Act
            var returnedViewModel = this.controller.create(fakedCreationViewModel);

            // Assert
            Assert.IsNotNull(returnedViewModel);
            this.appContext.getRepository().DidNotReceive().add(Arg.Any<HelpRequestToAssignAlert>());
        }
    }
}
