using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using FluentValidation.Attributes;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Notifications;

namespace Tuto.DataLayer.Models.Users
{
    [Validator(typeof(TutorValidator))]
    public class Tutor : Student
    {
        public Tutor()
        {
            this.scheduleBlocks = new HashSet<ScheduleBlock>();
            this.coursesSkills = new HashSet<Course>();
            this.groupSessions = new HashSet<AssignedGroupSession>();
            this.helpRequests = new HashSet<HelpRequest>();
            this.individualNotifications = new HashSet<TutorBaseNotification>();
        }

        public Boolean tutorAvailableForGroupSession { get; set; }
        public Boolean tutorAvailableForIndividualSession { get; set; }

        public TutorState tutorState { get; set; }
        public virtual ICollection<Course> coursesSkills { get; set; }
        public virtual ICollection<AssignedGroupSession> groupSessions { get; set; }
        public virtual ICollection<HelpRequest> helpRequests { get; set; }
        /*
        * TODO : Explain bug with entity (SF)
        */
        public virtual ICollection<ScheduleBlock> scheduleBlocks { get; set; }
        public virtual ICollection<TutorBaseNotification> individualNotifications { get; set; }

        public override ICollection<BaseNotification> getNotifications()
        {
            ICollection<BaseNotification> everyNotifications = new HashSet<BaseNotification>();

            this.individualNotifications.ToList().ForEach(everyNotifications.Add);
            this.sharedNotifications.ToList().ForEach(everyNotifications.Add);

            return everyNotifications;
        }

        public override bool isTutor()
        {
            return true;
        }

        protected virtual bool isAvailableAt(List<ScheduleBlock> toBeVerifiedBlocks)
        {
            if (this.scheduleBlocks.Count == 0)
            {
                return false;
            }

            IEnumerator<ScheduleBlock> verificationScheduleBlocksEnum = toBeVerifiedBlocks.GetEnumerator();

            bool blockIsMissing = false;
            while (verificationScheduleBlocksEnum.MoveNext() && !blockIsMissing)
            {
                blockIsMissing = !this.scheduleBlocks.Contains(verificationScheduleBlocksEnum.Current);
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

            return this.isAvailableAt(matchingBlocks);
        }

        public virtual int calculateMonthlyWorkedHours()
        {
            int totalWorkedHours = 0;
            List<IndividualSession> finishedIndividualSessions = this.helpRequests.Where(h => h.currentState == HelpRequestState.FINISHED).ToList().Select(t => t.individualSession).ToList();

            return finishedIndividualSessions.Count * 2; // 2 hours per individual sessions
        }

        public string getCoursesSkillsString()
        {
            if (this.coursesSkills.Count == 0)
            {
                return "";
            }

            var resultStr = new StringBuilder();
            foreach (Course course in this.coursesSkills)
            {
                resultStr.Append(course.courseName);
                resultStr.Append(", ");
            }

            return resultStr.ToString().Substring(0, resultStr.Length - 2);
        }
    }

    public class TutorValidator : AbstractValidator<Tutor>
    {
        public TutorValidator()
        {
            // TutorAvailableForGroupSession
            this.RuleFor(x => x.tutorAvailableForGroupSession)
                .Equal(true)
                .When(x => x.tutorAvailableForIndividualSession == false)
                .WithMessage(Resources.Resources.ErrorTutorAvailableAtLeastOne);

            // TutorAvailableForIndividualSession
            this.RuleFor(x => x.tutorAvailableForIndividualSession)
                .Equal(true)
                .When(x => x.tutorAvailableForGroupSession == false)
                .WithMessage(Resources.Resources.ErrorTutorAvailableAtLeastOne);

            // coursesSkills
            this.RuleFor(x => x.coursesSkills).NotEmpty().WithMessage(Resources.Resources.ErrorTutorCourseSkillsRequired);
        }

    }
}
