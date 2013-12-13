using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers;
using Tuto.Web.ViewModels.GroupSession;

namespace Tuto.Web.UnitTests.Controllers.ManagerTests.GroupSessionTests
{
    [TestClass]
    public class GroupSessionManagerControllerEditDefaultScheduleTests : GroupSessionManagerControllerBaseTests
    {
        [TestMethod]
        public void edit_default_schedule_action_should_kick_user_if_not_manager()
        {
            // Arrange
            var tutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, tutor);

            // Act
            var actionResult = this.controller.editDefaultSchedule();

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void edit_default_schedule_action_should_return_edit_default_schedule_view_if_manager()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            // Act
            var actionResult = this.controller.editDefaultSchedule();

            // Assert
            Assert.IsNotNull(actionResult);
            actionResult.AssertViewRendered().ForView("EditDefaultSchedule");
        }

        [TestMethod]
        public void post_edit_default_schedule_action_should_kick_user_if_not_manager()
        {
            // Arrange
            var tutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, tutor);

            var viewModel = this.fixture.Create<GroupSessionScheduleViewModel>();

            // Act
            var actionResult = this.controller.editDefaultSchedule(viewModel);

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void post_edit_default_schedule_action_should_delete_default_sessions_from_manager()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            var defaultGroupSession = this.fixture.Create<DefaultGroupSession>();
            manager.defaultGroupSessions.Add(defaultGroupSession);

            var viewModel = new GroupSessionScheduleViewModel {jsonGroupSessions = "[]"};

            // Act
            this.controller.editDefaultSchedule(viewModel);

            // Assert
            this.appContext.getRepository().Received().delete<DefaultGroupSession>(Arg.Is<int>(x => x == defaultGroupSession.id));
        }

        [TestMethod]
        public void post_edit_default_schedule_action_should_update_manager()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            var viewModel = new GroupSessionScheduleViewModel { jsonGroupSessions = "[]" };

            // Act
            this.controller.editDefaultSchedule(viewModel);

            // Assert
            this.appContext.getRepository().Received().update(manager);
        }

        [TestMethod]
        public void post_edit_default_schedule_action_should_update_manager_to_have_default_schedule()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            var viewModel = new GroupSessionScheduleViewModel { jsonGroupSessions = "[]" };

            // Act
            this.controller.editDefaultSchedule(viewModel);

            // Assert
            Assert.IsTrue(manager.hasDefaultSchedule);
        }
    }
}
