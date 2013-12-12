using System;
using FluentValidation;
using FluentValidation.Attributes;

namespace Tuto.DataLayer.Models
{
    [Validator(typeof(SessionValidator))]
    public class IndividualSession : Entity
    {
        public DateTime date { get; set;}
        public string place { get; set; }

        public string tutorMessage { get; set; }
        public string helpedMessage { get; set; }

        public int helpRequestId { get; set; }
        public virtual HelpRequest helpRequest { get; set; }

    }

    class SessionValidator : AbstractValidator<IndividualSession>
    {
        
        public SessionValidator()
        {
            //DATE
            this.RuleFor(x => x.date).NotEmpty().WithMessage(Resources.Resources.ErrorSessionDateRequired);
            this.RuleFor(x => x.date).Must(this.beValidDate).WithMessage(Resources.Resources.ErrorSessionDateFormat);

            //PLACE
            this.RuleFor(x => x.place).NotEmpty().WithMessage(Resources.Resources.ErrorSessionPlaceRequired);
        }

        private bool beValidDate(DateTime date)
        {
            return date != default(DateTime);
        }
    }
}
