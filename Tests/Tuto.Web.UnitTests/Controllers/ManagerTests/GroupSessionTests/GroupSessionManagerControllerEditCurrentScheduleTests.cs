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
    public class GroupSessionManagerControllerEditCurrentScheduleTests : GroupSessionManagerControllerBaseTests
    {
        [TestMethod]
        public void edit_current_schedule_action_should_kick_user_if_not_manager()
        {
            // Arrange
            var tutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, tutor);

            // Act
            var actionResult = this.controller.editCurrentSchedule();

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void edit_current_schedule_action_should_kick_user_if_manager_does_not_have_default_schedule()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            manager.hasDefaultSchedule = false;

            // Act
            var actionResult = this.controller.editCurrentSchedule();

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void edit_current_schedule_action_should_return_edit_current_schedule_view_if_manager()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            manager.hasDefaultSchedule = true;

            // Act
            var actionResult = this.controller.editCurrentSchedule();

            // Assert
            Assert.IsNotNull(actionResult);
            actionResult.AssertViewRendered().ForView("EditCurrentSchedule");
        }

        [TestMethod]
        public void post_edit_current_schedule_action_should_kick_user_if_not_manager()
        {
            // Arrange
            var tutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, tutor);

            var viewModel = this.fixture.Create<GroupSessionScheduleViewModel>();

            // Act
            var actionResult = this.controller.editCurrentSchedule(viewModel);

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void post_edit_current_schedule_action_should_delete_old_assigned_sessions_from_manager()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            var groupSessionWeekSchedule = this.fixture.Create<GroupSessionWeekSchedule>();
            manager.groupSessionWeekSchedules.Add(groupSessionWeekSchedule);

            var assignedGroupSession = this.fixture.Create<AssignedGroupSession>();
            groupSessionWeekSchedule.groupSessions.Add(assignedGroupSession);

            var viewModel = new GroupSessionScheduleViewModel {jsonGroupSessions = "[]"};

            // Act
            this.controller.editCurrentSchedule(viewModel);

            // Assert
            this.appContext.getRepository().Received().delete<AssignedGroupSession>(Arg.Is<int>(x => x == assignedGroupSession.id));
        }

        [TestMethod]
        public void post_edit_default_schedule_action_should_update_current_week_schedule()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            var groupSessionWeekSchedule = this.fixture.Create<GroupSessionWeekSchedule>();
            manager.groupSessionWeekSchedules.Add(groupSessionWeekSchedule);

            var assignedGroupSession = this.fixture.Create<AssignedGroupSession>();
            groupSessionWeekSchedule.groupSessions.Add(assignedGroupSession);

            var viewModel = new GroupSessionScheduleViewModel { jsonGroupSessions = "[]" };

            // Act
            this.controller.editCurrentSchedule(viewModel);

            // Assert
            this.appContext.getRepository().Received().update(groupSessionWeekSchedule);
        }
    }
}
