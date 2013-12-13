using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers;
using Tuto.Web.ViewModels.HelpRequest;

namespace Tuto.Web.UnitTests.Controllers.HelpRequestTests
{
    [TestClass]
    public class HelpRequestControllerListTests : HelpRequestControllerBaseTests
    {
        [TestMethod]
        public void index_action_should_redirect_to_list_view()
        {
            // Arrange
            var helpedUser = fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helpedUser);

            // Act
            var actionResult = controller.index();

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<HelpRequestController>(x => x.list());
        }

        [TestMethod]
        public void list_action_should_return_helped_list_when_helpeduser_is_logged_in()
        {
            // Arrange
            var helpedUser = fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helpedUser);

            // Act
            var returnedResult = controller.list();

            // Assert
            Assert.IsNotNull(returnedResult);
            returnedResult.AssertViewRendered().ForView("Helped_List");
        }

        [TestMethod]
        public void list_action_should_return_tutor_list_when_tutoruser_is_logged_in()
        {
            // Arrange
            var tutorUser = fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, tutorUser);

            // Act
            var returnedResult = controller.list();

            // Assert
            Assert.IsNotNull(returnedResult);
            returnedResult.AssertViewRendered().ForView("Tutor_List");
        }

        [TestMethod]
        public void list_action_should_return_helped_list_view_model_when_helped_user_is_loggedin()
        {
            // Arrange
            var helpedUser = fixture.Create<Helped>();
            
            var expectedHelpRequests = fixture.CreateMany<HelpRequest>(10).ToList();
            helpedUser.helpRequests = expectedHelpRequests;

            TestsUtilities.bypassAppAuthentification(this.appContext, helpedUser);

            // Act
            var viewResult = this.controller.list() as ViewResult;
            
            // Assert
            Assert.IsNotNull(viewResult);

            var viewModelObtained = viewResult.ViewData.Model as HelpedHelpRequestListViewModel;
            Assert.IsNotNull(viewModelObtained);
            Assert.IsNotNull(expectedHelpRequests);
            Assert.AreEqual(expectedHelpRequests.Count, viewModelObtained.allHelpRequests.Count);
        }

        [TestMethod]
        public void list_action_should_return_tutor_list_entry_when_tutor_user_is_loggedin()
        {
            // Arrange
            var tutorUser = fixture.Create<Tutor>();

            var expectedHelpRequests = fixture.CreateMany<HelpRequest>(10).ToList();
            tutorUser.helpRequests = expectedHelpRequests;

            TestsUtilities.bypassAppAuthentification(this.appContext, tutorUser);

            // Act
            var viewResult = this.controller.list() as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);

            var viewModelObtained = viewResult.ViewData.Model as List<TutorHelpRequestListEntryViewModel>;
            Assert.IsNotNull(viewModelObtained);
            Assert.IsNotNull(expectedHelpRequests);
            Assert.AreEqual(expectedHelpRequests.Count, viewModelObtained.Count);
        }

        [TestMethod]
        public void list_action_should_return_to_login_if_user_is_not_helped_or_tutor()
        {
            // Arrange
            var managerUser = fixture.Create<DataLayer.Models.Users.Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, managerUser);

            // Act
            var actionResult = this.controller.list();

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void list_action_should_return_to_login_if_no_user_is_logged_in()
        {
            // Act
            var redirectToRoute = this.controller.list();

            // Assert
            redirectToRoute.AssertActionRedirect().ToAction<AccountController>(x => x.unauthorized());
        }
    }
}
