using System.Collections.Generic;
using System.Linq;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.Repository;
using Tuto.Web.ViewModels;

namespace Tuto.Web.Controllers.Utilities
{
    public class UsersUtilities
    {
        public class TutorsUtilities
        {
            public static List<Tutor> getListOfAvailableTutorsAt(IEntityRepository repository, WeekDay weekDay, int startTime, int endTime)
            {
                return repository.getAll<Tutor>().ToList()
                    .Where(
                        x =>
                            x.isAvailableAt(weekDay, startTime, endTime) &&
                            x.tutorAvailableForGroupSession
                    ).ToList();
            }

            public static List<Tutor> getListOfAvailableTutorsAt(IEntityRepository repository, TutorAvailableAtViewModel viewModel)
            {
                return UsersUtilities.TutorsUtilities.getListOfAvailableTutorsAt(repository, viewModel.weekDay, viewModel.startTime, viewModel.endTime);
            }
        }

    }
}