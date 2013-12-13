using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.ModelUtilities;
using Tuto.DataLayer.Repository;

namespace Tuto.Web.ViewModels.HelpRequest
{
    [Validator(typeof (HelpRequestValidator))]
    public class HelpRequestAddViewModel
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayHelpRequestCourse")]
        public int courseId { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayHelpRequestMisunderstoodNotions")]
        public string misunderstoodNotions { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayHelpRequestComments")]
        public string comment { get; set; }

        public string scheduleJson { get; set; }
        public Course[] availableCourseList { get; set; }
    }

    class HelpRequestValidator : AbstractValidator<HelpRequestAddViewModel>
    {
        public HelpRequestValidator()
        {
            // Course
            this.RuleFor(x => x.courseId).NotEmpty().WithMessage(Resources.Resources.ErrorHelpRequestCourseRequired);

            // MisunderstoodNotions
            this.RuleFor(x => x.misunderstoodNotions).NotEmpty().WithMessage(Resources.Resources.ErrorHelpRequestMisunderstoodNotionsRequired);

            // Schedule
            this.RuleFor(x => x.scheduleJson).Must(beValidScheduleJson).WithMessage(Resources.Resources.ErrorHelpRequestInvalidFormat);
            this.RuleFor(x => x.scheduleJson).Must(notBeEmpty).WithMessage(Resources.Resources.ErrorHelpRequestSchedule);
            this.RuleFor(x => x.scheduleJson).NotEmpty().WithMessage(Resources.Resources.ErrorHelpRequestSchedule);
        }

        private static bool beValidScheduleJson(HelpRequestAddViewModel viewModel, string json)
        {
            return ScheduleUtilities.validateScheduleJsonString(json);
        }

        private static bool notBeEmpty(HelpRequestAddViewModel viewModel, string json)
        {
            // decode json
            var repository = new EntityRepository();
            return ScheduleUtilities.getScheduleBlocksFromJson(json, repository).Count > 0;
        }

    }
}