using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using MvcContrib.TestHelper;
using NSubstitute;
using Tuto.DataLayer.Models.Users;
using Ploeh.AutoFixture;
using Tuto.DataLayer.ModelUtilities;
using Tuto.Web.ViewModels.Account.Edit;
using Arg = NSubstitute.Arg;

namespace Tuto.Web.UnitTests.Controllers.AccountTests
{
    [TestClass]
    public class AccountControllerEditTests : AccountControllerBaseTests
    {
        [TestMethod]
        public void should_return_login_view_if_user_is_not_loged_in_view()
        {
            // Arrange
            appContext.getHttpContext().Session["loggedIn"] = null;

            // Act
            var viewResult = controller.edit();

            // Asserts
            Assert.IsNotNull(viewResult);
            viewResult.AssertActionRedirect();
        }

        [TestMethod]
        public void edit_action_with_logged_in_user_should_return_edit_view()
        {
            // Act
            var returnedResult = controller.edit();

            // Assert
            Assert.IsNotNull(returnedResult);
            returnedResult.AssertViewRendered().ForView("EditHelped");
        }

        [TestMethod]
        public void post_save_changes_should_return_view_with_errors_when_modelState_is_not_valid()
        {
            // Arrange
            var editHelpedViewModel = fixture.Create<EditHelpedViewModel>();
            controller.ModelState.AddModelError("Error", "Error");

            // Act
            var returnedResult = controller.editHelped(editHelpedViewModel);

            // Assert
            Assert.IsNotNull(returnedResult);
            returnedResult.AssertViewRendered().ForView("EditHelped");
        }

        [TestMethod]
        public void post_schedule_in_good_json_format_should_return_edit_view()
        {
            // Arrange
            var editHelpedViewModel = Mapper.Map<EditHelpedViewModel>(loggedInUser);
            editHelpedViewModel.jsonSchedule = ScheduleUtilities.getJsonFromScheduleBlocks(loggedInUser.scheduleBlocks);

            editHelpedViewModel.currentPassword = loggedInUser.password;
            editHelpedViewModel.currentEmail = loggedInUser.mail;

            // Act
            var viewResult = controller.editHelped(editHelpedViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Helped>(x => ScheduleUtilities.getJsonFromScheduleBlocks(x.scheduleBlocks) == editHelpedViewModel.jsonSchedule));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditHelped");
        }

        /*************************************
         * Helped
         *************************************/

        [TestMethod]
        public void helped_post_save_with_new_password_and_same_confirmed_password_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var editHelpedViewModel = Mapper.Map<EditHelpedViewModel>(loggedInUser);

            appContext.getRepository().getById<User>(loggedInUser.id).ReturnsForAnyArgs(loggedInUser);

            editHelpedViewModel.newPassword = "password2";
            editHelpedViewModel.confirmNewPassword = "password2";

            editHelpedViewModel.currentPassword = loggedInUser.password;
            editHelpedViewModel.currentEmail = loggedInUser.mail;

            // Act
            var viewResult = controller.editHelped(editHelpedViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Helped>(x => x.password == editHelpedViewModel.newPassword));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditHelped");
        }

        [TestMethod]
        public void helped_post_save_with_new_email_and_same_confirmed_email_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var editHelpedViewModel = Mapper.Map<EditHelpedViewModel>(loggedInUser);

            appContext.getRepository().getById<User>(loggedInUser.id).ReturnsForAnyArgs(loggedInUser);

            editHelpedViewModel.newEmail = "thenewemail@mail.com";
            editHelpedViewModel.confirmNewEmail = "thenewemail@mail.com";

            editHelpedViewModel.currentPassword = loggedInUser.password;
            editHelpedViewModel.currentEmail = loggedInUser.mail;

