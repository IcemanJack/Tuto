using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tuto.Web.AcceptanceTests.Tests
{
    [TestClass]
    public class RegisterLoginHelpRequestCreateTest : BasePageTest
    {
        [ClassInitialize]
        public static void classInit(TestContext context)
        {
            BasePageTest.classInitializier();
        }

        [ClassCleanup]
        public static void classClean()
        {
            BasePageTest.classCleanUp();
        }

        //[TestMethod]
        //public void a_helped_user_can_register_login_and_create_an_help_request()
        //{
 

        //    //var pageRegister = pageLogin.goToRegisterHelped();
        //    //pageRegister.display();

        //    var accountLoginTest = new AccountLoginTest();
        //    accountLoginTest.pageAccountLoginTestsInit();

        //    accountLoginTest.helped_can_login();

            


        //    var user = new Helped()
        //    {
        //        name = "François",
        //        lastName = "Bertrand",
        //        mail = "bert@horsemail.com",
        //        password = "mypassword"
        //    };

        //    //pageRegister.fillUserFiledsWithData(user);

        //    // TODO how to test schedule?
        //    //var pageLoginAfterRegistration = pageRegister.clickCreate();
        //    //pageLoginAfterRegistration.fillUserFiledsWithData(user);
        //    //pageLoginAfterRegistration.clickLogin();

        //    // TODO create an helprequest

        //}
    }
}
