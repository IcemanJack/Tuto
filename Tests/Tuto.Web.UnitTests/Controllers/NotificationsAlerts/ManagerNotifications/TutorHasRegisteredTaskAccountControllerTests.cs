using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Notifications.Manager;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers;
using Tuto.Web.ViewModels.Account.Register;

namespace Tuto.Web.UnitTests.Controllers.NotificationsAlerts.ManagerNotifications
{
    [TestClass]
    public class TutorHasRegisteredTaskAccountControllerTests : ConcreteControllersNotificationsBaseTests
    {
        private AccountController controller;

        [TestInitialize]
        public void tutorHasRegisteredTaskAccountControllerTestsInit()
        {
            // no authentification to bypass
            this.controller = new AccountController(this.appContext);
        }

        [TestMethod]
        public void tutor_has_registered_task_should_be_created_upon_tutor_successful_registeration()
        {
            // Arrange
            var fakedRegisterViewModel = this.fixture.Create<TutorRegisterViewModel>();
            var createdTutor = this.fixture.Create<Tutor>();
            var expectedCreatedNotification =
                new TutorHasRegisteredTask().getBuilder().setNewlyRegistredTutor(createdTutor).getNotification();

            createdTutor.mail = fakedRegisterViewModel.mail;
            fakedRegisterViewModel.scheduleBlocksJson = "[{\"start\":\"08:00 am\",\"end\":\"11:00 am\",\"day\":1}]";
            fakedRegisterViewModel.coursesSkillsJson = "[{\"department\":\"Général\",\"course\":\"Français 1\"},{\"department\":\"Général\",\"course\":\"Anglais 1\"}]";

            // Act
            var returnedViewModel = this.controller.registerTutor(fakedRegisterViewModel);

            // Assert
            Assert.IsNotNull(returnedViewModel);
            this.appContext.getRepository().Received().add(Arg.Is<TutorHasRegisteredTask>(x => x.registeredTutor.mail == createdTutor.mail));
        }

        [TestMethod]
        public void tutor_has_registered_task_should_not_be_created_upon_tutor_registeration_failure()
        {
            // Arrange
            var fakedViewModel = this.fixture.Create<TutorRegisterViewModel>();
            this.controller.ModelState.AddModelError("UnitTest", "Invalidating the modelstate");

            // Act
            var returnedViewModel = this.controller.registerTutor(fakedViewModel);

            // Assert
            Assert.IsNotNull(returnedViewModel);
            this.appContext.getRepository().DidNotReceive().add(Arg.Any<TutorHasRegisteredTask>());
        }
    }
}
