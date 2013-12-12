using System;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Tuto.DataLayer.Models.Notifications;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;

namespace Tuto.Web.Utilities
{
    public class NotificationUtilities
    {
        public static NotificationViewModelMappingConfiguration getNotificationMappingConfigForSpecificAlert(BaseNotification toBeMappedNotification)
        {
            NotificationUtilities.NotificationViewModelMappingConfiguration currentNotificationViewModelMapping = null;
            if (!NotificationsConfiguration.currentNotificationsMappingConfiguration.TryGetValue(ObjectContext.GetObjectType(toBeMappedNotification.GetType()), out currentNotificationViewModelMapping))
            {
                throw new NotificationUtilities.NotificationViewModelMappingConfiguration.NoAvailableNotificationViewModelMappingConfiguration();
            }

            return currentNotificationViewModelMapping;
        }

        public class NotificationViewModelMappingConfiguration
        {
            private const string DEFAULT_HELPED_SUBCLASS_NAME = "HelpedViewModel";
            private const string DEFAULT_TUTOR_SUBCLASS_NAME = "TutorViewModel";
            private const string DEFAULT_MANAGER_SUBCLASS_NAME = "ManagerViewModel";

            private const string DEFAULT_PARTIAL_HELPED_SUFFIX = "HelpedPartial";
            private const string DEFAULT_PARTIAL_TUTOR_SUFFIX = "TutorPartial";
            private const string DEFAULT_PARTIAL_MANAGER_SUFFIX = "ManagerPartial";

            public NotificationViewModelMappingConfiguration(Type mainViewModel, string partialViewName)
            {
                this.mainViewModelType = mainViewModel;

                this.helpedViewModelType = this.mainViewModelType.GetNestedTypes().FirstOrDefault(x => x.Name == DEFAULT_HELPED_SUBCLASS_NAME);
                this.tutorViewModelType = this.mainViewModelType.GetNestedTypes().FirstOrDefault(x => x.Name == DEFAULT_TUTOR_SUBCLASS_NAME);
                this.managerViewModelType = this.mainViewModelType.GetNestedTypes().FirstOrDefault(x => x.Name == DEFAULT_MANAGER_SUBCLASS_NAME);

                this.partialViewName = partialViewName;
                this.helpedPartialViewName = partialViewName + DEFAULT_PARTIAL_HELPED_SUFFIX;
                this.tutorPartialViewName = partialViewName + DEFAULT_PARTIAL_TUTOR_SUFFIX;
                this.managerPartialViewName = partialViewName + DEFAULT_PARTIAL_MANAGER_SUFFIX;
            }

            public Type mainViewModelType { get; set; }
            public Type helpedViewModelType { get; set; }
            public Type tutorViewModelType { get; set; }
            public Type managerViewModelType { get; set; }

            public string partialViewName { get; set; }
            public string helpedPartialViewName { get; set; }
            public string tutorPartialViewName { get; set; }
            public string managerPartialViewName { get; set; }


            public string getViewNameForSpecificUser(User fromUser)
            {
                var returnedViewNameBuilder = new StringBuilder(this.partialViewName);
                returnedViewNameBuilder.Append("/");
                if (fromUser.isHelped())
                {
                    returnedViewNameBuilder.Append(this.helpedPartialViewName);
                }
                else if (fromUser.isTutor())
                {
                    returnedViewNameBuilder.Append(this.tutorPartialViewName);
                }
                else if (fromUser.isManager())
                {
                    returnedViewNameBuilder.Append(this.managerPartialViewName);
                }

                return returnedViewNameBuilder.ToString();
            }

            public Type getViewModelForSpecificUser(User fromUser)
            {
                Type returnedType = null;
                if (fromUser.isHelped())
                {
                    returnedType = this.helpedViewModelType;
                }
                else if (fromUser.isTutor())
                {
                    returnedType = this.tutorViewModelType;
                }
                else if (fromUser.isManager())
                {
                    returnedType = this.managerViewModelType;
                }

                if (returnedType == null)
                {
                    throw new UserTypeNotSupportedByAlertType();
                }

                return returnedType;
            }

            public string getPartialViewNameForSpecificUser(User fromUser)
            {
                string returnedPartialViewModel = "";
                if (fromUser.isHelped())
                {
                    returnedPartialViewModel = this.helpedPartialViewName;
                }
                else if (fromUser.isTutor())
                {
                    returnedPartialViewModel = this.tutorPartialViewName;
                }
                else if (fromUser.isManager())
                {
                    returnedPartialViewModel = this.managerPartialViewName;
                }

                return returnedPartialViewModel;
            }

            public class NoAvailableNotificationViewModelMappingConfiguration : Exception
            {

            }

            public class UserTypeNotSupportedByAlertType : Exception
            {

            }
        }
    }
}