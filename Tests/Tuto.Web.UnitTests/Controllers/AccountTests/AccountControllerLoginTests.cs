using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Tuto.DataLayer.Models.Users;
using Ploeh.AutoFixture;
using Tuto.Web.ViewModels.Account;

namespace Tuto.Web.UnitTests.Controllers.AccountTests
{
    [TestClass]
    public class AccountControllerLoginTests : AccountControllerBaseTests
    {
        [TestMethod]
        public void unauthorized_action_should_return_login_view()
        {
            // Act
            var returnedView = this.controller.unauthorized();

            // Assert
            Assert.IsNotNull(returnedView);
            returnedView.AssertViewRendered().ForView("Login");
        }

        [TestMethod]
        public void login_action_should_return_login_view()
        {
            // Act
            var returnedView = this.controller.login();

            // Assert
            Assert.IsNotNull(returnedView);
            returnedView.AssertViewRendered().ForView("Login");
        }

        [TestMethod]
        public void index_action_should_return_login_view()
        {
            // Act
            var result = controller.index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            result.AssertViewRendered().ForView("Login");
        }

        [TestMethod]
        public void register_tutor_action_should_return_register_tutor_view()
        {
            // Act
            var returnedResult = this.controller.registerTutor();

            // Assert
            Assert.IsNotNull(returnedResult);
            returnedResult.AssertViewRendered().ForView("RegisterTutor");
        }
        
        [TestMethod]
        public void post_connect_should_return_login_view_with_errors_when_bad_mail_or_bad_password()
        {
            // Arrange
            var user = fixture.Create<Helped>();
            var invalidViewModel = Mapper.Map<UserLoginViewModel>(user);

            // Act
            var viewResult = controller.login(invalidViewModel);

            // Assert
            this.appContext.getRepository().ReceivedWithAnyArgs().single<Helped>(null);

            Assert.IsNotNull(viewResult);
            viewResult.AssertActionRedirect();

        }
        
        [TestMethod]
        public void post_connect_should_return_home_view_when_good_mail_and_good_password()
        {
            // Arrange
            var validViewModel = Mapper.Map<UserLoginViewModel>(loggedInUser);

            this.appContext.getRepository().single<User>(m => m.id == loggedInUser.id).ReturnsForAnyArgs(loggedInUser);

            // Act
            ActionResult viewResult = this.controller.login(validViewModel);
            
            // Assert
            viewResult.AssertActionRedirect().ToAction("index");
        }

        [TestMethod]
        public void post_login_should_return_login_view_when_invalid_modelstate()
        {
            // Arrange
            this.controller.ModelState.AddModelError("UnitTests", "");

            // Act
            var viewResult = this.controller.login();

            // Assert
            Assert.IsNotNull(viewResult);
            viewResult.AssertViewRendered().ForView("Login");
        }

        [TestMethod]
        public void disconnect_action_should_render_login_view()
        {
            // Arrange
            var fakedLoggedInHelped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedLoggedInHelped);

            // Act
            var returnedAction = this.controller.disconnect();

            // Assert
            Assert.IsNotNull(returnedAction);
            returnedAction.AssertActionRedirect().ToAction("login");
        }

    }
}