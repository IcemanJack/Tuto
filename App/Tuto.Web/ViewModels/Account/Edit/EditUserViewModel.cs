using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Tuto.DataLayer.ModelUtilities;
using Tuto.DataLayer.Repository;

namespace Tuto.Web.ViewModels.Account.Edit
{
    public abstract class EditUserViewModel
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayAccountEditCurrentEmail")]
        public string currentEmail { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayAccountEditCurrentPassword")]
        public string currentPassword { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayAccountEditNewName")]
        public string newName { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayAccountEditNewLastName")]
        public string newLastName { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayAccountEditNewPassword")]
        public string newPassword { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayAccountNewPasswordConfirm")]
        public string confirmNewPassword { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayAccountEditNewEmail")]
        public string newEmail { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayAccountEditNewEmailConfirm")]
        public string confirmNewEmail { get; set; }

        public virtual bool isManager()
        {
            return false;
        }

        public virtual bool isTutor()
        {
            return false;
        }

        public virtual bool isHelped()
        {
            return false;
        }
    }

    class EditUserValidator : AbstractValidator<EditUserViewModel>
    {
        public EditUserValidator()
        {
            // Password
            this.RuleFor(x => x.currentPassword).NotEmpty().WithMessage(Resources.Resources.ErrorUserPasswordRequired);

            // NewMail
            this.RuleFor(x => x.newEmail).EmailAddress().WithMessage(Resources.Resources.ErrorUserMailValid);
            this.RuleFor(x => x.newEmail).Must(this.beUniqueMail).WithMessage(Resources.Resources.ErrorUserMailUnique);

            // ConfirmNewMail
            this.RuleFor(x => x.confirmNewEmail).Equal(x => x.newEmail).WithMessage(Resources.Resources.ErrorUserConfirmEmail);

            // NewPassword
            this.RuleFor(x => x.newPassword).Length(6, 20).WithMessage(Resources.Resources.ErrorUserPasswordMinimum);

            // ConfirmPassword
            this.RuleFor(x => x.confirmNewPassword).Equal(x => x.newPassword).WithMessage(Resources.Resources.ErrorUserConfirmEmail);
        }

        //function to unique
        private bool beUniqueMail(string newEmail)
        {
            var repository = new EntityRepository();

            return AccountLoginUtilities.getUserFromLogin(repository, newEmail) == null;
        }
    }
}