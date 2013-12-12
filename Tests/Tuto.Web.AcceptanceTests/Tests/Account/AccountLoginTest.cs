using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.Web.AcceptanceTests.PageObjects.Account;

namespace Tuto.Web.AcceptanceTests.Tests.Account
{
    [TestClass]
    public class AccountLoginTest : BasePageTest
    {
        private PageAccountLogin pageAccountLogin;

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
        public void pageAccountLoginTestsInit()
        {
            // Arrange
            this.pageAccountLogin = new PageAccountLogin(driver, PageAccountLogin.PAGE_ID);
        }

        [TestMethod]
        public void user_can_display_login_page()
        {
            // Act
            this.pageAccountLogin.display();
            // Assert
            Assert.IsTrue(this.pageAccountLogin.containsPageId());
        }

        [TestMethod]
        public void tutor_can_login()
        {
            // Act
            this.pageAccountLogin.display();
            tutor.mail = "tutor@horsemail.com";
            tutor.password = "mypassword";
            this.pageAccountLogin.fillUserFiledsWithData(tutor);
            this.pageAccountLogin.clickLogin();
            // Assert
            Assert.IsTrue(this.pageAccountLogin.containsPageId("Home_Index"));
        }

        [TestMethod]
        public void helped_can_login()
        {
            // Act
            this.pageAccountLogin.display();
            helped.mail = "helped@horsemail.com";
            helped.password = "mypassword";
            this.pageAccountLogin.fillUserFiledsWithData(helped);
            this.pageAccountLogin.clickLogin();
            // Assert
            Assert.IsTrue(this.pageAccountLogin.containsPageId("Home_Index"));
        }

        [TestMethod]
        public void manager_can_login()
        {
            // Act
            this.pageAccountLogin.display();
            manager.mail = "fbertrand@mail.com";
            manager.password = "mypassword";
            this.pageAccountLogin.fillUserFiledsWithData(manager);
            this.pageAccountLogin.clickLogin();
            // Assert
            Assert.IsTrue(this.pageAccountLogin.containsPageId("Home_Index"));
        }
    }
}
