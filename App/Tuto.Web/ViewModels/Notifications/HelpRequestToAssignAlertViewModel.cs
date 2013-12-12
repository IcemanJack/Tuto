using System;

namespace Tuto.Web.ViewModels.Notifications
{
    public class HelpRequestToAssignAlertViewModel
    {
        public class ManagerViewModel : AbstractNotificationViewModel
        {
            public int helpRequestId { get; set; }
            public string helpedFirstName { get; set; }
            public string helpedLastName { get; set; }
            public DateTime helpRequestCreatedTime { get; set; }
        }
    }
}