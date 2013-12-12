using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.Repository;

namespace Tuto.DataLayer.ModelUtilities
{
    public class AccountLoginUtilities
    {
        public static User getUserFromLogin(IEntityRepository repo, string givenEmail)
        {
            // check the account type for a given username
            
            // try as Helped User
            User foundUser = repo.single<Helped>(x => x.mail == givenEmail);
            if (foundUser != null)
            {
                return foundUser;
            }

            // try as a Tutor User
            foundUser = repo.single<Tutor>(x => x.mail == givenEmail);
            if (foundUser != null)
            {
                return foundUser;
            }

            // try as a Manager User
            foundUser = repo.single<Manager>(x => x.mail == givenEmail);
            if (foundUser != null)
            {
                return foundUser;
            }

            return null;
        }
    }
}
