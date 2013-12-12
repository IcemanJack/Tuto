using System;
using Tuto.DataLayer.Enums;

namespace Tuto.DataLayer.Models.Notifications
{
    public abstract class BaseNotification : Entity
    {
        protected BaseNotification(NotificationTypes alertType)
        {
            this.createdTime = DateTime.Now;
            this.hasBeenSeen = false;
            this.type = alertType;
        }

        public DateTime createdTime { get; set; }
        public bool hasBeenSeen { get; set; }
        public virtual NotificationTypes type { get; set; }

        public NotificationTypes getNotificationType()
        {
            return this.type;
        }

        public abstract bool isASharedAlert();

        public abstract class AbstractNotificationBuilder<T> where T : BaseNotification
        {
            protected T buildInstance;

            protected AbstractNotificationBuilder(T buildInstance)
            {
                this.buildInstance = buildInstance;
            }

            public T getNotification()
            {
                return this.buildInstance;
            }
        }

    }
}