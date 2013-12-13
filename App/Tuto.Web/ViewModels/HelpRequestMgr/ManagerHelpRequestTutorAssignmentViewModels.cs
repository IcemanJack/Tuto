using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Attributes;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.Repository;

namespace Tuto.Web.ViewModels.HelpRequestMgr
{
    public class ManagerHelpRequestTutorAssignmentViewModels
    {
        public class AssignHelpRequestValidator : AbstractValidator<AssignHelpRequestViewModel>
        {
            public AssignHelpRequestValidator()
            {
                this.RuleFor(x => x.weekPickStart).NotEmpty().WithMessage(Resources.Resources.HelpRequestMgrAssignWeekPickInvalidFormat).Must(this.beAValidDate).WithMessage(Resources.Resources.HelpRequestMgrAssignWeekPickInvalidFormat);
                this.RuleFor(x => x.weekPickEnd).NotEmpty().WithMessage(Resources.Resources.HelpRequestMgrAssignWeekPickInvalidFormat).Must(this.beAValidDate).WithMessage(Resources.Resources.HelpRequestMgrAssignWeekPickInvalidFormat);

                this.RuleFor(x => x.localName).NotEmpty().WithMessage(Resources.Resources.HelpRequestMgrAssignEmptyLocalName);

                DateTime dummyVar; // unused variable used to parse date into... (since the tryparse function requires a variable to be passed in reference
                this.RuleFor(x => x.getSessionStartDateTime()).Must(this.beInTheFuture).When(x => DateTime.TryParse(x.weekPickStart, out dummyVar) && DateTime.TryParse(x.weekPickEnd, out dummyVar), ApplyConditionTo.CurrentValidator).WithName("SessionDateValidationError");

                this.RuleFor(x => x.assignTutorId).Must(this.beAValidTutorId).WithMessage(Resources.Resources.HelpRequestMgrAssignInvalidTutorId);
                this.RuleFor(x => x.helpRequestId).Must(this.beAValidHelpRequestId).WithMessage(Resources.Resources.HelpRequestMgrAssignInvalidHelpRequestId);
                this.RuleFor(x => x.assignStartHour).InclusiveBetween(8, 16).WithMessage(Resources.Resources.HelpRequestMgrAssignInvalidStartHour);
                this.RuleFor(x => x.assignEndHour).InclusiveBetween(10, 18).WithMessage(Resources.Resources.HelpRequestMgrAssignInvalidEndHour);
            }

            private bool beAValidDate(String date)
            {
                return date != null && Regex.IsMatch(date, "^([0-9]{2}/[0-9]{2}/[0-9]{4})$");
            }

            private bool beAValidTutorId(int id)
            {
                var repo = new EntityRepository();

                var tutor = repo.getById<Tutor>(id);

                return tutor != null && tutor.tutorAvailableForIndividualSession;
            }

            private bool beAValidHelpRequestId(int id)
            {
                var repo = new EntityRepository();

                var hr = repo.getById<DataLayer.Models.HelpRequest>(id);

                return hr != null && hr.getState() == HelpRequestState.NOT_ASSIGNED;
            }

            private bool beInTheFuture(DateTime sessionDate)
            {
                return sessionDate > DateTime.Now;
            }
        }

        [Validator(typeof(AssignHelpRequestValidator))]
        public class AssignHelpRequestViewModel
        {
            public static IEnumerable<SelectListItem> dayOfWeekHtmSelectListItems = new List<SelectListItem>
            {
                new SelectListItem() { Text = Resources.Resources.Moday, Value = ((int)WeekDay.MONDAY).ToString() },
                new SelectListItem() { Text = Resources.Resources.Tuesday, Value = ((int)WeekDay.TUESDAY).ToString() },
                new SelectListItem() { Text = Resources.Resources.Wednesday, Value = ((int)WeekDay.WEDNESDAY).ToString() },
                new SelectListItem() { Text = Resources.Resources.Thursday, Value = ((int)WeekDay.THURSDAY).ToString() },
                new SelectListItem() { Text = Resources.Resources.Friday, Value = ((int)WeekDay.FRIDAY).ToString() }
            };

            public static IEnumerable<SelectListItem> assignSessionStartHoursList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "8h", Value = "8" },
                new SelectListItem() { Text = "9h", Value = "9" },
                new SelectListItem() { Text = "10h", Value = "10" },
                new SelectListItem() { Text = "11h", Value = "11" },
                new SelectListItem() { Text = "12h", Value = "12" },
                new SelectListItem() { Text = "13h", Value = "13" },
                new SelectListItem() { Text = "14h", Value = "14" },
                new SelectListItem() { Text = "15h", Value = "15" },
                new SelectListItem() { Text = "16h", Value = "16" }
            };

            public static IEnumerable<SelectListItem> assignSessionEndHoursList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "10h", Value = "10" },
                new SelectListItem() { Text = "11h", Value = "11" },
                new SelectListItem() { Text = "12h", Value = "12" },
                new SelectListItem() { Text = "13h", Value = "13" },
                new SelectListItem() { Text = "14h", Value = "14" },
                new SelectListItem() { Text = "15h", Value = "15" },
                new SelectListItem() { Text = "16h", Value = "16" },
                new SelectListItem() { Text = "15h", Value = "17" },
                new SelectListItem() { Text = "16h", Value = "18" }
            };

            public int helpRequestId { get; set; }
            public string helpedFirstName { get; set; }
            public string helpedLastName { get; set; }
            public DateTime helpRequestCreatedTime { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "HelpRequestMgrAssignCourseName")]
            public string helpRequestCourseName { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "HelpRequestMgrAssignMisunderstoodNotions")]
            public string helpRequestMisunderstoodNotions { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "HelpRequestMgrAssignComments")]
            public string helpRequestComment { get; set; }

            public HelpRequestState helpRequestState { get; set; }
            public string helpRequestScheduleJson { get; set; }

            public List<TutorListEntryViewModel> availableTutors { get; set; }

            //
            // properties mainly used on HTTP Post
            //

            public string weekPickStart { get; set; }
            public string weekPickEnd { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "HelpRequestMgrAssignLocalName")]
            public string localName { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "HelpRequestMgrAssignManualAssignedTutor")]
            public int assignTutorId { get; set; }

            public WeekDay assignWeekDay { get; set; }
            public int assignStartHour { get; set; }
            public int assignEndHour { get; set; }

            public DateTime getSessionStartDateTime()
            {
                // calculate the first date of the week
                DateTime weekPickStartDate = DateTime.Parse(this.weekPickStart);
                DateTime weekFirstDayDate = weekPickStartDate.Subtract(TimeSpan.FromDays((double)weekPickStartDate.DayOfWeek));

                return weekFirstDayDate.AddDays((int)this.assignWeekDay).AddHours(this.assignStartHour);
            }

            public class TutorListEntryViewModel
            {
                public int id { get; set; }
                public string firstName { get; set; }
                public string lastName { get; set; }

                [Display(ResourceType = typeof(Resources.Resources), Name = "HelpRequestMgrAssignMonthlyHours")]
                public int monthlyWorkingHours { get; set; }

                public string skilledCourses { get; set; }
            }
        }

        public class GetTutorAvailableForGivenBlockViewModel
        {
            public int helpRequestId { get; set; }
            public WeekDay dayOfWeek { get; set; }
            public int startHour { get; set; }
            public int endHour { get; set; }
        }

        public class TutorAvailableForGivenBlockViewModel
        {
            public int tutorUserId { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public int monthlyWorkingHours { get; set; }
        }
    }
}