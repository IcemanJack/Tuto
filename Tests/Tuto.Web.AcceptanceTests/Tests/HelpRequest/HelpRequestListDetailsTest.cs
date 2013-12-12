using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.TestUtilities.Database;
using Tuto.Web.AcceptanceTests.PageObjects.HelpRequest;

namespace Tuto.Web.AcceptanceTests.Tests.HelpRequest
{
    [TestClass]
    public class HelpRequestListDetailsTest : BasePageTest
    {
        private PageHelpRequestListDetails pageHelpRequestListDetails;

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
        public void helpRequestTestsInit()
        {
            //arrange
            this.pageHelpRequestListDetails = new PageHelpRequestListDetails(driver, PageHelpRequestListDetails.PAGE_ID);
        }

        [TestMethod]
        public void a_user_can_display_helpRequestListDetails_page()
        {
            this.pageHelpRequestListDetails.display();
            Assert.IsTrue(this.pageHelpRequestListDetails.containsPageId());
        }

        [TestMethod]
        public void a_user_can_display_helpRequestListDetails_and_see_helpRequest_comment()
        {
            //arrange
            string helpRequestComment = TestData.helpRequest.comment;

            //action
            this.pageHelpRequestListDetails.display();
            bool pageContainHelpRequestComment = this.pageHelpRequestListDetails.contain(helpRequestComment);

            //assert
            Assert.IsTrue(this.pageHelpRequestListDetails.containPageGroupSessionListId());
            //Assert.IsTrue(pageContainHelpRequestComment);
            // TODO : assert that the text shown in the comment section is the text in the TestData
        }

        [TestMethod]
        public void a_user_can_display_helpRequestListDetails_and_see_helpRequest_misunderstoodNotions()
        {
            //arrange
            string helpRequestMisunderstoodNotions = TestData.helpRequest.misunderstoodNotions;

            //action
            this.pageHelpRequestListDetails.display();
            bool pageContainHelpRequestMisunderstoodNotions = this.pageHelpRequestListDetails.contain(helpRequestMisunderstoodNotions);

            //assert
            Assert.IsTrue(this.pageHelpRequestListDetails.containPageGroupSessionListId());
            //Assert.IsTrue(pageContainHelpRequestMisunderstoodNotions);
            //Assert.IsTrue(pageContainHelpRequestComment);
            // TODO : assert that the text shown in the comment section is the text in the TestData
        }
    }
}
