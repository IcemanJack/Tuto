using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.Web.AcceptanceTests.PageObjects.Account;

namespace Tuto.Web.AcceptanceTests.Tests.Account
{
    [TestClass]
    public class AccountRegisterHelpedTest : BasePageTest
    {
        private PageAccountRegisterHelped pageAccountRegisterHelped;

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
            this.pageAccountRegisterHelped = new PageAccountRegisterHelped(driver, PageAccountRegisterHelped.PAGE_ID);
        }

        [TestMethod]
        public void a_user_can_register()
        {
            this.pageAccountRegisterHelped.display();
            this.pageAccountRegisterHelped.fillUserFieldsWithData(helped);

            //pageAccountRegisterHelped.fillCalendarWithData(TestData.scheduleBlocks);
            //var pageLogin = pageAccountRegisterHelped.clickCreate();

            Assert.IsTrue(this.pageAccountRegisterHelped.containsPageId());
        }
    }
}


