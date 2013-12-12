using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.ModelUtilities;
using Tuto.DataLayer.Repository;
using Tuto.Web.ViewModels.Account.Edit;

namespace Tuto.Web.Controllers.Utilities
{
    public class AccountEditUtilities
    {
        // You have to update only those that are different and not null from the current online user
        public static T makeNewOnlineUserFromViewModel<T>(T user, EditUserViewModel editUserViewModel, IEntityRepository repository) where T : User
        {
            if ((editUserViewModel.newName != null) && !editUserViewModel.newName.Equals(user.name))
            {
                user.name = editUserViewModel.newName;
            }
            if ((editUserViewModel.newLastName != null) && !editUserViewModel.newLastName.Equals(user.lastName))
            {
                user.lastName = editUserViewModel.newLastName;
            }
            if ((editUserViewModel.newPassword != null) && !editUserViewModel.newPassword.Equals(user.password))
            {
                user.password = editUserViewModel.newPassword;
            }
            if ((editUserViewModel.newEmail != null) && !editUserViewModel.newEmail.Equals(user.mail))
            {
                user.mail = editUserViewModel.newEmail;
            }

            var student = user as Student;
            if (student != null)
            {
                var studentViewModel = editUserViewModel as EditStudentViewModel;
                if ((studentViewModel.jsonSchedule != null))
                {
                    var viewModelScheduleBlocks = ScheduleUtilities.getScheduleBlocksFromJson(studentViewModel.jsonSchedule, repository);

                    var tutor = user as Tutor;
                    if (tutor != null)
                    {
                        if (!viewModelScheduleBlocks.Equals(tutor.scheduleBlocks))
                        {
                            tutor.scheduleBlocks = viewModelScheduleBlocks;
                        }
                    }
                    else
                    {
                        var helped = user as Helped;
                        if (!viewModelScheduleBlocks.Equals(helped.scheduleBlocks))
                        {
                            helped.scheduleBlocks = viewModelScheduleBlocks;
                        }
                    }
                }
            }

            return user;
        }
    }
}