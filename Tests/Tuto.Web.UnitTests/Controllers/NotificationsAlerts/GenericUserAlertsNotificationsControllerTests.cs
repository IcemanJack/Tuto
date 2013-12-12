using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Notifications;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;
using Tuto.Web.Utilities;

namespace Tuto.Web.UnitTests.Controllers.NotificationsAlerts
{
    [TestClass]
    public class GenericUserAlertsNotificationsControllerTests : GenericNotificationControllerBaseTests
    {
        //
        // Dummy test alert type
        //

        // helped dummy alert type

        //
        // ==============================================
        //

        static GenericUserAlertsNotificationsControllerTests()
        {
            NotificationsConfiguration.currentNotificationsMappingConfiguration
                .Add(typeof(TestHelpedAlert),
                     new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(TestHelpedAlertViewModel), "Test1"));

            NotificationsConfiguration.currentNotificationsMappingConfiguration
                .Add(typeof(TestTutorAlert),
                     new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(TestTutorAlertViewModel), "Test2"));

            NotificationsConfiguration.currentNotificationsMappingConfiguration
                .Add(typeof(TestManagerAlert),
                     new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(TestManagerAlertViewModel), "Test3"));

            NotificationsConfiguration.currentNotificationsMappingConfiguration
                .Add(typeof(TestSharedNotification),
                     new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(TestSharedAlertViewModel), "Test4"));

            // test alert mapping
            Mapper.CreateMap<TestHelpedAlert, TestHelpedAlertViewModel.HelpedViewModel>();
            Mapper.CreateMap<TestTutorAlert, TestTutorAlertViewModel.TutorViewModel>();
            Mapper.CreateMap<TestManagerAlert, TestManagerAlertViewModel.ManagerViewModel>();

            Mapper.CreateMap<TestSharedNotification, TestSharedAlertViewModel.HelpedViewModel>();
            Mapper.CreateMap<TestSharedNotification, TestSharedAlertViewModel.TutorViewModel>();
            Mapper.CreateMap<TestSharedNotification, TestSharedAlertViewModel.ManagerViewModel>();
        }

        [TestMethod]
        public void a_helped_user_can_view_his_individual_alerts()
        {
            // Arrange
            var helpedUserWithIndividualAlert = this.fixture.Create<Helped>();
            var dummyAlert = this.fixture.Create<TestHelpedAlert>();

            helpedUserWithIndividualAlert.individualNotifications.Add(dummyAlert);
            TestsUtilities.bypassAppAuthentification(this.appContext, helpedUserWithIndividualAlert);

            // Act
            PartialViewResult alertsViewResult = this.controller.getLoggedInUserAlerts();

            // Assert
            Assert.IsNotNull(alertsViewResult);

            var returnedViewModel = alertsViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.IsNotNull(returnedViewModel.FirstOrDefault());

            var returnedAlertViewModel = returnedViewModel.First().Value as TestHelpedAlertViewModel.HelpedViewModel;

            Assert.IsNotNull(returnedAlertViewModel);
            Assert.AreEqual(dummyAlert.testProp, returnedAlertViewModel.testProp);
        }

        [TestMethod]
        public void a_helped_user_can_view_his_individual_alerts_and_his_shared_alerts()
        {
            // Arrange
            var helpedUserWithMultipleAlerts = this.fixture.Create<Helped>();
            var dummyIndividualAlert = this.fixture.Create<TestHelpedAlert>();
            var dummySharedAlert = this.fixture.Create<TestSharedNotification>();

            helpedUserWithMultipleAlerts.individualNotifications.Add(dummyIndividualAlert);
            helpedUserWithMultipleAlerts.sharedNotifications.Add(dummySharedAlert);
            TestsUtilities.bypassAppAuthentification(this.appContext, helpedUserWithMultipleAlerts);

            // Act
            PartialViewResult alertsViewResult = this.controller.getLoggedInUserAlerts();

            // Assert
            Assert.IsNotNull(alertsViewResult);

            var returnedViewModel = alertsViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(2, returnedViewModel.Count);
        }

        [TestMethod]
        public void a_tutor_user_can_view_his_individual_alerts()
        {
            // Arrange
            var tutorUserWithIndividualAlert = this.fixture.Create<Tutor>();
            var dummyAlert = this.fixture.Create<TestTutorAlert>();

            tutorUserWithIndividualAlert.individualNotifications.Add(dummyAlert);
            TestsUtilities.bypassAppAuthentification(this.appContext, tutorUserWithIndividualAlert);

            // Act
            PartialViewResult alertsViewResult = this.controller.getLoggedInUserAlerts();

            // Assert
            Assert.IsNotNull(alertsViewResult);

            var returnedViewModel = alertsViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.IsNotNull(returnedViewModel.FirstOrDefault());

            var returnedAlertViewModel = returnedViewModel.First().Value as TestTutorAlertViewModel.TutorViewModel;

            Assert.IsNotNull(returnedAlertViewModel);
            Assert.AreEqual(dummyAlert.testProp, returnedAlertViewModel.testProp);
        }

        [TestMethod]
        public void a_tutor_user_can_view_his_individual_alerts_and_his_shared_alerts()
        {
            // Arrange
            var tutorUserWithMultipleAlerts = this.fixture.Create<Tutor>();
            var dummyIndividualAlert = this.fixture.Create<TestTutorAlert>();
            var dummySharedAlert = this.fixture.Create<TestSharedNotification>();

            tutorUserWithMultipleAlerts.individualNotifications.Add(dummyIndividualAlert);
            tutorUserWithMultipleAlerts.sharedNotifications.Add(dummySharedAlert);
            TestsUtilities.bypassAppAuthentification(this.appContext, tutorUserWithMultipleAlerts);

            // Act
            PartialViewResult alertsViewResult = this.controller.getLoggedInUserAlerts();

            // Assert
            Assert.IsNotNull(alertsViewResult);

            var returnedViewModel = alertsViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(2, returnedViewModel.Count);
        }

        [TestMethod]
        public void a_manager_can_view_his_individual_alerts()
        {
            // Arrange
            var managerUserWithIndividualAlert = this.fixture.Create<Manager>();
            var dummyAlert = this.fixture.Create<TestManagerAlert>();

            managerUserWithIndividualAlert.individualNotifications.Add(dummyAlert);
            TestsUtilities.bypassAppAuthentification(this.appContext, managerUserWithIndividualAlert);

            // Act
            PartialViewResult alertsViewResult = this.controller.getLoggedInUserAlerts();

            // Assert
            Assert.IsNotNull(alertsViewResult);

            var returnedViewModel = alertsViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.IsNotNull(returnedViewModel.FirstOrDefault());

            var returnedAlertViewModel = returnedViewModel.First().Value as TestManagerAlertViewModel.ManagerViewModel;

            Assert.IsNotNull(returnedAlertViewModel);
            Assert.AreEqual(dummyAlert.testProp, returnedAlertViewModel.testProp);
        }

        [TestMethod]
        public void a_manager_can_view_his_individual_alerts_and_his_shared_alerts()
        {
            // Arrange
            var managerUserWithMultipleAlerts = this.fixture.Create<Manager>();
            var dummyIndividualAlert = this.fixture.Create<TestManagerAlert>();
            var dummySharedAlert = this.fixture.Create<TestSharedNotification>();

            managerUserWithMultipleAlerts.individualNotifications.Add(dummyIndividualAlert);
            managerUserWithMultipleAlerts.sharedNotifications.Add(dummySharedAlert);
            TestsUtilities.bypassAppAuthentification(this.appContext, managerUserWithMultipleAlerts);

            // Act
            PartialViewResult alertsViewResult = this.controller.getLoggedInUserAlerts();

            // Assert
            Assert.IsNotNull(alertsViewResult);

            var returnedViewModel = alertsViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(2, returnedViewModel.Count);
        }

        public class TestHelpedAlert : HelpedBaseNotification
        {
            public TestHelpedAlert() : base(NotificationTypes.ALERT)
            {}

            public string testProp { get; set; }
        }

        public class TestHelpedAlertViewModel
        {
            public class HelpedViewModel
            {
                public string testProp { get; set; }
            }
        }

        public class TestManagerAlert : ManagerBaseNotification
        {
            public TestManagerAlert() : base(NotificationTypes.ALERT)
            { }

            public string testProp { get; set; }
        }

        public class TestManagerAlertViewModel
        {
            public class ManagerViewModel
            {
                public string testProp { get; set; }
            }
        }

        public class TestSharedAlertViewModel
        {
            public class HelpedViewModel
            {
                public string testProp { get; set; }
            }

            public class ManagerViewModel
            {
                public string testProp { get; set; }
            }

            public class TutorViewModel
            {
                public string testProp { get; set; }
            }
        }

        public class TestSharedNotification : SharedBaseNotification
        {
            public TestSharedNotification() : base(NotificationTypes.ALERT)
            { }

            public string testProp { get; set; }
        }

        public class TestTutorAlert : TutorBaseNotification
        {
            public TestTutorAlert() : base(NotificationTypes.ALERT)
            { }

            public string testProp { get; set; }
        }

        public class TestTutorAlertViewModel
        {
            public class TutorViewModel
            {
                public string testProp { get; set; }
            }
        }
    }
}