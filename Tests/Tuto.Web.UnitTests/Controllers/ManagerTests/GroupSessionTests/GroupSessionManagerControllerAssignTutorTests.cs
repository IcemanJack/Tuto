using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Notifications.Tutor;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers;

namespace Tuto.Web.UnitTests.Controllers.ManagerTests.GroupSessionTests
{
    [TestClass]
    public class GroupSessionManagerControllerAssignTutorTests : GroupSessionManagerControllerBaseTests
    {
        [TestMethod]
        public void assign_tutor_action_should_kick_user_if_not_manager()
        {
            // Arrange
            var tutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, tutor);

            // Act
            var actionResult = this.controller.ajax_AssignTutor(0, 0);

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void assign_tutor_action_should_return_error_if_invalid_session_or_tutor()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            var session = this.fixture.Create<AssignedGroupSession>();
            var tutor = this.fixture.Create<Tutor>();

            // Act
            var actionResult = this.controller.ajax_AssignTutor(session.id, tutor.id) as ContentResult;

            // Assert
            Assert.IsTrue(actionResult.Content.Equals("Error"));
        }

        [TestMethod]
        public void assign_tutor_action_should_assign_tutor_to_session()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            var session = this.fixture.Create<AssignedGroupSession>();
            var tutor = this.fixture.Create<Tutor>();

            this.appContext.getRepository().getById<AssignedGroupSession>(session.id).Returns(session);
            this.appContext.getRepository().getById<Tutor>(tutor.id).Returns(tutor);

            // Act
            this.controller.ajax_AssignTutor(session.id, tutor.id);

            // Assert
            this.appContext.getRepository().Received().update(session);
            Assert.IsTrue(session.tutor == tutor);
        }

        [TestMethod]
        public void assign_tutor_action_should_return_success_on_success()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            var session = this.fixture.Create<AssignedGroupSession>();
            var tutor = this.fixture.Create<Tutor>();

            this.appContext.getRepository().getById<AssignedGroupSession>(session.id).Returns(session);
            this.appContext.getRepository().getById<Tutor>(tutor.id).Returns(tutor);

            // Act
            var actionResult = this.controller.ajax_AssignTutor(session.id, tutor.id) as ContentResult;

            // Assert
            Assert.IsTrue(actionResult.Content.Equals("Success"));
        }

        [TestMethod]
        public void assign_tutor_action_should_add_notification()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            var session = this.fixture.Create<AssignedGroupSession>();
            var tutor = this.fixture.Create<Tutor>();

            this.appContext.getRepository().getById<AssignedGroupSession>(session.id).Returns(session);
            this.appContext.getRepository().getById<Tutor>(tutor.id).Returns(tutor);

            // Act
            this.controller.ajax_AssignTutor(session.id, tutor.id);

            // Assert
            this.appContext.getRepository().ReceivedWithAnyArgs().add<AssignedToGroupSessionAlert>(null);
        }

    }
}
