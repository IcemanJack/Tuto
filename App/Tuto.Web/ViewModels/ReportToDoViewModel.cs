using FluentValidation;
using FluentValidation.Attributes;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Repository;

namespace Tuto.Web.ViewModels
{
    [Validator(typeof(TutorReportToDoValidator))]
    public class ReportToDoViewModel
    {
        public int helpRequestId { get; set; }
        public string message { get; set; }
    }

    public class TutorReportToDoValidator : AbstractValidator<ReportToDoViewModel>
    {
        public TutorReportToDoValidator()
        {
            // helpRequestId
            this.RuleFor(x => x.helpRequestId)
                .NotEmpty()
                .Must(helpRequestExists);

            // Message
            this.RuleFor(x => x.message)
                .NotEmpty()
                .WithMessage(Resources.Resources.ErrorReportMessageRequired);
            this.RuleFor(x => x.message)
                .Length(0,200)
                .WithMessage(Resources.Resources.ErrorReportMessageMaxLenght);
        }

        private static bool helpRequestExists(int helpRequestId)
        {
            var repository = new EntityRepository();
            if (repository.single<IndividualSession>(x => x.helpRequestId == helpRequestId) != null)
            {
                return true;
            }
            return false;
        }
    }
}