using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Notifications;
using Tuto.Web.Config;
using Tuto.Web.Utilities;

namespace Tuto.Web.Controllers
{
    public  class NotificationsController : DefaultController
    {
        static NotificationsController()
        {

        }

        public NotificationsController(WebAppLaunchContext context) : base(context)
        {
            
        }

        public NotificationsController() : this(new WebAppLaunchContext())
        {}

        [HttpPost]
        public virtual JsonResult deleteNotification(int notificationId)
        {
            // get the notification
            var toDeleteNotification = this.appContext.getRepository().getById<BaseNotification>(notificationId);
            if (toDeleteNotification == null)
            {
                return this.Json("Failed");
            }

            dynamic concreteNotification = toDeleteNotification;
            // if the shared alert is still concerned by more than 1 user, only delete the logged in user without entirely deleting the shared alert
            if (concreteNotification is SharedBaseNotification && ((SharedBaseNotification)concreteNotification).concernedUsers.Count > 1)
            {
                var sharedNotification = concreteNotification as SharedBaseNotification;
                sharedNotification.concernedUsers.Remove(this.getLoggedInUser()); // remove the logged in user

                this.appContext.getRepository().update(sharedNotification);
            }
            else // directory delete the notification from db
            {
                this.appContext.getRepository().delete<BaseNotification>(toDeleteNotification.id);
            }

            
            return this.Json("Success");
        }

        [ChildActionOnly]
        public virtual PartialViewResult getLoggedInUserAlerts()
        {
            var viewListViewModel = new List<KeyValuePair<string, object>>();
            IEnumerator<BaseNotification> alertsEnum = this.getLoggedInUser().getNotifications().Where(x => x.getNotificationType() == NotificationTypes.ALERT).ToList().GetEnumerator();
            while (alertsEnum.MoveNext())
            {
                NotificationUtilities.NotificationViewModelMappingConfiguration currentNotificationViewModelMapping = NotificationUtilities.getNotificationMappingConfigForSpecificAlert(alertsEnum.Current);

                // get the view model to display with the current alert
                Type toDisplayViewModelType = currentNotificationViewModelMapping.getViewModelForSpecificUser(this.getLoggedInUser());
                dynamic actualConcreteAlert = alertsEnum.Current;
                var toDisplayViewModel = Mapper.DynamicMap(actualConcreteAlert, ObjectContext.GetObjectType(alertsEnum.Current.GetType()), toDisplayViewModelType);

                viewListViewModel.Add(new KeyValuePair<string, object>(currentNotificationViewModelMapping.getViewNameForSpecificUser(this.getLoggedInUser()), toDisplayViewModel)); // <ViewName, ViewModel>
            }

            return this.PartialView("_AlertsList", viewListViewModel);
        }

        [ChildActionOnly]
        public virtual PartialViewResult getLoggedInUserTasks()
        {
            var viewListViewModel = new List<KeyValuePair<string, object>>();
            IEnumerator<BaseNotification> alertsEnum = this.getLoggedInUser().getNotifications().Where(x => x.getNotificationType() == NotificationTypes.TASK).ToList().GetEnumerator();
            while (alertsEnum.MoveNext())
            {
                NotificationUtilities.NotificationViewModelMappingConfiguration currentNotificationViewModelMapping = NotificationUtilities.getNotificationMappingConfigForSpecificAlert(alertsEnum.Current);

                // get the view model to display with the current alert
                Type toDisplayViewModelType = currentNotificationViewModelMapping.getViewModelForSpecificUser(this.getLoggedInUser());
                dynamic actualConcreteAlert = alertsEnum.Current;
                var toDisplayViewModel = Mapper.DynamicMap(actualConcreteAlert, ObjectContext.GetObjectType(alertsEnum.Current.GetType()), toDisplayViewModelType);

                viewListViewModel.Add(new KeyValuePair<string, object>(currentNotificationViewModelMapping.getViewNameForSpecificUser(this.getLoggedInUser()), toDisplayViewModel)); // <ViewName, ViewModel>
            }

            return this.PartialView("_TasksList", viewListViewModel);
        }

    }
}