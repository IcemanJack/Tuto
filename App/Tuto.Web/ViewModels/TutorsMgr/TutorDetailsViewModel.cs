using System;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.GroupSessions;

namespace Tuto.Web.ViewModels.TutorsMgr
{
    public class TutorDetailsViewModel
    {
        public int id { get; set; }

        public string mail { get; set; }

        public string name { get; set; }
        public string lastName { get; set; }

        public int workedHours { get; set; }

        public Boolean tutorAvailableForGroupSession { get; set; }
        public Boolean tutorAvailableForIndividualSession { get; set; }

        public TutorState tutorState { get; set; }
        public Course[] coursesSkills { get; set; }
        public AssignedGroupSession[] groupSessions { get; set; }
        public DataLayer.Models.HelpRequest[] helpRequests { get; set; }
        public string jsonSchedule { get; set; }
    }
}