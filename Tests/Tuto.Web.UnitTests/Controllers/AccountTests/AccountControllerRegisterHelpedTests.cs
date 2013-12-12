using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers;
using Tuto.Web.ViewModels.Account.Register;

namespace Tuto.Web.UnitTests.Controllers.AccountTests
{
    [TestClass]
    public class AccountControllerRegisterHelpedTests : AccountControllerBaseTests
    {
        [TestMethod]
        public void register_tutor_action_should_return_register_helped_view()
        {
            // Act
            var actionResult = this.controller.registerHelped();

            // Assert
            actionResult.AssertViewRendered()
                .ForView("RegisterHelped");
        }

        [TestMethod]
        public void post_create_should_add_helped_to_repository()
        {
            // Arrange   
            var helpedRegisterViewModel = this.fixture.Create<HelpedRegisterViewModel>();

            // Action
            this.controller.registerHelped(helpedRegisterViewModel);

            // Assert
            this.appContext.getRepository()
                .Received()
                .add(
                    Arg.Is<User>(
                        x => (x.password == helpedRegisterViewModel.password && x.mail == helpedRegisterViewModel.mail)));
        }

        [TestMethod]
        public void post_create_should_return_view_with_errors_when_modelState_is_not_valid()
        {
            //Arrange
            var helpedRegisterViewModel = this.fixture.Create<HelpedRegisterViewModel>();
            this.controller.ModelState.AddModelError("Error", "Error");

            //Act
            var actionResult = this.controller.registerHelped(helpedRegisterViewModel);

            //Assert
            actionResult.AssertViewRendered()
                .ForView("RegisterHelped");
        }

        [TestMethod]
        public void post_create_should_redirect_to_index_on_success()
        {
            //Arrange
            var helpedRegisterViewModel = this.fixture.Create<HelpedRegisterViewModel>();

            //Act
            var actionResult = this.controller.registerHelped(helpedRegisterViewModel);

            //Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.login());
        }
}
}