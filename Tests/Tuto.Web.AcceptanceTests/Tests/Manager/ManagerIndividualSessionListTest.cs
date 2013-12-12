using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.Web.AcceptanceTests.PageObjects.Manager;

namespace Tuto.Web.AcceptanceTests.Tests.Manager
{
    [TestClass]
    public class ManagerIndividualSessionListTest : BasePageTest
    {
        private PageManagerIndividualSessionList pageIndividualSessionList;

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
        public void pageManagerIndividualSessionInitTests()
        {
            //arrange
            this.pageIndividualSessionList = new PageManagerIndividualSessionList(driver, PageManagerIndividualSessionList.PAGE_ID);
        }

        //[TestMethod]
        //public void a_user_can_display_individualSessionList_page()
        //{
        //    pageIndividualSessionList.display();
        //    Assert.IsTrue(pageIndividualSessionList.containsPageId());
        //}
    }
}
