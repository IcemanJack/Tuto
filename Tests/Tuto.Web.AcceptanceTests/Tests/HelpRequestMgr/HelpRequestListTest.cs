using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.Web.AcceptanceTests.PageObjects;
using Tuto.Web.AcceptanceTests.Tests.Account;

namespace Tuto.Web.AcceptanceTests.Tests.HelpRequestMgr
{
    [TestClass]
    public class HelpRequestListTest : BasePageTest
    {
        private AccountLoginTest accountLoginTests;
        private PageManagerHelpRequestList pageManagerHelpRequestList;

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
        public void pageAccountEditTestsInit()
        {
            // Arrange
            this.pageManagerHelpRequestList = new PageManagerHelpRequestList(driver, PageManagerHelpRequestList.PAGE_ID);

            // Reusing of acceptance tests
            this.accountLoginTests = new AccountLoginTest();
            this.accountLoginTests.pageAccountLoginTestsInit();
        }

        [TestMethod]
        public void manager_can_consult_helpRequest_list()
        {
            // Act
            this.accountLoginTests.manager_can_login();
            this.pageManagerHelpRequestList.display();

            // Asserts
            Assert.IsTrue(this.pageManagerHelpRequestList.containsPageId());
        }
        /*
        [TestMethod]
        public void manager_can_consult_tutor_report()
        {
            // Act
            accountLoginTests.tutor_can_login();
            pageManagerHelpRequestList.display();
            pageManagerHelpRequestList.clickConsultAs("Tutor");

            // Asserts
            Assert.IsTrue(pageManagerHelpRequestList.containsPageId());
            //Assert.IsTrue(reportToPopUpIsVisible());
        }
         */
    }
}
