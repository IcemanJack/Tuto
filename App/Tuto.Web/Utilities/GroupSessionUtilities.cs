using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Repository;

namespace Tuto.Web.Utilities
{
    public class GroupSessionUtilities
    {
        public static ICollection<DefaultGroupSession> getDefaultGroupSessionsFromJson(string jsonString, IEntityRepository repository)
        {
            var groupSessionsList = new HashSet<DefaultGroupSession>();

            dynamic groupSessions = JsonConvert.DeserializeObject(jsonString);
            foreach (var groupSession in groupSessions)
            {
                int day = groupSession.day;
                int startTime = groupSession.startTime;
                string place = groupSession.place;

                var startScheduleBlock = repository.single<ScheduleBlock>(x => (x.startTime == startTime && (Int32)x.weekDay == day));

                var newGroupSession = new DefaultGroupSession() { place = place, startScheduleBlock = startScheduleBlock };
                groupSessionsList.Add(newGroupSession);
            }

            return groupSessionsList;
        }

        public static string getJsonFromDefaultGroupSessions(ICollection<DefaultGroupSession> defaultGroupSessions)
        {
            var dynamicGroupSessions = new List<dynamic>();

            foreach (var defaultGroupSession in defaultGroupSessions)
            {
                dynamic dynamicGroupSession = new System.Dynamic.ExpandoObject();
                dynamicGroupSession.day = (Int32) defaultGroupSession.startScheduleBlock.weekDay;
                dynamicGroupSession.startTime = defaultGroupSession.startScheduleBlock.startTime;
                dynamicGroupSession.endTime = dynamicGroupSession.startTime + 2;
                dynamicGroupSession.place = defaultGroupSession.place;

                dynamicGroupSessions.Add(dynamicGroupSession);  
            }

            return JsonConvert.SerializeObject(dynamicGroupSessions.ToArray());
        }

    }
}
