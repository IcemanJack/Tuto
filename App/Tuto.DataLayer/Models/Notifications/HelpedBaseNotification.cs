using Tuto.DataLayer.Enums;

namespace Tuto.DataLayer.Models.Notifications
{
    public /*abstract*/ class HelpedBaseNotification : BaseNotification
    {
        protected HelpedBaseNotification(NotificationTypes type) : base(type)
        { }

        public HelpedBaseNotification() : base(NotificationTypes.ALERT)
        { }

        public int helpedUserId { get; set; }

        // navigation properties
        public virtual Users.Helped helpedUser { get; set; }

        public override bool isASharedAlert()
        {
            return false;
        }
    }
}