            // Act
            var viewResult = controller.editHelped(editHelpedViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Helped>(x => x.mail == editHelpedViewModel.newEmail));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditHelped");
        }

        [TestMethod]
        public void helped_post_save_with_new_name_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var editHelpedViewModel = Mapper.Map<EditHelpedViewModel>(loggedInUser);

            appContext.getRepository().getById<Helped>(loggedInUser.id).ReturnsForAnyArgs(loggedInUser);

            editHelpedViewModel.newName = "Foo";

            editHelpedViewModel.currentPassword = loggedInUser.password;
            editHelpedViewModel.currentEmail = loggedInUser.mail;

            // Act
            var viewResult = controller.editHelped(editHelpedViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Helped>(x => x.name == editHelpedViewModel.newName));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditHelped");
        }

        [TestMethod]
        public void helped_post_save_with_new_lastname_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var editHelpedViewModel = Mapper.Map<EditHelpedViewModel>(loggedInUser);

            appContext.getRepository().getById<Helped>(loggedInUser.id).ReturnsForAnyArgs(loggedInUser);

            editHelpedViewModel.newLastName = "Bar";

            editHelpedViewModel.currentPassword = loggedInUser.password;
            editHelpedViewModel.currentEmail = loggedInUser.mail;

            // Act
            var viewResult = controller.editHelped(editHelpedViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Helped>(x => x.lastName == editHelpedViewModel.newLastName));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditHelped");
        }

        /*************************************
         * Tutor
         *************************************/
        [TestMethod]
        public void tutor_post_save_with_new_password_and_same_confirmed_password_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var fakedLoggedInTutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedLoggedInTutor);

            var editTutorViewModel = Mapper.Map<EditTutorViewModel>(fakedLoggedInTutor);

            appContext.getRepository().getById<User>(fakedLoggedInTutor.id).ReturnsForAnyArgs(fakedLoggedInTutor);

            editTutorViewModel.newPassword = "password2";
            editTutorViewModel.confirmNewPassword = "password2";

            editTutorViewModel.currentPassword = fakedLoggedInTutor.password;
            editTutorViewModel.currentEmail = fakedLoggedInTutor.mail;

            // Act
            var viewResult = controller.editTutor(editTutorViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Tutor>(x => x.password == editTutorViewModel.newPassword));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditTutor");
        }

        [TestMethod]
        public void tutor_post_save_with_new_email_and_same_confirmed_email_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var fakedLoggedInTutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedLoggedInTutor);

            var editTutorViewModel = Mapper.Map<EditTutorViewModel>(fakedLoggedInTutor);

            appContext.getRepository().getById<User>(fakedLoggedInTutor.id).ReturnsForAnyArgs(fakedLoggedInTutor);

            editTutorViewModel.newEmail = "thenewemail@mail.com";
            editTutorViewModel.confirmNewEmail = "thenewemail@mail.com";

            editTutorViewModel.currentPassword = fakedLoggedInTutor.password;
            editTutorViewModel.currentEmail = fakedLoggedInTutor.mail;

            // Act
            var viewResult = controller.editTutor(editTutorViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Tutor>(x => x.mail == editTutorViewModel.newEmail));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditTutor");
        }

        [TestMethod]
        public void tutor_post_save_with_new_name_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var fakedLoggedInTutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedLoggedInTutor);

            var editTutorViewModel = Mapper.Map<EditTutorViewModel>(fakedLoggedInTutor);

            appContext.getRepository().getById<Helped>(loggedInUser.id).ReturnsForAnyArgs(loggedInUser);

            editTutorViewModel.newName = "Foo";

            editTutorViewModel.currentPassword = fakedLoggedInTutor.password;
            editTutorViewModel.currentEmail = fakedLoggedInTutor.mail;

            // Act
            var viewResult = controller.editTutor(editTutorViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Tutor>(x => x.name == editTutorViewModel.newName));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditTutor");
        }

        [TestMethod]
        public void tutor_post_save_with_new_lastname_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var fakedLoggedInTutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedLoggedInTutor);

            var editTutorViewModel = Mapper.Map<EditTutorViewModel>(fakedLoggedInTutor);

            appContext.getRepository().getById<Tutor>(fakedLoggedInTutor.id).ReturnsForAnyArgs(fakedLoggedInTutor);

            editTutorViewModel.newLastName = "Bar";

            editTutorViewModel.currentPassword = fakedLoggedInTutor.password;
            editTutorViewModel.currentEmail = fakedLoggedInTutor.mail;

            // Act
            var viewResult = controller.editTutor(editTutorViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Tutor>(x => x.lastName == editTutorViewModel.newLastName));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditTutor");
        }

        /*************************************
         * Manager
         *************************************/

        [TestMethod]
        public void manager_post_save_with_new_password_and_same_confirmed_password_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var fakedLoggedInManager = this.appContext.getConfiguration().mainManager;
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedLoggedInManager);

            var editManagerViewModel = Mapper.Map<EditManagerViewModel>(fakedLoggedInManager);

            appContext.getRepository().getById<User>(fakedLoggedInManager.id).ReturnsForAnyArgs(fakedLoggedInManager);

            editManagerViewModel.newPassword = "password2";
            editManagerViewModel.confirmNewPassword = "password2";

            editManagerViewModel.currentPassword = fakedLoggedInManager.password;
            editManagerViewModel.currentEmail = fakedLoggedInManager.mail;

            // Act
            var viewResult = controller.editManager(editManagerViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Manager>(x => x.password == editManagerViewModel.newPassword));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditManager");
        }

        [TestMethod]
        public void manager_post_save_with_new_email_and_same_confirmed_email_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var fakedLoggedInManager = this.appContext.getConfiguration().mainManager;
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedLoggedInManager);

            var editManagerViewModel = Mapper.Map<EditManagerViewModel>(fakedLoggedInManager);

            appContext.getRepository().getById<User>(fakedLoggedInManager.id).ReturnsForAnyArgs(fakedLoggedInManager);

            editManagerViewModel.newEmail = "thenewemail@mail.com";
            editManagerViewModel.confirmNewEmail = "thenewemail@mail.com";

            editManagerViewModel.currentPassword = fakedLoggedInManager.password;
            editManagerViewModel.currentEmail = fakedLoggedInManager.mail;

            // Act
            var viewResult = controller.editManager(editManagerViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Manager>(x => x.mail == editManagerViewModel.newEmail));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditManager");
        }

        [TestMethod]
        public void manager_post_save_with_new_name_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var fakedLoggedInManager = this.appContext.getConfiguration().mainManager;
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedLoggedInManager);

            var editManagerViewModel = Mapper.Map<EditManagerViewModel>(fakedLoggedInManager);

            appContext.getRepository().getById<User>(loggedInUser.id).ReturnsForAnyArgs(fakedLoggedInManager);

            editManagerViewModel.newName = "Foo";

            editManagerViewModel.currentPassword = fakedLoggedInManager.password;
            editManagerViewModel.currentEmail = fakedLoggedInManager.mail;

            // Act
            var viewResult = controller.editManager(editManagerViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Manager>(x => x.name == editManagerViewModel.newName));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditManager");
        }

        [TestMethod]
        public void manager_post_save_with_new_lastname_should_return_edit_view_and_save_to_db()
        {
            // Arrange
            var fakedLoggedInManager = this.appContext.getConfiguration().mainManager;
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedLoggedInManager);

            var editManagerViewModel = Mapper.Map<EditManagerViewModel>(fakedLoggedInManager);

            appContext.getRepository().getById<Manager>(fakedLoggedInManager.id).ReturnsForAnyArgs(fakedLoggedInManager);

            editManagerViewModel.newLastName = "Bar";

            editManagerViewModel.currentPassword = fakedLoggedInManager.password;
            editManagerViewModel.currentEmail = fakedLoggedInManager.mail;

            // Act
            var viewResult = controller.editManager(editManagerViewModel) as ViewResult;

            // Assert
            appContext.getRepository().Received().update(Arg.Is<Manager>(x => x.lastName == editManagerViewModel.newLastName));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditManager");
        }

        
        //----------

        [TestMethod]
        public void edit_action_should_return_edit_tutor_edit_view_when_tutor_is_connected()
        {
            // Arrange
            var fakedTutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedTutor);

            // Act
            var returnedView = this.controller.edit();

            // Assert
            Assert.IsNotNull(returnedView);
            returnedView.AssertViewRendered().ForView("EditTutor");
        }

        [TestMethod]
        public void edit_action_should_return_manager_edit_view_when_manager_is_connected()
        {
            // Arrange
            var fakedManager = this.appContext.getConfiguration().mainManager;
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedManager);

            // Act
            var returnedView = this.controller.edit();

            // Assert
            Assert.IsNotNull(returnedView);
            returnedView.AssertViewRendered().ForView("EditManager");
        }

    }
}
