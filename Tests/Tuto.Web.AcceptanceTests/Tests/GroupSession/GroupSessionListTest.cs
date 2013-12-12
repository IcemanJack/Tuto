using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.Web.AcceptanceTests.PageObjects.GroupSession;

namespace Tuto.Web.AcceptanceTests.Tests.GroupSession
{
    [TestClass]
    public class GroupSessionListTest : BasePageTest
    {
        private PageGroupSessionList pageGroupSessionList;

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
            this.pageGroupSessionList = new PageGroupSessionList(driver, PageGroupSessionList.PAGE_ID);
        }

        //[TestMethod]
        //public void a_user_can_display_groupSession_page()
        //{
        //    this.pageGroupSessionList.display();
        //    Assert.IsTrue(pageGroupSessionList.containsPageId());
        //}

        //[TestMethod]
        //public void a_user_can_display_groupSession_page_and_see_groupSession_place()
        //{
        //    //arrange
        //    string groupSessionPlace = TestData.groupSession.place;

        //    //action
        //    pageGroupSessionList.display();
        //    bool pageContainGroupSessionPlace = pageGroupSessionList.contain(groupSessionPlace);

        //    //assert
        //    Assert.IsTrue(pageContainGroupSessionPlace);
        //}

        //[TestMethod]
        //public void a_user_can_display_groupSession_page_and_see_groupSession_date()
        //{
        //    //arrange
        //    String groupSessionDate = TestData.groupSession.date;

        //    //action
        //    pageGroupSessionList.display();
        //    bool pageContainGroupSessionDate = pageGroupSessionList.contain(groupSessionDate);

        //    //assert
        //    Assert.IsTrue(pageContainGroupSessionDate);
        //}
    }
}
