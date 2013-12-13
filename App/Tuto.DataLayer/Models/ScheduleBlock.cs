using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Attributes;
using Tuto.DataLayer.Models.Users;

namespace Tuto.DataLayer.Models
{
    public enum WeekDay : int
    {
        MONDAY = 1,
        TUESDAY = 2,
        WEDNESDAY = 3,
        THURSDAY = 4,
        FRIDAY = 5
    }

    public static class WeekDayExtension
    {
        public static string toString(this WeekDay weekDay)
        {
            switch (weekDay)
            {
                case WeekDay.MONDAY:
                    return Resources.Resources.Moday;
                case WeekDay.TUESDAY:
                    return Resources.Resources.Tuesday;
                case  WeekDay.WEDNESDAY:
                    return Resources.Resources.Wednesday;
                case WeekDay.THURSDAY:
                    return Resources.Resources.Thursday;
                case WeekDay.FRIDAY:
                    return Resources.Resources.Friday;
                default:
                    return Resources.Resources.Moday;
            }
        }
    }

    [Validator(typeof(ScheduleBlockValidator))]
    public class ScheduleBlock : Entity
    {
        public WeekDay weekDay { get; set; }
        public int startTime { get; set; } // 8 to 18 (for 8h am to 6h pm)

        public virtual ICollection<Tutor> tutors { get; set; }
        public virtual ICollection<Helped> helpeds { get; set; }

        public override bool Equals(object other)
        {
            var otherBlock = other as ScheduleBlock;
            if (otherBlock == null)
            {
                return false;
            }

            return ((this.weekDay == otherBlock.weekDay) && (this.startTime == otherBlock.startTime));
        }

        public override int GetHashCode()
        {
            var dayHash = this.weekDay.GetHashCode();
            var timeHash = this.startTime.GetHashCode();

            return (dayHash + timeHash).GetHashCode();
        }
    }

    public class ScheduleBlockValidator : AbstractValidator<ScheduleBlock>
    {
        public ScheduleBlockValidator()
        {
            // StartTime
            this.RuleFor(x => x.startTime).GreaterThan(DataLayerConfig.SCHEDULE_MINIMUM_TIME - 1).LessThan(DataLayerConfig.SCHEDULE_MAXIMUM_TIME + 1).WithMessage(Resources.Resources.ErrorScheduleBlockLength);
        }

    }
}