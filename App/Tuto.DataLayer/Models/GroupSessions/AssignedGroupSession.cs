using System;
using Tuto.DataLayer.Models.Users;

namespace Tuto.DataLayer.Models.GroupSessions
{
    public class AssignedGroupSession : Entity
    {
        public String place { get; set; }

        public virtual ScheduleBlock startScheduleBlock { get; set; }
        public virtual Tutor tutor { get; set; }
        public virtual GroupSessionWeekSchedule weekSchedule { get; set; }

        public DateTime getDate()
        {
            return this.weekSchedule.weekStartDay.AddDays((Int32)this.startScheduleBlock.weekDay - 1);
        }
    }
}
