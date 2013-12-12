using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.DataLayer.Enums;
using Tuto.TestUtilities.Database;
using Tuto.Web.AcceptanceTests.PageObjects.Manager;

namespace Tuto.Web.AcceptanceTests.Tests.Manager
{
    [TestClass]
    public class ManagerTutorListTest : BasePageTest
    {
        private PageManagerTutorList pageManagerTutorList;

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
        public void pageManagerTutorListInitTests()
        {
            //arrange
            this.pageManagerTutorList = new PageManagerTutorList(driver, PageManagerTutorList.PAGE_ID);
        }

        [TestMethod]
        public void a_user_can_display_managertutorlist_page()
        {
            this.pageManagerTutorList.display();
            Assert.IsTrue(this.pageManagerTutorList.containsPageId());
        }

        [TestMethod]
        public void a_manager_can_confirm_a_tutor()
        {
            //arrange
            TutorState tutorState = TestData.tutor.tutorState;

            //action
            this.pageManagerTutorList.display();
           // bool pageContainGroupSessionDate = pageGroupSessionList.contain(groupSessionDate);

            //assert
            //Assert.IsTrue(pageContainGroupSessionDate);

        }

        public void a_manager_can_refuse_a_tutor()
        {
            
        }
    }
}
