using OpenQA.Selenium;
using Tuto.DataLayer.Models.Users;

namespace Tuto.Web.AcceptanceTests.PageObjects.Account
{
    public class PageAccountLogin : BasePageObject
    {
        private const string TEXT_BOX_MAIL_ID = "txt_mail";
        private const string TEXT_BOX_PASSWORD_ID = "txt_password";

        private const string LINK_REGISTER_TUTOR = "lnk_register_tutor";
        private const string LINK_REGISTER_HELPED = "lnk_register_helped";

        private const string BUTTON_LOGIN_ID = "btn_login";
        public static string PAGE_ID = "Account_Login";

        public PageAccountLogin(IWebDriver driver, string pageId) : base(driver, pageId)
        {
            
        }

        public PageAccountLogin display()
        {
            this.driver.Navigate().GoToUrl(this.baseUrl + "/Account/Login");
            return this;
        }

        public void fillUserFiledsWithData(User user)
        {
            var userMailField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_MAIL_ID));
            userMailField.SendKeys(user.mail);

            var userPasswordField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_PASSWORD_ID));
            userPasswordField.SendKeys(user.password);
        }

        public PageAccountRegisterTutor goToRegisterTutor()
        {
            var registerTutorLink = this.driver.FindElement(By.CssSelector("#" + LINK_REGISTER_TUTOR));
            registerTutorLink.Click();

            return new PageAccountRegisterTutor(this.driver, PageAccountRegisterTutor.PAGE_ID);
        }

        public PageAccountRegisterHelped goToRegisterHelped()
        {
            var registerHelpedLink = this.driver.FindElement(By.CssSelector("#" + LINK_REGISTER_HELPED));
            registerHelpedLink.Click();

            return new PageAccountRegisterHelped(this.driver, PageAccountRegisterHelped.PAGE_ID);
        }

        public bool contain(string textToFind)
        {
            var isTextContained = this.driver.FindElement(By.Id(PAGE_ID))
                .Text
                .Contains(textToFind);

            return isTextContained;
        }

        public void clickLogin()
        {
            var loginButton = this.driver.FindElement(By.CssSelector("#" + BUTTON_LOGIN_ID));
            loginButton.Click();
        }
    }
}