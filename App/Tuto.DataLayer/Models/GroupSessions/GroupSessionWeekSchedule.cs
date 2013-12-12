using System;
using System.Collections.Generic;
using Tuto.DataLayer.Models.Users;

namespace Tuto.DataLayer.Models.GroupSessions
{
    public class GroupSessionWeekSchedule : Entity
    {
        public GroupSessionWeekSchedule()
        {
            this.groupSessions = new HashSet<AssignedGroupSession>();
        }

        public DateTime weekStartDay { get; set; }

        public virtual ICollection<AssignedGroupSession> groupSessions { get; set; }
        public virtual Manager manager { get; set; }

        public static DateTime getNextWeekStartDay()
        {
            var result = DateTime.Now;
            var dayDelta = DayOfWeek.Sunday - result.DayOfWeek;
            var nextWeekStartDay = result.AddDays(dayDelta + 7);

            return nextWeekStartDay;
        }
    }
}
