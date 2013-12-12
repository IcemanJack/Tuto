using System.Collections.Generic;
using System.Linq;
using Tuto.DataLayer.Models.Notifications;

namespace Tuto.DataLayer.Models.Users
{

    public class Helped : Student
    {
        public Helped()
        {
            this.scheduleBlocks = new HashSet<ScheduleBlock>();
            this.helpRequests = new HashSet<HelpRequest>();
            this.individualNotifications = new HashSet<HelpedBaseNotification>();
        }

        public virtual ICollection<HelpRequest> helpRequests { get; set; }
        /*
         * TODO : Explain bug with entity (SF)
         */
        public virtual ICollection<ScheduleBlock> scheduleBlocks { get; set; } 
        public virtual ICollection<HelpedBaseNotification> individualNotifications { get; set; }

        public override ICollection<BaseNotification> getNotifications()
        {
            ICollection<BaseNotification> everyNotifications = new HashSet<BaseNotification>();

            this.individualNotifications.ToList().ForEach(everyNotifications.Add);
            this.sharedNotifications.ToList().ForEach(everyNotifications.Add);

            return everyNotifications;
        }

        public override bool isHelped()
        {
            return true;
        }

        protected virtual bool isAvailableAt(ScheduleBlock[] toBeVerifiedBlocks)
        {
            if (this.scheduleBlocks.Count == 0)
            {
                return false;
            }

            IEnumerator<ScheduleBlock> scheduleEnum = this.scheduleBlocks.GetEnumerator();
            bool blockIsMissing = false;
            while (scheduleEnum.MoveNext() && !blockIsMissing)
            {
                blockIsMissing = !this.scheduleBlocks.Contains(scheduleEnum.Current);
            }

            return !blockIsMissing;
        }

        public bool isAvailableAt(WeekDay theDay, int fromTime, int toTime)
        {
            // constrct the matching schedule block array
            var matchingBlocks = new List<ScheduleBlock>();

            for (int currentTime = fromTime; currentTime < toTime; currentTime++)
            {
                matchingBlocks.Add(new ScheduleBlock() { startTime = currentTime, weekDay = theDay });
            }

            return this.isAvailableAt(matchingBlocks.ToArray());
        }
    }
}
