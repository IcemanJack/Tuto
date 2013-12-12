using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.ModelUtilities;
using Tuto.DataLayer.Repository;

namespace Tuto.Web.ViewModels.Account.Register
{
    [Validator(typeof (TutorRegisterValidator))]
    public class TutorRegisterViewModel
    {
        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayUserName")]
        public string name { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayUserLastName")]
        public string lastName { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayUserMail")]
        public string mail { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayUserPassword")]
        public string password { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayUserConfirmPassword")]
        public string confirmPassword { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayTutorAvailableForGroupSession")]
        public Boolean tutorAvailableForGroupSession { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayTutorAvailableForIndividualSession")]
        public Boolean tutorAvailableForIndividualSession { get; set; }

        public string coursesSkillsJson { get; set; }
        public string scheduleBlocksJson { get; set; }

        public List<Course> getCoursesSkills(IEntityRepository repository)
        {
            var result = new List<Course>();

            dynamic coursesSkills = JsonConvert.DeserializeObject(this.coursesSkillsJson);
            for (var i = 0; i < coursesSkills.Count; i++)
            {
                var currentCourseSkill = coursesSkills[i];
                string departmentName = currentCourseSkill.department;
                string courseName = currentCourseSkill.course;

                var course = repository.single<Course>(x => (x.courseName == courseName && x.department.name == departmentName));

                if (course != null)
                {
                    result.Add(course);  
                }
            }

            return result;
        }
    }

    public class TutorRegisterValidator : AbstractValidator<TutorRegisterViewModel>
    {
        public TutorRegisterValidator()
        {
            // Name
            this.RuleFor(x => x.name).NotEmpty().WithMessage(Resources.Resources.ErrorUserNameRequired);

            // LastName
            this.RuleFor(x => x.lastName).NotEmpty().WithMessage(Resources.Resources.ErrorUserLastNameRequired);

            // Mail
            this.RuleFor(x => x.mail).NotEmpty().WithMessage(Resources.Resources.ErrorUserMailRequired);
            this.RuleFor(x => x.mail).EmailAddress().WithMessage(Resources.Resources.ErrorUserMailValid);
            this.RuleFor(x => x.mail).Must(beUniqueMail).WithMessage(Resources.Resources.ErrorUserMailUnique);

            // Password
            this.RuleFor(x => x.password).NotEmpty().WithMessage(Resources.Resources.ErrorUserPasswordRequired);
            this.RuleFor(x => x.password).Length(6, 20).WithMessage(Resources.Resources.ErrorUserPasswordMinimum);

            // ConfirmPassword
            this.RuleFor(x => x.confirmPassword)
                .Equal(x => x.password)
                .WithMessage(Resources.Resources.ErrorUserConfirmPassword);

            // TutorAvailableForGroupSession
            this.RuleFor(x => x.tutorAvailableForGroupSession)
                .Equal(true)
                .When(x => x.tutorAvailableForIndividualSession == false)
                .WithMessage(Resources.Resources.ErrorTutorAvailableAtLeastOne);

            // tutorAvailableForIndividualSession
            this.RuleFor(x => x.tutorAvailableForIndividualSession)
                .Equal(true)
                .When(x => x.tutorAvailableForGroupSession == false)
                .WithMessage(Resources.Resources.ErrorTutorAvailableAtLeastOne);

            // coursesSkills
            this.RuleFor(x => x.coursesSkillsJson).NotEmpty().Must(haveAtLeastOneCourseSkill).WithMessage(Resources.Resources.ErrorTutorCourseSkillsRequired);

            // scheduleBlocks
            this.RuleFor(x => x.scheduleBlocksJson).NotEmpty().Must(haveAtLeastOneScheduleBlock).WithMessage(Resources.Resources.ErrorUserScheduleBlockRequired);
        }

        private static bool beUniqueMail(string mail)
        {
            var repository = new EntityRepository();

            return AccountLoginUtilities.getUserFromLogin(repository, mail) == null;
        }

        private static bool haveAtLeastOneCourseSkill(string coursesSkillsJson)
        {
            dynamic coursesSkillsArray = JsonConvert.DeserializeObject(coursesSkillsJson);
            return (coursesSkillsArray.Count >= 1);
        }

        private static bool haveAtLeastOneScheduleBlock(string scheduleBlockJson)
        {
            dynamic scheduleBlocksArray = JsonConvert.DeserializeObject(scheduleBlockJson);
            return (scheduleBlocksArray.Count >= 1);
        }

    }
}