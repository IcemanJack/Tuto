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
    public class GenericUserTasksNotificationsControllerTests : GenericNotificationControllerBaseTests
    {
        //
        // Dummy test Task type
        //

        // helped dummy Task type

        //
        // ==============================================
        //

        static GenericUserTasksNotificationsControllerTests()
        {
            NotificationsConfiguration.currentNotificationsMappingConfiguration
                .Add(typeof(TestHelpedTask),
                     new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(TestHelpedTaskViewModel), "Test1"));

            NotificationsConfiguration.currentNotificationsMappingConfiguration
                .Add(typeof(TestTutorTask),
                     new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(TestTutorTaskViewModel), "Test2"));

            NotificationsConfiguration.currentNotificationsMappingConfiguration
                .Add(typeof(TestManagerTask),
                     new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(TestManagerTaskViewModel), "Test3"));

            NotificationsConfiguration.currentNotificationsMappingConfiguration
                .Add(typeof(TestSharedNotification),
                     new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(TestSharedTaskViewModel), "Test4"));

            // test Task mapping
            Mapper.CreateMap<TestHelpedTask, TestHelpedTaskViewModel.HelpedViewModel>();
            Mapper.CreateMap<TestTutorTask, TestTutorTaskViewModel.TutorViewModel>();
            Mapper.CreateMap<TestManagerTask, TestManagerTaskViewModel.ManagerViewModel>();

            Mapper.CreateMap<TestSharedNotification, TestSharedTaskViewModel.HelpedViewModel>();
            Mapper.CreateMap<TestSharedNotification, TestSharedTaskViewModel.TutorViewModel>();
            Mapper.CreateMap<TestSharedNotification, TestSharedTaskViewModel.ManagerViewModel>();
        }

        [TestMethod]
        public void a_helped_user_can_view_his_individual_tasks()
        {
            // Arrange
            var helpedUserWithIndividualTask = this.fixture.Create<Helped>();
            var dummyTask = this.fixture.Create<TestHelpedTask>();

            helpedUserWithIndividualTask.individualNotifications.Add(dummyTask);
            TestsUtilities.bypassAppAuthentification(this.appContext, helpedUserWithIndividualTask);

            // Act
            PartialViewResult tasksViewResult = this.controller.getLoggedInUserTasks();

            // Assert
            Assert.IsNotNull(tasksViewResult);

            var returnedViewModel = tasksViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.IsNotNull(returnedViewModel.FirstOrDefault());

            var returnedTaskViewModel = returnedViewModel.First().Value as TestHelpedTaskViewModel.HelpedViewModel;

            Assert.IsNotNull(returnedTaskViewModel);
            Assert.AreEqual(dummyTask.testProp, returnedTaskViewModel.testProp);
        }

        [TestMethod]
        public void a_helped_user_can_view_his_individual_tasks_and_his_shared_tasks()
        {
            // Arrange
            var helpedUserWithMultipleTasks = this.fixture.Create<Helped>();
            var dummyIndividualTask = this.fixture.Create<TestHelpedTask>();
            var dummySharedTask = this.fixture.Create<TestSharedNotification>();

            helpedUserWithMultipleTasks.individualNotifications.Add(dummyIndividualTask);
            helpedUserWithMultipleTasks.sharedNotifications.Add(dummySharedTask);
            TestsUtilities.bypassAppAuthentification(this.appContext, helpedUserWithMultipleTasks);

            // Act
            PartialViewResult tasksViewResult = this.controller.getLoggedInUserTasks();

            // Assert
            Assert.IsNotNull(tasksViewResult);

            var returnedViewModel = tasksViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(2, returnedViewModel.Count);
        }

        [TestMethod]
        public void a_tutor_user_can_view_his_individual_tasks()
        {
            // Arrange
            var tutorUserWithIndividualTask = this.fixture.Create<Tutor>();
            var dummyTask = this.fixture.Create<TestTutorTask>();

            tutorUserWithIndividualTask.individualNotifications.Add(dummyTask);
            TestsUtilities.bypassAppAuthentification(this.appContext, tutorUserWithIndividualTask);

            // Act
            PartialViewResult tasksViewResult = this.controller.getLoggedInUserTasks();

            // Assert
            Assert.IsNotNull(tasksViewResult);

            var returnedViewModel = tasksViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.IsNotNull(returnedViewModel.FirstOrDefault());

            var returnedTaskViewModel = returnedViewModel.First().Value as TestTutorTaskViewModel.TutorViewModel;

            Assert.IsNotNull(returnedTaskViewModel);
            Assert.AreEqual(dummyTask.testProp, returnedTaskViewModel.testProp);
        }

        [TestMethod]
        public void a_tutor_user_can_view_his_individual_tasks_and_his_shared_Tasks()
        {
            // Arrange
            var tutorUserWithMultipleTasks = this.fixture.Create<Tutor>();
            var dummyIndividualTask = this.fixture.Create<TestTutorTask>();
            var dummySharedTask = this.fixture.Create<TestSharedNotification>();

            tutorUserWithMultipleTasks.individualNotifications.Add(dummyIndividualTask);
            tutorUserWithMultipleTasks.sharedNotifications.Add(dummySharedTask);
            TestsUtilities.bypassAppAuthentification(this.appContext, tutorUserWithMultipleTasks);

            // Act
            PartialViewResult tasksViewResult = this.controller.getLoggedInUserTasks();

            // Assert
            Assert.IsNotNull(tasksViewResult);

            var returnedViewModel = tasksViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(2, returnedViewModel.Count);
        }

        [TestMethod]
        public void a_manager_can_view_his_individual_tasks()
        {
            // Arrange
            var managerUserWithIndividualTask = this.fixture.Create<Manager>();
            var dummyTask = this.fixture.Create<TestManagerTask>();

            managerUserWithIndividualTask.individualNotifications.Add(dummyTask);
            TestsUtilities.bypassAppAuthentification(this.appContext, managerUserWithIndividualTask);

            // Act
            PartialViewResult tasksViewResult = this.controller.getLoggedInUserTasks();

            // Assert
            Assert.IsNotNull(tasksViewResult);

            var returnedViewModel = tasksViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.IsNotNull(returnedViewModel.FirstOrDefault());

            var returnedTaskViewModel = returnedViewModel.First().Value as TestManagerTaskViewModel.ManagerViewModel;

            Assert.IsNotNull(returnedTaskViewModel);
            Assert.AreEqual(dummyTask.testProp, returnedTaskViewModel.testProp);
        }

        [TestMethod]
        public void a_manager_can_view_his_individual_tasks_and_his_shared_tasks()
        {
            // Arrange
            var managerUserWithMultipleTasks = this.fixture.Create<Manager>();
            var dummyIndividualTask = this.fixture.Create<TestManagerTask>();
            var dummySharedTask = this.fixture.Create<TestSharedNotification>();

            managerUserWithMultipleTasks.individualNotifications.Add(dummyIndividualTask);
            managerUserWithMultipleTasks.sharedNotifications.Add(dummySharedTask);
            TestsUtilities.bypassAppAuthentification(this.appContext, managerUserWithMultipleTasks);

            // Act
            PartialViewResult tasksViewResult = this.controller.getLoggedInUserTasks();

            // Assert
            Assert.IsNotNull(tasksViewResult);

            var returnedViewModel = tasksViewResult.ViewData.Model as List<KeyValuePair<string, object>>;

            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(2, returnedViewModel.Count);
        }

        public class TestHelpedTask : HelpedBaseNotification
        {
            public TestHelpedTask() : base(NotificationTypes.TASK)
            {}

            public string testProp { get; set; }
        }

        public class TestHelpedTaskViewModel
        {
            public class HelpedViewModel
            {
                public string testProp { get; set; }
            }
        }

        public class TestManagerTask : ManagerBaseNotification
        {
            public TestManagerTask() : base(NotificationTypes.TASK)
            { }

            public string testProp { get; set; }
        }

        public class TestManagerTaskViewModel
        {
            public class ManagerViewModel
            {
                public string testProp { get; set; }
            }
        }

        // shared dummy Task type
        public class TestSharedNotification : SharedBaseNotification
        {
            public TestSharedNotification() : base(NotificationTypes.TASK)
            { }

            public string testProp { get; set; }
        }

        public class TestSharedTaskViewModel
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

        public class TestTutorTask : TutorBaseNotification
        {
            public TestTutorTask() : base(NotificationTypes.TASK)
            { }

            public string testProp { get; set; }
        }

        public class TestTutorTaskViewModel
        {
            public class TutorViewModel
            {
                public string testProp { get; set; }
            }
        }
    }
}