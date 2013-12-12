using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.ViewModels;

namespace Tuto.Web.UnitTests.Controllers.HelpRequestTests
{
    [TestClass]
    public class HelpRequestControllerReportTests: HelpRequestControllerBaseTests
    {
        [TestMethod]
        public void get_action_should_return_to_helpRequest_list_if_bad_id_is_given()
        {
            // Arrange
            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            // Act
            var actionResult = this.controller.report(1) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsTrue(actionResult.RouteValues["action"].Equals("list"));
            Assert.IsTrue(actionResult.RouteValues["controller"].Equals("HelpRequest"));
        }

        [TestMethod]
        public void get_action_should_return_tutorReport_if_good_id_is_given()
        {
            // Arrange
            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            var individualSession = this.fixture.Create<IndividualSession>();
            individualSession.helpRequestId = 1;
            this.appContext.getRepository().single<IndividualSession>(x => x.helpRequestId == 1)
                .ReturnsForAnyArgs(individualSession);

            // Act
            var viewResult = this.controller.report(1) as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            viewResult.AssertViewRendered().ForView("Report");
        }

        [TestMethod]
        public void post_with_invalid_model_state_should_return_tutorReport()
        {
            // Arrange
            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            var ReportToDoViewModel = this.fixture.Create<ReportToDoViewModel>();
            this.controller.ModelState.AddModelError("Error", "Error");

            var viewResult = this.controller.report(ReportToDoViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            viewResult.AssertViewRendered().ForView("Report");
        }

        [TestMethod]
        public void post_with_tutor_message_should_update_and_return_index_view()
        {
            // Arrange
            var tutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, tutor);

            var ReportToDoViewModel = this.fixture.Create<ReportToDoViewModel>();
            ReportToDoViewModel.helpRequestId = 1;

            var individualSession = this.fixture.Create<IndividualSession>();
            var helpedRequest = this.fixture.Create<HelpRequest>();
            helpedRequest.id = 1;
            helpedRequest.individualSession = individualSession;

            this.appContext.getRepository().single<HelpRequest>(x => x.id == helpedRequest.id)
                .ReturnsForAnyArgs(helpedRequest);

            // Act
            var actionResult = this.controller.report(ReportToDoViewModel) as RedirectToRouteResult;

            // Assert
            this.appContext.getRepository().Received().update(
                Arg.Is<IndividualSession>(x => x.tutorMessage == ReportToDoViewModel.message));

            Assert.IsNotNull(actionResult);
            Assert.IsTrue(actionResult.RouteValues["action"].Equals("list"));
            Assert.IsTrue(actionResult.RouteValues["controller"].Equals("HelpRequest"));
        }

        [TestMethod]
        public void post_with_helped_message_should_update_and_return_index_view()
        {
            // Arrange
            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            var ReportToDoViewModel = this.fixture.Create<ReportToDoViewModel>();
            ReportToDoViewModel.helpRequestId = 1;

            var individualSession = this.fixture.Create<IndividualSession>();
            var helpedRequest = this.fixture.Create<HelpRequest>();
            helpedRequest.id = 1;
            helpedRequest.individualSession = individualSession;

            this.appContext.getRepository().single<HelpRequest>(x => x.id == helpedRequest.id)
                .ReturnsForAnyArgs(helpedRequest);

            // Act
            var actionResult = this.controller.report(ReportToDoViewModel) as RedirectToRouteResult;

            // Assert
            this.appContext.getRepository().Received().update(Arg.Is<IndividualSession>(x => x.helpedMessage == ReportToDoViewModel.message));

            Assert.IsNotNull(actionResult);
            Assert.IsTrue(actionResult.RouteValues["action"].Equals("list"));
            Assert.IsTrue(actionResult.RouteValues["controller"].Equals("HelpRequest"));
        }
    }
}
