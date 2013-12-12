using System.Web.Mvc;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.UnitTests.Generic;
using Tuto.Web.ViewModels;
using Ploeh.AutoFixture;
using Tuto.Web.ViewModels.Account;

namespace Tuto.Web.UnitTests.Controllers.AccountTests
{
    [TestClass]
    public class AccountControllerLoginTests : AccountControllerBaseTests
    {
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
            AssertFunctions.assertValidRenderedViewForName(controller.registerTutor(), "RegisterTutor");
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
            viewResult.AssertActionRedirect();
        }
    }
}