using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Notifications.Tutor;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers.Manager;

namespace Tuto.Web.UnitTests.Controllers.NotificationsAlerts.TutorNotifications
{
    [TestClass]
    public class AssignedToGroupSessionAlertGroupSessionMgrControllerTests : ConcreteControllersNotificationsBaseTests
    {
        private GroupSessionMgrController controller;

        [TestInitialize]
        public void initController()
        {
            // fake manager authentification
            TestsUtilities.bypassAppAuthentification(this.appContext, this.appContext.getConfiguration().mainManager);

            this.controller = new GroupSessionMgrController(this.appContext);
        }

        [TestMethod]
        public void assigning_a_tutor_to_a_group_session_should_create_an_assigned_to_group_session_tutor_alert()
        {
            // Arrange
            var fakedGroupSession = this.fixture.Create<AssignedGroupSession>();
            var fakedTutor = this.fixture.Create<Tutor>();
            var expectedReturnedAlert = new AssignedToGroupSessionAlert().getBuilder().setConcernedGroupSession(fakedGroupSession).getNotification();

            this.appContext.getRepository().getById<AssignedGroupSession>(-1).ReturnsForAnyArgs(fakedGroupSession);
            this.appContext.getRepository().getById<Tutor>(-1).ReturnsForAnyArgs(fakedTutor);

            // Act
            var returnedViewModel = this.controller.ajax_AssignTutor(-1, -1);

            // Assert
            Assert.IsNotNull(returnedViewModel);
            this.appContext.getRepository().Received().add(
                Arg.Is<AssignedToGroupSessionAlert>(x => x.concernedGroupSession == expectedReturnedAlert.concernedGroupSession)
            );
        }

        [TestMethod]
        public void error_on_assigning_a_tutor_to_a_group_session_should_not_create_an_assigned_to_group_session_tutor_alert()
        {
            // Arrange
            const int CERTAINLY_INVALID_GROUPSESSION_ID = -1;
            const int CERTAINLY_INVALID_TUTOR_ID = -1;

            // Act
            var returnedViewModel = this.controller.ajax_AssignTutor(CERTAINLY_INVALID_GROUPSESSION_ID, CERTAINLY_INVALID_TUTOR_ID) as ContentResult;

            // Assert
            Assert.IsNotNull(returnedViewModel);
            this.appContext.getRepository().DidNotReceive().add(Arg.Any<AssignedToGroupSessionAlert>());
        }
    }
}
