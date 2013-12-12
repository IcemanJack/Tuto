using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Notifications;
using Tuto.DataLayer.Models.Users;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;
using Tuto.Web.Controllers;
using Tuto.Web.Mappers;

namespace Tuto.Web.UnitTests.Controllers.NotificationsAlerts
{
    [TestClass]
    public class GenericNotificationControllerBaseTests
    {
        protected WebAppLaunchContext appContext;
        protected NotificationsController controller;
        protected Fixture fixture;

        [TestInitialize]
        public void notificationBaseTestInit()
        {
            this.fixture = new Fixture();
            this.fixture.Customizations.Add(new VirtualMembersOmitter());
            AutoMapperConfiguration.Configure();

            this.appContext = new TestWebAppLaunchContext();

            this.controller = new NotificationsController(this.appContext);
        }

        [TestMethod]
        public void delete_individual_notification_with_valid_id_should_successfully_delete()
        {
            // Arrange
            var fakedIndividualNotification = this.fixture.Create<IndividualNotificationTest>();
            this.appContext.getRepository().getById<BaseNotification>(-1).ReturnsForAnyArgs(fakedIndividualNotification);

            // Act
            var returnedJsonResult = this.controller.deleteNotification(-1) as JsonResult;

            // Assert
            Assert.IsNotNull(returnedJsonResult);
            Assert.AreEqual("Success", returnedJsonResult.Data.ToString());
            this.appContext.getRepository().Received().delete<BaseNotification>(Arg.Is<int>(x => x == fakedIndividualNotification.id));
        }

        [TestMethod]
        public void delete_notification_with_invalid_id_should_fail()
        {
            // Arrange
            int invalidNotificationId = -1;

            // Act
            var returnedJsonResult = this.controller.deleteNotification(invalidNotificationId);

            // Assert
            Assert.IsNotNull(returnedJsonResult);
            Assert.AreEqual("Failed", returnedJsonResult.Data.ToString());
        }

        [TestMethod]
        public void delete_shared_notification_with_more_than_one_user_should_remove_user_from_concerned_user_list()
        {
            const int NUM_CREATED_USERS = 3;

            // Arrange
            var fakedSharedNotification = this.fixture.Create<SharedNotificationTest>();
            var fakeHelped = this.fixture.Create<Helped>();
            var fakeTutor = this.fixture.Create<Tutor>();
            var fakeManager = this.fixture.Create<Manager>();

            TestsUtilities.bypassAppAuthentification(this.appContext, fakeHelped);

            fakedSharedNotification.concernedUsers.Add(fakeHelped);
            fakedSharedNotification.concernedUsers.Add(fakeTutor);
            fakedSharedNotification.concernedUsers.Add(fakeManager);

            this.appContext.getRepository().getById<BaseNotification>(-1).ReturnsForAnyArgs(fakedSharedNotification);

            // Act
            var returnedJsonResult = this.controller.deleteNotification(-1);

            // Assert
            Assert.IsNotNull(returnedJsonResult);
            Assert.AreEqual("Success", returnedJsonResult.Data.ToString());
            this.appContext.getRepository().Received().update(Arg.Is<SharedBaseNotification>(x => x.concernedUsers.Count == (NUM_CREATED_USERS - 1)));
        }

        [TestMethod]
        public void delete_shared_notification_with_one_concerned_user_should_remove_completely_the_notification()
        {
            // Arrange
            var fakedSharedNotification = this.fixture.Create<SharedNotificationTest>();
            var fakeHelped = this.fixture.Create<Helped>();

            fakedSharedNotification.concernedUsers.Add(fakeHelped);

            this.appContext.getRepository().getById<BaseNotification>(-1).ReturnsForAnyArgs(fakedSharedNotification);

            // Act
            var returnedJsonResult = this.controller.deleteNotification(-1);

            // Assert
            Assert.IsNotNull(returnedJsonResult);
            Assert.AreEqual("Success", returnedJsonResult.Data.ToString());
            this.appContext.getRepository().Received().delete<BaseNotification>(Arg.Is<int>(x => x == fakedSharedNotification.id));
        }

        protected class IndividualNotificationTest : BaseNotification
        {
            public IndividualNotificationTest() : base(NotificationTypes.ALERT)
            { }

            public override bool isASharedAlert()
            {
                return false;
            }
        }

        protected class SharedNotificationTest : SharedBaseNotification
        {
            public SharedNotificationTest() : base(NotificationTypes.TASK)
            { }
        }
    }
}
