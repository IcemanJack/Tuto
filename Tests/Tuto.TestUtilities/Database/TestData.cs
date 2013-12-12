using System;
using System.Collections.Generic;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;

namespace Tuto.TestUtilities.Database
{
    public class TestData
    {
        static public Tutor tutor
        {
            get
            {
                var testTutor = new Tutor()
                {
                    mail = "jesuisuntest@hotmail.com",
                    password = "salut",
                    name = "Bonjour",
                    lastName = "Allo",
                    tutorAvailableForGroupSession = true,
                    tutorAvailableForIndividualSession = true,
                    tutorState = TutorState.TO_BE_CONFIRMED
                };

                return testTutor;
            }
        }

        static public HelpRequest helpRequest
        {
            get
            {
                var testHelpRequest = new HelpRequest()
                {
                    id = 1,
                    course = new Course() { courseName = "Français 1" },
                    helped = new Helped() { id = 3, lastName = "Helped", mail = "helped@horsemail.com", name = "VAGUE", password = "mypassword" },
                    tutor = new Tutor() { id = 2, lastName = "Tutor", mail = "tutor@horsemail.com", name = "YOLO", password = "mypassword" },
                    misunderstoodNotions = Resources.Resources.TestDataHelpRequestMisunderstoodNotions,
                    scheduleJson = Resources.Resources.TestDataHelpRequestScheduleJson,
                    comment = Resources.Resources.TestDataHelpRequestComment
                };

                return testHelpRequest;
            }
        }
        
        static public IndividualSession individualSession
        {
            get
            {
                var testIndividualSession = new IndividualSession()
                {
                    id = 1,
                    helpedMessage = "WOW, FANTASTIQUE!",
                    date = new DateTime(2013, 10, 23, 12, 30, 00),
                    helpRequest = helpRequest,
                    helpRequestId = 1,
                    tutorMessage = "L'ETUDIANT NEST PAS TRÈS INTELLIGENT"
                };
                return testIndividualSession;
            }
        }

        static public ScheduleBlock[] scheduleBlocks
        {
            get
            {
                var blocks = new List<ScheduleBlock>
                {
                    new ScheduleBlock() {id = 0, startTime = 10, weekDay = WeekDay.FRIDAY},
                    new ScheduleBlock() {id = 1, startTime = 11, weekDay = WeekDay.FRIDAY},
                    new ScheduleBlock() {id = 2, startTime = 8, weekDay = WeekDay.MONDAY},
                    new ScheduleBlock() {id = 3, startTime = 15, weekDay = WeekDay.MONDAY}
                };

                return blocks.ToArray();
            }
        }
    }
}
