using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.Web.AcceptanceTests.PageObjects.Account;

namespace Tuto.Web.AcceptanceTests.Tests.Account
{
    [TestClass]
    public class AccountRegisterTutorTest : BasePageTest
    {
        private PageAccountRegisterTutor pageAccountRegisterTutor;

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

        [TestInitialize]
        public void pageAccountRegisterTestsInit() 
        {
            //arrange
            this.pageAccountRegisterTutor = new PageAccountRegisterTutor(driver, PageAccountRegisterTutor.PAGE_ID);
        }

        [TestMethod]
        public void a_user_can_register()
        {
            this.pageAccountRegisterTutor.display();
            Assert.IsTrue(this.pageAccountRegisterTutor.containsPageId());
        }

    }
}


