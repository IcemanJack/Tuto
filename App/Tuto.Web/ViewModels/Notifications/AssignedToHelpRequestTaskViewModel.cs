using System;

namespace Tuto.Web.ViewModels.Notifications
{
    public class AssignedToHelpRequestTaskViewModel
    {
        public class HelpedViewModel : AbstractNotificationViewModel
        {
            public int helpRequestId { get; set; }
            public DateTime helpRequestCreatedDatetime { get; set; }
            public DateTime individualSessionDatetime { get; set; }
        }

        public class TutorViewModel : AbstractNotificationViewModel
        {
            public int helpRequestId { get; set; }
            public DateTime helpRequestCreatedDatetime { get; set; }
            public DateTime individualSessionDatetime { get; set; }
            public string helpedFirstname { get; set; }
            public string helpedLastname { get; set; }
        }
    }
}