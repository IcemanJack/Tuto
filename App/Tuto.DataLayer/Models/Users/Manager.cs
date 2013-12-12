using System.Collections.Generic;
using System.Linq;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Notifications;

namespace Tuto.DataLayer.Models.Users
{
    public class Manager : User
    {
        public Manager()
        {
            this.defaultGroupSessions = new HashSet<DefaultGroupSession>();
            this.groupSessionWeekSchedules = new HashSet<GroupSessionWeekSchedule>();
            this.individualNotifications = new HashSet<ManagerBaseNotification>();
        }

        public virtual ICollection<ManagerBaseNotification> individualNotifications { get; set; }

        public virtual ICollection<DefaultGroupSession> defaultGroupSessions { get; set; }
        public virtual ICollection<GroupSessionWeekSchedule> groupSessionWeekSchedules { get; set; }
        public bool hasDefaultSchedule { get; set; }

        public override ICollection<BaseNotification> getNotifications()
        {
            ICollection<BaseNotification> everyAlerts = new HashSet<BaseNotification>();

            this.individualNotifications.ToList().ForEach(everyAlerts.Add);
            this.sharedNotifications.ToList().ForEach(everyAlerts.Add);

            return everyAlerts;
        }

        public override bool isManager()
        {
            return true;
        }

        public GroupSessionWeekSchedule getCurrentGroupSessionWeekSchedule()
        {
            return this.groupSessionWeekSchedules.ToArray().LastOrDefault();
        }

        public override bool Equals(object otherObj)
        {
            if (!(otherObj is Manager))
            {
                return false;
            }

            var otherManager = (Manager) otherObj;
            return (otherManager.mail == this.mail) && (otherManager.password == this.password);
        }

        public override int GetHashCode() { return base.GetHashCode(); }
    }
}
