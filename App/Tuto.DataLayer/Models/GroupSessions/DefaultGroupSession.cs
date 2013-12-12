using System;
using Tuto.DataLayer.Models.Users;

namespace Tuto.DataLayer.Models.GroupSessions
{
    public class DefaultGroupSession : Entity
    {
        public String place { get; set; }

        public virtual ScheduleBlock startScheduleBlock { get; set; }
        public virtual Manager manager { get; set; }
    }
}
