using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers;
using Tuto.Web.ViewModels.GroupSession;

namespace Tuto.Web.UnitTests.Controllers.ManagerTests.GroupSessionTests
{
    [TestClass]
    public class GroupSessionManagerControllerListTests : GroupSessionManagerControllerBaseTests
    {
        [TestMethod]
        public void list_action_should_kick_user_if_not_manager()
        {
            // Arrange
            var tutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, tutor);

            // Act
            var actionResult = this.controller.list();

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void list_action_should_return_list_view_if_manager()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            // Act
            var actionResult = this.controller.list();

            // Assert
            Assert.IsNotNull(actionResult);
            actionResult.AssertViewRendered().ForView("GroupSessionList");
        }

        [TestMethod]
        public void list_action_should_return_list_view_with_empty_view_model_if_manager_has_no_default_schedule()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            manager.hasDefaultSchedule = false;

            // Act
            var actionResult = this.controller.list() as ViewResult;

            // Assert
            Assert.IsNotNull(actionResult);
            actionResult.AssertViewRendered().ForView("GroupSessionList");
            Assert.IsTrue(actionResult.ViewData.Model is ICollection<GroupSessionListViewModel.ManagerViewModel>);
            Assert.IsTrue((actionResult.ViewData.Model as ICollection<GroupSessionListViewModel.ManagerViewModel>).Count == 0);
        }

        [TestMethod]
        public void list_action_should_update_manager_if_manager_has_no_current_week_schedule()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            manager.hasDefaultSchedule = true;

            // Act
            var actionResult = this.controller.list() as ViewResult;

            // Assert
            this.appContext.getRepository().Received().update(manager);
        }

    }
}
