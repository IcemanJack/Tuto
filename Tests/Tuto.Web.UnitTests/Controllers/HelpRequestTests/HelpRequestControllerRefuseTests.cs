using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers;

namespace Tuto.Web.UnitTests.Controllers.HelpRequestTests
{
    [TestClass]
    public class HelpRequestControllerRefuseTests : HelpRequestControllerBaseTests
    {
        private HelpRequest createHelpRequest()
        {
            var helpRequest = this.fixture.Create<HelpRequest>();
            var individualSession = this.fixture.Create<IndividualSession>();
            var tutor = this.fixture.Create<Tutor>();

            helpRequest.individualSession = individualSession;
            helpRequest.tutor = tutor;

            return helpRequest;
        }

        [TestMethod]
        public void refuse_action_should_kick_user_if_no_user_is_logged_in()
        {
            // Act
            var actionResult = this.controller.refuse(0);

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void refuse_action_should_kick_user_if_user_is_not_helped_or_tutor()
        {
            // Arrange
            var manager = this.fixture.Create<DataLayer.Models.Users.Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, manager);

            // Act
            var actionResult = this.controller.refuse(0);

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<AccountController>(x => x.unauthorized());
        }

        [TestMethod]
        public void refuse_action_should_return_to_list_if_help_request_id_is_invalid()
        {
            // Arrange
            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            // Act
            var actionResult = this.controller.refuse(0);

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<HelpRequestController>(x => x.list());
        }

        [TestMethod]
        public void refuse_action_should_return_to_list_on_success()
        {
            // Arrange
            var helpRequest = this.createHelpRequest();
            this.appContext.getRepository().getById<HelpRequest>(helpRequest.id).Returns(helpRequest);

            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            // Act
            var actionResult = this.controller.refuse(helpRequest.id);

            // Assert
            actionResult.AssertActionRedirect()
                .ToAction<HelpRequestController>(x => x.list());
        }

        [TestMethod]
        public void refuse_action_should_remove_tutor_and_individual_session_in_help_request_with_helped()
        {
            // Arrange
            var helpRequest = this.createHelpRequest();
            this.appContext.getRepository().getById<HelpRequest>(helpRequest.id).Returns(helpRequest);

            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            // Act
            this.controller.refuse(helpRequest.id);

            // Assert
            Assert.IsNull(helpRequest.tutor);
            Assert.IsNull(helpRequest.individualSession);
            this.appContext.getRepository().ReceivedWithAnyArgs().delete<IndividualSession>(0);
            this.appContext.getRepository().Received().update(Arg.Is<HelpRequest>(x => x.id == helpRequest.id));
        }

        [TestMethod]
        public void refuse_action_should_remove_tutor_and_individual_session_in_help_request_with_tutor()
        {
            // Arrange
            var helpRequest = this.createHelpRequest();
            this.appContext.getRepository().getById<HelpRequest>(helpRequest.id).Returns(helpRequest);

            var tutor = this.fixture.Create<Tutor>();
            TestsUtilities.bypassAppAuthentification(this.appContext, tutor);

            // Act
            this.controller.refuse(helpRequest.id);

            // Assert
            Assert.IsNull(helpRequest.tutor);
            Assert.IsNull(helpRequest.individualSession);
            this.appContext.getRepository().ReceivedWithAnyArgs().delete<IndividualSession>(0);
            this.appContext.getRepository().Received().update(Arg.Is<HelpRequest>(x => x.id == helpRequest.id));
        }
    }
}
