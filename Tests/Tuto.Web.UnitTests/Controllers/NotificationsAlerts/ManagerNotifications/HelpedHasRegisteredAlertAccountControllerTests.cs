using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Notifications.Manager;
using Tuto.Web.Controllers;
using Tuto.Web.ViewModels.Account.Register;

namespace Tuto.Web.UnitTests.Controllers.NotificationsAlerts.ManagerNotifications
{
    [TestClass]
    public class HelpedHasRegisteredAlertAccountControllerTests : ConcreteControllersNotificationsBaseTests
    {
        private AccountController controller;

        [TestInitialize]
        public void helpedHasRegisteredAlertAccountControllerTestsInit()
        {
            this.controller = new AccountController(this.appContext);
        }

        [TestMethod]
        public void an_helped_has_registered_alert_should_be_created_upon_successful_helped_registeration()
        {
            // Arrange
            var fakedHelpedCreationViewModel = this.fixture.Create<HelpedRegisterViewModel>();
            
            fakedHelpedCreationViewModel.scheduleBlocksJson = "[{\"start\":\"08:00 am\",\"end\":\"11:00 am\",\"day\":1}]";

            // Act
            var returnedViewModel = this.controller.registerHelped(fakedHelpedCreationViewModel);

            // Assert
            Assert.IsNotNull(returnedViewModel);
            this.appContext.getRepository().Received().add(Arg.Is<HelpedHasRegisteredAlert>(x => x.helpedUser.mail == fakedHelpedCreationViewModel.mail));
        }

        [TestMethod]
        public void an_helped_has_registered_alert_should_not_be_created_upon_helpred_registeration_failure()
        {
            // Arrange
            var fakedHelpedCreationViewModel = this.fixture.Create<HelpedRegisterViewModel>();
            this.controller.ModelState.AddModelError("UnitTest", "Invalidating the modelstate");

            // Act
            var returnedViewModel = this.controller.registerHelped(fakedHelpedCreationViewModel);

            // Assert
            Assert.IsNotNull(returnedViewModel);
            this.appContext.getRepository().DidNotReceive().add(Arg.Any<HelpedHasRegisteredAlert>());
        }

    }
}
