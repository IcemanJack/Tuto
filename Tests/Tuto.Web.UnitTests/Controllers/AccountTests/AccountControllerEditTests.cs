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
using Tuto.Web.UnitTests.Generic;

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
            AssertFunctions.assertValidRenderedViewForName(controller.edit(), "EditHelped");
        }

        [TestMethod]
        public void post_save_changes_should_return_view_with_errors_when_modelState_is_not_valid()
        {
            // Arrange
            var editHelpedViewModel = fixture.Create<EditHelpedViewModel>();
            controller.ModelState.AddModelError("Error", "Error");

            AssertFunctions.assertValidRenderedViewForName(controller.editHelped(editHelpedViewModel), "EditHelped");
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

        [TestMethod]
        public void post_save_with_new_password_and_same_confirmed_password_should_return_edit_view_and_save_to_db()
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
            appContext.getRepository().Received().update(Arg.Is<User>(x => x.password == editHelpedViewModel.newPassword));

            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewData["AccountEditSuccessMessage"]);
            viewResult.AssertViewRendered().ForView("EditHelped");
        }

        [TestMethod]
        public void post_save_with_new_email_and_same_confirmed_email_should_return_edit_view_and_save_to_db()
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
        public void post_save_with_new_name_should_return_edit_view_and_save_to_db()
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
        public void post_save_with_new_lastname_should_return_edit_view_and_save_to_db()
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
    }
}
