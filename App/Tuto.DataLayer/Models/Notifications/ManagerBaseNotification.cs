using Tuto.DataLayer.Enums;

namespace Tuto.DataLayer.Models.Notifications
{
    public abstract class ManagerBaseNotification : BaseNotification
    {
        protected ManagerBaseNotification(NotificationTypes type) : base(type)
        { }

        public int managerUserId { get; set; }

        // navigation properties
        public virtual Users.Manager managerUser { get; set; }

        public override bool isASharedAlert()
        {
            return false;
        }

        public abstract class AbstractManagerBaseNotificationBuilder<T> : AbstractNotificationBuilder<T> where T : ManagerBaseNotification
        {
            protected AbstractManagerBaseNotificationBuilder(T buildInstance) : base(buildInstance)
            { }

            public dynamic setConcernedManager(Users.Manager concernedManager)
            {
                this.buildInstance.managerUser = concernedManager;
                return this;
            }
        }
    }
}
