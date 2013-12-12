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
    public class AccountControllerRegisterTutorTests : AccountControllerBaseTests
    {
        [TestMethod]
        public void register_tutor_action_should_return_register_tutor_view()
        {
            // Act
            var actionResult = this.controller.registerTutor();
     
            // Assert
            actionResult.AssertViewRendered()
              .ForView("RegisterTutor");
        }
        
        [TestMethod]
        public void post_create_should_add_tutor_to_repository()
        {
            // Arrange   
            var tutorRegisterViewModel = this.fixture.Create<TutorRegisterViewModel>();
            tutorRegisterViewModel.coursesSkillsJson = "[{\"department\":\"Général\",\"course\":\"Français 1\"},{\"department\":\"Général\",\"course\":\"Anglais 1\"}]";
            
            // Action
            this.controller.registerTutor(tutorRegisterViewModel);

            // Assert
            this.appContext.getRepository().Received().add(Arg.Is<User>(x => (x.password == tutorRegisterViewModel.password && x.mail == tutorRegisterViewModel.mail)));
         }
        
        [TestMethod]
        public void post_create_should_return_view_with_errors_when_modelState_is_not_valid()
        {
            //Arrange
            var tutorRegisterViewModel = this.fixture.Create<TutorRegisterViewModel>();
            tutorRegisterViewModel.coursesSkillsJson = "[{\"department\":\"Général\",\"course\":\"Français 1\"},{\"department\":\"Général\",\"course\":\"Anglais 1\"}]";
            this.controller.ModelState.AddModelError("Error", "Error");

            //Act
            var actionResult = this.controller.registerTutor(tutorRegisterViewModel);

            //Assert
            actionResult.AssertActionRedirect().ToAction("registerTutor");
        }

        [TestMethod]
        public void post_create_should_redirect_to_index_on_success()
        {
            //Arrange
            var tutorRegisterViewModel = this.fixture.Create<TutorRegisterViewModel>();
            tutorRegisterViewModel.coursesSkillsJson = "[{\"department\":\"Général\",\"course\":\"Français 1\"}]";

            //Act
            var actionResult = this.controller.registerTutor(tutorRegisterViewModel);

            //Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.login());
        }

    }
}