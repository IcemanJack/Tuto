using System;
using System.Data.Entity;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.ModelUtilities;
using Tuto.DataLayer.Repository;

namespace Tuto.DataLayer
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<TutoContext>
    {
        protected override void Seed(TutoContext context)
        {
            var repository = new EntityRepository();

            // TODO seed tables dynamically

            // Departments
            var informatique = new Department() {name = "Informatique"};

            // Courses
            var programmation = new Course() {courseName = "Programmation 1"};
            var algorithmie = new Course() { courseName = "Algorithmie 1" };
            programmation.department = informatique;
            algorithmie.department = informatique;

            repository.addAll(programmation, algorithmie);

            // ScheduleBlocks
            var weekDays = (WeekDay[])Enum.GetValues(typeof(WeekDay));
            foreach (var weekDay in weekDays)
            {
                for (var i = ScheduleBlock.MINIMUM_TIME; i < ScheduleBlock.MAXIMUM_TIME; i++)
                {
                    var newScheduleBlock = new ScheduleBlock()
                    {
                        startTime = i,
                        weekDay = weekDay
                    };
                    repository.add(newScheduleBlock);
                }
            }

            // USERS
            var helped = new Helped
            {
                mail = "simonfrenette@mail.com",
                name = "Simon",
                lastName = "Frenette",
                password = "calinours",
                scheduleBlocks =  ScheduleUtilities.getScheduleBlocksFromJson("[{\"start\":\"11:00 am\",\"end\":\"01:00 pm\",\"day\":2},{\"start\":\"02:00 pm\",\"end\":\"05:00 pm\",\"day\":2},{\"start\":\"11:00 am\",\"end\":\"12:00 pm\",\"day\":4}]", repository)
            };

            repository.add(helped);

            var tutor = new Tutor
            {
                mail = "jeremiepoisson@mail.com",
                name = "Jeremie",
                lastName = "Poisson",
                password = "calinours",
                tutorAvailableForGroupSession = true,
                tutorAvailableForIndividualSession = true,
                scheduleBlocks = ScheduleUtilities.getScheduleBlocksFromJson("[{\"start\":\"11:00 am\",\"end\":\"01:00 pm\",\"day\":2},{\"start\":\"02:00 pm\",\"end\":\"05:00 pm\",\"day\":2},{\"start\":\"11:00 am\",\"end\":\"12:00 pm\",\"day\":4}]", repository)
            };

            repository.add(tutor);

            var manager = new Manager()
            {
                lastName = "Bertrand",
                name = "François",
                mail = "fbertrand@mail.com",
                password = "mypassword"
            };

            repository.add(manager);

            // HELPREQUESTS
            var demoIndividualSession1 = new IndividualSession()
            {
                date = new DateTime(2013, 10, 25, 12, 00, 00),
                helpedMessage = "Oui c'est vraiment très dur, je ne comprends rien hahahahahaha",
                tutorMessage = "Je suis le meilleur",
                place = "G-170"
            };

            var demoIndividualSession2 = new IndividualSession()
            {
                date = new DateTime(2013, 12, 25, 12, 00, 00),
                place = "G-169"
            };

            var demoIndividualSession3 = new IndividualSession()
            {
                date = new DateTime(2013, 10, 25, 12, 00, 00),
                place = "G-159"
            };

            // TO CONFIRM
            var demoHelpRequest2 = new HelpRequest()
            {
                tutor = tutor,
                helped = helped,
                course = programmation,
                comment = "Je ne comprends pas vraiment oh c'est dur dur dur",
                createdTime = DateTime.Now,
                helpedHasConfirmed = true,
                scheduleJson = "[{\"start\":\"08:00 am\",\"end\":\"09:00 am\",\"day\":1}]",
                misunderstoodNotions = "Toute wow incroyable",
                tutorHasConfirmed = false,
                individualSession = demoIndividualSession2
            };

            // TO CONFIRM 2
            var demoHelpRequest3 = new HelpRequest()
            {
                tutor = tutor,
                helped = helped,
                course = programmation,
                comment = "Je ne comprends pas vraiment oh c'est dur dur dur",
                createdTime = DateTime.Now,
                helpedHasConfirmed = false,
                scheduleJson = "[{\"start\":\"08:00 am\",\"end\":\"09:00 am\",\"day\":1}]",
                misunderstoodNotions = "Toute wow incroyable",
                tutorHasConfirmed = true,
                individualSession = demoIndividualSession2
            };

            // CONFIRMED
            var demoHelpRequest4 = new HelpRequest()
            {
                tutor = tutor,
                helped = helped,
                course = programmation,
                comment = "Je ne comprends pas vraiment oh c'est dur dur dur",
                createdTime = DateTime.Now,
                helpedHasConfirmed = true,
                scheduleJson = "[{\"start\":\"08:00 am\",\"end\":\"09:00 am\",\"day\":1}]",
                misunderstoodNotions = "Toute wow incroyable",
                tutorHasConfirmed = true,
                individualSession = demoIndividualSession2
            };

            // FINISHED
            var demoHelpRequest5 = new HelpRequest()
            {
                tutor = tutor,
                helped = helped,
                course = programmation,
                comment = "Je ne comprends pas vraiment oh c'est dur dur dur",
                createdTime = DateTime.Now,
                helpedHasConfirmed = true,
                scheduleJson = "[{\"start\":\"08:00 am\",\"end\":\"09:00 am\",\"day\":1}]",
                misunderstoodNotions = "Toute wow incroyable",
                tutorHasConfirmed = true,
                individualSession = demoIndividualSession3
            };

            // BILAN
            var demoHelpRequest6 = new HelpRequest()
            {
                tutor = tutor,
                helped = helped,
                course = programmation,
                comment = "Je ne comprends pas vraiment oh c'est dur dur dur",
                createdTime = DateTime.Now,
                helpedHasConfirmed = true,
                scheduleJson = "[{\"start\":\"08:00 am\",\"end\":\"09:00 am\",\"day\":1}]",
                misunderstoodNotions = "Toute wow incroyable",
                tutorHasConfirmed = true,
                individualSession = demoIndividualSession1
            };

            repository.addAll(demoHelpRequest2, demoHelpRequest3, demoHelpRequest4, demoHelpRequest5, demoHelpRequest6);


            // Users
            const string jsonScheduleBlocks = "[{\"start\":\"08:00 am\",\"end\":\"10:00 am\",\"day\":1},{\"start\":\"11:00 am\",\"end\":\"01:00 pm\",\"day\":2},{\"start\":\"02:00 pm\",\"end\":\"05:00 pm\",\"day\":2},{\"start\":\"11:00 am\",\"end\":\"12:00 pm\",\"day\":4}]";
            var helpedUser = new Helped()
            {
                id = 3,
                lastName = "Helped",
                mail = "helped@horsemail.com",
                name = "Kevin",
                password = "mypassword",
                scheduleBlocks = ScheduleUtilities.getScheduleBlocksFromJson(jsonScheduleBlocks, repository)
            };
            var helpedUser2 = new Helped()
            {
                id = 3,
                lastName = "Helped",
                mail = "helped2@horsemail.com",
                name = "Yvonne",
                password = "mypassword",
                scheduleBlocks = ScheduleUtilities.getScheduleBlocksFromJson(jsonScheduleBlocks, repository)
            };
            repository.add(helpedUser2);
            var tutorUser = new Tutor()
            {
                id = 2,
                lastName = "Tutor",
                mail = "tutor@horsemail.com",
                name = "Yvon",
                password = "mypassword",
                tutorAvailableForGroupSession = true,
                tutorAvailableForIndividualSession = true,
                scheduleBlocks = ScheduleUtilities.getScheduleBlocksFromJson(jsonScheduleBlocks, repository)
            };
            var tutorUser2 = new Tutor()
            {
                id = 2,
                lastName = "Tutor",
                mail = "tutor@horsemail.com",
                name = "Maxime",
                password = "mypassword",
                tutorAvailableForGroupSession = true,
                tutorAvailableForIndividualSession = false,
                scheduleBlocks = ScheduleUtilities.getScheduleBlocksFromJson(jsonScheduleBlocks, repository)
            };
            repository.add(tutorUser2);

            var tutorUserIndividual = new Tutor()
            {
                id = 3,
                name = "Tutor",
                lastName = "Individual",
                mail = "individual@tutor.com",
                password = "mypassword",
                tutorAvailableForGroupSession = false,
                tutorAvailableForIndividualSession = true
            };
            tutorUserIndividual.scheduleBlocks.Add(new ScheduleBlock() { startTime = 10, weekDay = WeekDay.MONDAY });
            tutorUserIndividual.scheduleBlocks.Add(new ScheduleBlock() { startTime = 11, weekDay = WeekDay.MONDAY });
            tutorUserIndividual.coursesSkills.Add(programmation);
            repository.add(tutorUserIndividual);

            var managerUser = new Manager()
            {
                lastName = "Bertrand",
                name = "François",
                mail = "fbertrand@mail.com",
                password = "mypassword"
            };
            repository.add(managerUser);

            // HelpRequests
            var helpRequest1 = new HelpRequest()
            {
                id = 1,
                course = programmation,
                helped = helpedUser,
                tutor = tutorUser,
                misunderstoodNotions = Resources.Resources.TestDataHelpRequestMisunderstoodNotions,
                scheduleJson = Resources.Resources.TestDataHelpRequestScheduleJson,
                comment = Resources.Resources.TestDataHelpRequestComment
            };
            var helpRequest2 = new HelpRequest()
            {
                id = 2,
                course = programmation,
                helped = helpedUser,
                tutor = tutorUser,
                misunderstoodNotions = Resources.Resources.TestDataHelpRequestMisunderstoodNotions2,
                scheduleJson = Resources.Resources.TestDataHelpRequestScheduleJson2,
                comment = Resources.Resources.TestDataHelpRequestComment2
            };
            var helpRequest3 = new HelpRequest()
            {
                id = 3,
                course = programmation,
                helped = helpedUser,
                tutor = null,
                misunderstoodNotions = Resources.Resources.TestDataHelpRequestMisunderstoodNotions2,
                scheduleJson = Resources.Resources.TestDataHelpRequestScheduleJson2,
                comment = Resources.Resources.TestDataHelpRequestComment2
            };
            var helpRequestFinished = new HelpRequest()
            {
                id = 4,
                course = programmation,
                helped = helpedUser,
                tutor = tutorUser,
                helpedHasConfirmed = true,
                tutorHasConfirmed = true,
                currentState = HelpRequestState.FINISHED,
                misunderstoodNotions = Resources.Resources.TestDataHelpRequestMisunderstoodNotions2,
                scheduleJson = Resources.Resources.TestDataHelpRequestScheduleJson2,
                comment = Resources.Resources.TestDataHelpRequestComment2
            };
            var helpRequestFinished2 = new HelpRequest()
            {
                id = 4,
                course = programmation,
                helped = helpedUser,
                tutor = tutorUser,
                helpedHasConfirmed = true,
                tutorHasConfirmed = true,
                currentState = HelpRequestState.FINISHED,
                misunderstoodNotions = Resources.Resources.TestDataHelpRequestMisunderstoodNotions2,
                scheduleJson = Resources.Resources.TestDataHelpRequestScheduleJson2,
                comment = Resources.Resources.TestDataHelpRequestComment2
            };
            var helpRequestFinished3 = new HelpRequest()
            {
                id = 4,
                course = programmation,
                helped = helpedUser,
                tutor = tutorUser,
                helpedHasConfirmed = true,
                tutorHasConfirmed = true,
                currentState = HelpRequestState.FINISHED,
                misunderstoodNotions = Resources.Resources.TestDataHelpRequestMisunderstoodNotions2,
                scheduleJson = Resources.Resources.TestDataHelpRequestScheduleJson2,
                comment = Resources.Resources.TestDataHelpRequestComment2
            };
            repository.addAll(helpRequest1, helpRequest2, helpRequest3, helpRequestFinished, helpRequestFinished2, helpRequestFinished3);

            // IndividualSessions

            var individualSession1 = new IndividualSession()
            {
                date = new DateTime(2013, 10, 23, 10, 00, 00),
                helpRequest = helpRequest1,
                place = "Hell",
                tutorMessage = null,
                helpedMessage = null
            };
            var individualSession2 = new IndividualSession()
            {
                date = new DateTime(2013, 10, 23, 12, 00, 00),
                helpRequest = helpRequest2,
                place = "Heaven",
                tutorMessage = null,
                helpedMessage = null
            };
            var individualSession3 = new IndividualSession()
            {
                date = new DateTime(2013, 10, 23, 14, 00, 00),
                helpRequest = helpRequestFinished,
                place = "Earth",
                tutorMessage = Resources.Resources.TestDataIndividualSessionTutorMessage,
                helpedMessage = Resources.Resources.TestDataIndividualSessionHelpedMessage
            };
            var individualSessionEmptyMessages = new IndividualSession()
            {
                date = new DateTime(2013, 10, 24, 8, 00, 00),
                helpRequest = helpRequestFinished2,
                place = "Ocean",
                tutorMessage = null,
                helpedMessage = null
            };
            var individualSessionEmptyMessages2 = new IndividualSession()
            {
                date = new DateTime(2013, 10, 24, 10, 00, 00),
                helpRequest = helpRequestFinished3,
                place = "Space",
                tutorMessage = null,
                helpedMessage = null
            };

            repository.addAll(individualSession1, individualSession2, individualSession3,
                individualSessionEmptyMessages, individualSessionEmptyMessages2);
        }


    }

    
}
