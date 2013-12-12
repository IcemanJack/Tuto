using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Tuto.DataLayer.ModelUtilities;
using Tuto.DataLayer.Repository;

namespace Tuto.Web.ViewModels.Account
{
    public class UserLoginViewModel
    {
        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayUserMail")]    
        public string mail { get; set; }
        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayUserPassword")] 
        public string password { get; set; }
    }

    class UserLoginValidator : AbstractValidator<UserLoginViewModel>
    {
        public UserLoginValidator()
        {
            // MAIL
            this.RuleFor(x => x.mail).NotEmpty().WithMessage(Resources.Resources.ErrorUserMailRequired);
            this.RuleFor(x => x.mail).EmailAddress().WithMessage(Resources.Resources.ErrorUserMailValid);

            // PASSWORD
            this.RuleFor(x => x.password).NotEmpty().WithMessage(Resources.Resources.ErrorUserPasswordRequired);
            this.RuleFor(x => x.password).Must(beValidUser).WithMessage(Resources.Resources.ErrorUserLoginWrongCredentials);

        }

        private static bool beValidUser(UserLoginViewModel user, string password)
        {
            var repository = new EntityRepository();

            var existingUser = AccountLoginUtilities.getUserFromLogin(repository, user.mail);
            return existingUser != null && existingUser.password == user.password;
        }

    }
}