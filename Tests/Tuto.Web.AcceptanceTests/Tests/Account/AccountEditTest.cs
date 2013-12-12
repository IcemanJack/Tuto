using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.Web.AcceptanceTests.PageObjects;

namespace Tuto.Web.AcceptanceTests.Tests.Account
{
    [TestClass]
    public class AccountEditTest : BasePageTest
    {
        private AccountLoginTest accountLoginTests;
        private PageAccountEdit pageAccountEdit;

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
            this.pageAccountEdit = new PageAccountEdit(driver, PageAccountEdit.PAGE_ID);

            // Reusing of acceptance tests
            this.accountLoginTests = new AccountLoginTest(); 
            this.accountLoginTests.pageAccountLoginTestsInit();
        }
        
        [TestMethod]
        public void tutor_can_display_edit_page()
        {
            // Act
            this.accountLoginTests.tutor_can_login();
            this.pageAccountEdit.display();

            // Asserts
            Assert.IsTrue(this.pageAccountEdit.containsPageId());
        }

        [TestMethod]
        public void tutor_can_edit_name()
        {
            // Act
            this.accountLoginTests.tutor_can_login();
            this.pageAccountEdit.display();

            this.pageAccountEdit.fillUserNewNameFieldsWithData("OG");
            this.pageAccountEdit.fillUserEmailFieldWithData(tutor);
            this.pageAccountEdit.fillUserPasswordFieldWithData(tutor);
            this.pageAccountEdit.clickSave();

            // Asserts
            Assert.IsTrue(this.pageAccountEdit.containsPageId());
            Assert.IsTrue(this.pageAccountEdit.containsSuccessMessage());
        }
    }
}
