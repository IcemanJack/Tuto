using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Repository;

namespace Tuto.DataLayer.ModelUtilities
{
    public class ScheduleUtilities
    {
        private const string TIME_FORMAT = "hh:mm tt";
        private const string SCHEDULE_JSON_SCHEMA =
            "{ \"type\": \"array\", \"items\": [{ \"type\" : \"object\",\"properties\" : {\"start\" : {\"type\" : \"string\"},\"end\" : {\"type\" : \"string\"},\"day\" : {\"type\" : \"number\"}},\"additionalProperties\" : false}]}";

        public static bool validateScheduleJsonString(string jsonString)
        {
            try
            {
                var scheduleSchema = JsonSchema.Parse(SCHEDULE_JSON_SCHEMA);
                var objSchedule = JArray.Parse(jsonString);
                return objSchedule.IsValid(scheduleSchema);
            }
            catch (JsonReaderException) // will fail if invalid json syntax
            {
                return false;
            }
        }

        public static string getJsonFromScheduleBlocks(ICollection<ScheduleBlock> scheduleBlocks)
        {
            var partialScheduleBlocks = new List<dynamic>();

            foreach (var scheduleBlock in scheduleBlocks)
            {
                dynamic scheduleBlockPartial = new System.Dynamic.ExpandoObject();
                scheduleBlockPartial.start = hourIntToTimeString(scheduleBlock.startTime);
                scheduleBlockPartial.end = hourIntToTimeString(scheduleBlock.startTime + 1);
                scheduleBlockPartial.day = (Int32) scheduleBlock.weekDay;

                partialScheduleBlocks.Add(scheduleBlockPartial);
            }
            
            return JsonConvert.SerializeObject(partialScheduleBlocks.ToArray()); ;
        }

        public static ICollection<ScheduleBlock> getScheduleBlocksFromJson(string jsonString, IEntityRepository repository)
        {
            if (!validateScheduleJsonString(jsonString))
                return Enumerable.Empty<ScheduleBlock>().ToArray();

            var scheduleBlocksList = new HashSet<ScheduleBlock>();

            dynamic scheduleBlocks = JsonConvert.DeserializeObject(jsonString);
            foreach (var scheduleBlock in scheduleBlocks)
            {
                var timeStart = timeStringToHourInt((string) scheduleBlock.start);
                var timeEnd = timeStringToHourInt((string) scheduleBlock.end);
                int day = scheduleBlock.day;
                for (var i = timeStart; i < timeEnd; i++)
                {
                    int currentBlockTime = i;
                    var currentScheduleBlock = repository.single<ScheduleBlock>(x => (x.startTime == currentBlockTime && (Int32) x.weekDay == day));
                    if (currentScheduleBlock != null)
                    {
                        scheduleBlocksList.Add(currentScheduleBlock);
                    }
                }
            }

            return scheduleBlocksList;
        }

        public static int timeStringToHourInt(string time)
        {
            return DateTime.ParseExact(time, TIME_FORMAT, CultureInfo.InvariantCulture).TimeOfDay.Hours;
        }

        public static string hourIntToTimeString(int hour)
        {
            // 08:00 am
            // 02:00 pm
            var result = new StringBuilder();

            int shortHour;
            string pmAmChoice;
            
            if (hour == 12)
            {
                shortHour = 12;
            }
            else
            {
                shortHour = hour % 12;
            }

            if (hour >= 12)
            {
                pmAmChoice = "pm";
            }
            else
            {
                pmAmChoice = "am";
            }

            result.Append(shortHour);
            result.Append(":00 ");
            result.Append(pmAmChoice);

            return result.ToString();
        }
    }
}