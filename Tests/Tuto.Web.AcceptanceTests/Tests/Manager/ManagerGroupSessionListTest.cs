using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.Web.AcceptanceTests.PageObjects.Manager;

namespace Tuto.Web.AcceptanceTests.Tests.Manager
{
    [TestClass]
    public class ManagerGroupSessionListTest : BasePageTest
    {
        private PageManagerGroupSessionList pageManagerGroupSessionList;

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
        public void groupSessionTestsInit()
        {
            //arrange
            this.pageManagerGroupSessionList = new PageManagerGroupSessionList(driver, PageManagerGroupSessionList.PAGE_ID);
        }

        //[TestMethod]
        //public void a_manager_can_display_groupSession_page()
        //{
        //    pageManagerGroupSessionList.display();
        //    Assert.IsTrue(pageManagerGroupSessionList.containsPageId());
        //}

        //[TestMethod]
        //public void a_manager_can_display_groupSession_page_and_see_groupSession_place()
        //{
        //    a_manager_can_display_groupSession_page_and_see_groupSession_element(TestData.groupSession.place);
        //}

        //[TestMethod]
        //public void a_manager_can_display_groupSession_page_and_see_groupSession_time()
        //{
        //    a_manager_can_display_groupSession_page_and_see_groupSession_element(TestData.groupSession.time);
        //}

        //[TestMethod]
        //public void a_manager_can_display_groupSession_page_and_see_groupSession_date()
        //{
        //    a_manager_can_display_groupSession_page_and_see_groupSession_element(TestData.groupSession.date);
        //}

        //private void a_manager_can_display_groupSession_page_and_see_groupSession_element(String testDataElement)
        //{
        //    //arrange
        //    string groupSession = testDataElement;

        //    //action
        //    pageManagerGroupSessionList.display();
        //    bool pageContainGroupSession = pageManagerGroupSessionList.contain(groupSession);

        //    //assert
        //    Assert.IsTrue(pageContainGroupSession);
        //}
    }
}
