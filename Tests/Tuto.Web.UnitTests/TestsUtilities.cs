using System;
using System.Linq.Expressions;
using NSubstitute;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;

namespace Tuto.Web.UnitTests
{
    public class TestsUtilities
    {
        // this will bypass the application authentification when the app launch context is mocked.
        // this allows the unit tests to bypass the system authentification system
        public static void bypassAppAuthentification(WebAppLaunchContext currentContext, User dummyUser)
        {
            const string userMail = "thiscanbeanything, really!";
            const string userPassword = "password are delicious!";

            // make sure to enter valid login credientials so it looks into the db
            currentContext.getHttpContext().Session["loggedIn"] = true;
            currentContext.getHttpContext().Session["mail"] = userMail;
            currentContext.getHttpContext().Session["password"] = userPassword;

            // return a dummy user
            Expression<Func<User, bool>> expr = (u => u.mail == userMail && u.password == userPassword);

            if (dummyUser.isHelped())
            {
                currentContext.getRepository().single<Helped>(null).ReturnsForAnyArgs(dummyUser as Helped);
                currentContext.getRepository().single<Tutor>(null).ReturnsForAnyArgs(null as Tutor);
                currentContext.getRepository().single<Manager>(null).ReturnsForAnyArgs(null as Manager);
            }
            else if (dummyUser.isTutor())
            {
                currentContext.getRepository().single<Helped>(null).ReturnsForAnyArgs(null as Helped);
                currentContext.getRepository().single<Tutor>(null).ReturnsForAnyArgs(dummyUser as Tutor);
                currentContext.getRepository().single<Manager>(null).ReturnsForAnyArgs(null as Manager); 
            }
            else if (dummyUser.isManager())
            {
                currentContext.getRepository().single<Helped>(null).ReturnsForAnyArgs(null as Helped);
                currentContext.getRepository().single<Tutor>(null).ReturnsForAnyArgs(null as Tutor);
                currentContext.getRepository().single<Manager>(null).ReturnsForAnyArgs(dummyUser as Manager);
            }

            /*currentContext.getRepository()
                .single<User>(x => x.mail == userMail && x.password == userPassword)
                .ReturnsForAnyArgs(dummyUser);*/
        } 
    }
}