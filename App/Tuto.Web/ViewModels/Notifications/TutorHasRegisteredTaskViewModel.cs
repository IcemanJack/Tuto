namespace Tuto.Web.ViewModels.Notifications
{
    public class TutorHasRegisteredTaskViewModel
    {
        public class ManagerViewModel : AbstractNotificationViewModel
        {
            public int tutorId { get; set; }
            public string tutorFirstName { get; set; }
            public string tutorLastName { get; set; }
        }
    }
}