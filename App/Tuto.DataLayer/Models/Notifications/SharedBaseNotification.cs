using System.Collections.Generic;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Users;

namespace Tuto.DataLayer.Models.Notifications
{
    public abstract class SharedBaseNotification : BaseNotification
    {
        protected SharedBaseNotification(NotificationTypes type) : base(type)
        {
            this.concernedUsers = new HashSet<User>();
        }

        public virtual ICollection<User> concernedUsers { get; set; }

        public override bool isASharedAlert()
        {
            return true;
        }

        public abstract class AbstractSharedBaseNotificationBuilder<T> : AbstractNotificationBuilder<T>  where T : SharedBaseNotification 
        {
            protected AbstractSharedBaseNotificationBuilder(T buildInstance)
                : base(buildInstance)
            { }

            public dynamic addConcernedUser(User userToAdd)
            {
                this.buildInstance.concernedUsers.Add(userToAdd);
                return this;
            }
        }
    }
}