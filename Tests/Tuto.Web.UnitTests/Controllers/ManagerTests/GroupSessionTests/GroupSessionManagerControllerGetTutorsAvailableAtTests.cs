using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers;
using Tuto.Web.ViewModels.TutorsMgr;

namespace Tuto.Web.UnitTests.Controllers.ManagerTests.GroupSessionTests
{
    [TestClass]
    public class GroupSessionManagerControllerGetTutorsAvailableAtTests : GroupSessionManagerControllerBaseTests
    {
        [TestMethod]
        public void get_available_tutors_action_should_kick_user_if_not_manager()
        {
            // Arrange
            var tutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, tutor);

            var viewModel = this.fixture.Create<TutorAvailableAtViewModel>();

            // Act
            var actionResult = this.controller.ajax_GetTutorsAvailableAt(viewModel);

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void get_available_tutors_actions_should_return_json_result()
        {
            // Arrange
            var manager = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            var viewModel = this.fixture.Create<TutorAvailableAtViewModel>();

            // Act
            var actionResult = this.controller.ajax_GetTutorsAvailableAt(viewModel) as JsonResult;

            // Assert
            Assert.IsNotNull(actionResult);
        }
    }
}
