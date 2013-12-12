using System;
using System.Collections.Generic;
using Tuto.DataLayer.Models.Notifications.Manager;
using Tuto.DataLayer.Models.Notifications.Shared;
using Tuto.DataLayer.Models.Notifications.Tutor;
using Tuto.Web.Utilities;
using Tuto.Web.ViewModels.Notifications;

namespace Tuto.Web.Config
{
    public class NotificationsConfiguration
    {
        /*
         * Dynamic dyanmic object type transposition for notications 
         * { Type(OfTheNotification, TheViewModelMappingConfiguration(Type(ofTheViewModel), "TheViewSuffitToUse")) },
         * { ... },
         * ...
         */
         public static Dictionary<Type, NotificationUtilities.NotificationViewModelMappingConfiguration> currentNotificationsMappingConfiguration = new Dictionary<Type, NotificationUtilities.NotificationViewModelMappingConfiguration>()
         {
            {typeof(AssignedToHelpRequestTask), new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(AssignedToHelpRequestTaskViewModel), "AssignedToHelpRequestTask")},
            {typeof(AssignedToGroupSessionAlert), new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(AssignedToGroupSessionAlertViewModel), "AssignedToGroupSessionAlert")},
            {typeof(TutorHasRegisteredTask), new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(TutorHasRegisteredTaskViewModel), "TutorHasRegisteredTask")},
            {typeof(HelpRequestToAssignAlert), new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(HelpRequestToAssignAlertViewModel), "HelpRequestToAssignAlert")},
            {typeof(HelpedHasRegisteredAlert), new NotificationUtilities.NotificationViewModelMappingConfiguration(typeof(HelpedHasRegisteredAlertViewModel), "HelpedHasRegisteredAlert")}
         }; 
    }
}