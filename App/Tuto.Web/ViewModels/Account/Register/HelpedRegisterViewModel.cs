using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Tuto.DataLayer.ModelUtilities;
using Tuto.DataLayer.Repository;

namespace Tuto.Web.ViewModels.Account.Register
{
    [Validator(typeof(HelpedRegisterValidator))]
    public class HelpedRegisterViewModel
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

        public string scheduleBlocksJson { get; set; }
    }

    public class HelpedRegisterValidator : AbstractValidator<HelpedRegisterViewModel>
    {
        public HelpedRegisterValidator()
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

            // scheduleBlocks
            this.RuleFor(x => x.scheduleBlocksJson).NotEmpty().Must(haveAtLeastOneScheduleBlock).WithMessage(Resources.Resources.ErrorUserScheduleBlockRequired);
        }

        private static bool beUniqueMail(string mail)
        {
            var repository = new EntityRepository();
            return AccountLoginUtilities.getUserFromLogin(repository, mail) == null;
        }

        private static bool haveAtLeastOneScheduleBlock(string scheduleBlockJson)
        {
            dynamic scheduleBlocksArray = JsonConvert.DeserializeObject(scheduleBlockJson);
            return (scheduleBlocksArray.Count >= 1);
        }

    }
}