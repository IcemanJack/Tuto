using System;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Attributes;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Users;

namespace Tuto.DataLayer.Models
{
    [Validator(typeof(HelpRequestValidator))]
    public class HelpRequest : Entity
    {
        // filter used to retreive help request within different state
        public static readonly Expression<Func<HelpRequest, bool>> TO_BE_CONFIRMED_HELPREQUESTS_FILTER = (t => t.individualSession == null);
        public static readonly Expression<Func<HelpRequest, bool>> FINISHED_HELPREQUESTS_FILTER = (t => t.individualSession != null && t.individualSession.date.CompareTo(DateTime.Now) < 0); 
        public static readonly Expression<Func<HelpRequest, bool>> CONFIRMED_HELPREQUESTS_FILTER = (t => t.individualSession != null && t.individualSession.date.CompareTo(DateTime.Now) > 0);

        public HelpRequest()
        {
            this.createdTime = DateTime.Now;
            this.currentState = this.getState();
        }

        public DateTime createdTime { get; set; }
        public virtual HelpRequestState currentState { get; set; }
        public bool helpedHasConfirmed { get; set; }
        public bool tutorHasConfirmed { get; set; }

        public string misunderstoodNotions { get; set; }
        public string scheduleJson { get; set; }
        public string comment { get; set; }
        
        public int courseId { get; set; }
        public Nullable<int> tutorId { get; set; }  // needed to make a foreign key nullable
        public int helpedId { get; set; }

        public virtual Course course { get; set; }  
        public virtual Helped helped { get; set; }
        public virtual Tutor tutor { get; set; } 

        public virtual IndividualSession individualSession { get; set; } // will be null if any session has been assigned

        public HelpRequestState getState()
        {
            if (this.tutorId == null) return HelpRequestState.NOT_ASSIGNED;
            if (!(this.tutorHasConfirmed && this.helpedHasConfirmed)) return HelpRequestState.TO_BE_CONFIRMED;
            if (this.individualSession.date.CompareTo(DateTime.Now) < 0) return HelpRequestState.FINISHED;
            return HelpRequestState.CONFIRMED;
        }

        public bool hasTutorAssigned()
        {
            return this.individualSession != null;
        }

        public bool hasHelpedUserAssigned()
        {
            return this.helped != null;
        }
    }

    class HelpRequestValidator : AbstractValidator<HelpRequest>
    {
        public HelpRequestValidator()
        {
            // MisunderstoodNotions
            this.RuleFor(x => x.misunderstoodNotions).NotEmpty().WithMessage(Resources.Resources.ErrorHelpRequestMisunderstoodNotionsRequired);

            // ScheduleJson
            this.RuleFor(x => x.scheduleJson).NotEmpty().WithMessage(Resources.Resources.ErrorHelpRequestSchedule);
        }
    }
}