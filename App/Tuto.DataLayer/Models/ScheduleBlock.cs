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
                    return "Lundi";
                case WeekDay.TUESDAY:
                    return "Mardi";
                case  WeekDay.WEDNESDAY:
                    return "Mercredi";
                case WeekDay.THURSDAY:
                    return "Jeudi";
                case WeekDay.FRIDAY:
                    return "Vendredi";
                default:
                    return "Lundi";
            }
        }
    }

    [Validator(typeof(ScheduleBlockValidator))]
    public class ScheduleBlock : Entity
    {
        public const int MINIMUM_TIME = 8;
        public const int MAXIMUM_TIME = 18;
        
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
            this.RuleFor(x => x.startTime).GreaterThan(ScheduleBlock.MINIMUM_TIME - 1).LessThan(ScheduleBlock.MAXIMUM_TIME + 1).WithMessage(Resources.Resources.ErrorScheduleBlockLength);
        }

    }
}