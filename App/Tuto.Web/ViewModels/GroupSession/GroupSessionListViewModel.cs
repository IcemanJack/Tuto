using System;
using System.Collections.Generic;
using Tuto.DataLayer.Models.GroupSessions;

namespace Tuto.Web.ViewModels.GroupSession
{
    public class GroupSessionListViewModel
    {
        public int id { get; set; }
        public DateTime groupSessionDate { get; set; }
        public string weekDayStr { get; set; }
        public int startTime { get; set; }
        public int endTime { get; set; }
        public string place { get; set; }

        public class ManagerViewModel : GroupSessionListViewModel
        {
            public bool hasAssignedTutor { get; set; }
            public string tutorFirstName { get; set; }
            public string tutorLastName { get; set; }
        }
    }
}