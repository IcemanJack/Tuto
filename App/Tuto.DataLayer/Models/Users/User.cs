using System.Collections.Generic;
using Tuto.DataLayer.Models.Notifications;

namespace Tuto.DataLayer.Models.Users
{
    public abstract class User : Entity
    {
        protected User()
        {
            this.sharedNotifications = new HashSet<SharedBaseNotification>();
        }

        public string mail { get; set; }
        public string password { get; set; }

        public string name { get; set; }
        public string lastName { get; set; }

        // BUG (entity) : (we cannot specifies users alerts here) we need to specify every navigation properties indivudually in every sub-class (its an entity bug)
        public virtual ICollection<SharedBaseNotification> sharedNotifications { get; set; }
        public abstract ICollection<BaseNotification> getNotifications();

        public virtual bool isManager()
        {
            return false;
        }

        public virtual bool isTutor()
        {
            return false;
        }

        public virtual bool isHelped()
        {
            return false;
        }
    }
}
