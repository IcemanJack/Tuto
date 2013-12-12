using System;

namespace Tuto.Web.ViewModels.Notifications
{
    public class AssignedToGroupSessionAlertViewModel
    {
        public class TutorViewModel : AbstractNotificationViewModel
        {
            public DateTime groupSessionPlannedDate { get; set; }
            public string groupSessionPlace { get; set; }
        }
    }
}