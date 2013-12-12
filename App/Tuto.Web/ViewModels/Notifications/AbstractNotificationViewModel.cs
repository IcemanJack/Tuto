using System;

namespace Tuto.Web.ViewModels.Notifications
{
    public abstract class AbstractNotificationViewModel
    {
        public int id { get; set; }
        public DateTime createdTime { get; set; }
    }
}