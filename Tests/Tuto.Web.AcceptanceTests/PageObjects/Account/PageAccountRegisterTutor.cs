using OpenQA.Selenium;
using Tuto.DataLayer.Models.Users;

namespace Tuto.Web.AcceptanceTests.PageObjects.Account
{
    public class PageAccountRegisterTutor : BasePageObject
    {
        private const string TEXT_BOX_MAIL_ID = "txt_mail";
        private const string TEXT_BOX_PASSWORD_ID = "txt_password";
        private const string TEXT_BOX_CONFIRM_PASSWORD_ID = "txt_confirmPassword";
        private const string TEXT_BOX_NAME_ID = "txt_name";
        private const string TEXT_BOX_LAST_NAME_ID = "txt_lastName";
        private const string CHECK_BOX_AVAILABLE_GROUP_ID = "chk_availableGroup";
        private const string CHECK_BOX_AVAILABLE_INDIVIDUAL_ID = "chk_availableIndividual";
        private const string BUTTON_REGISTER_ID = "btn_register";
        public static string PAGE_ID = "Account_Register_Tutor";

        public PageAccountRegisterTutor(IWebDriver driver, string pageId) : base(driver, pageId)
        {
            
        }

        public PageAccountRegisterTutor display()
        {
            this.driver.Navigate().GoToUrl(this.baseUrl + "/Account/RegisterTutor");
            return this;
        }

        public void fillUserFiledsWithData(Tutor tutor)
        {
            var userMailField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_MAIL_ID + " input"));
            userMailField.SendKeys(tutor.mail);

            var userPasswordField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_PASSWORD_ID + " input"));
            userPasswordField.SendKeys(tutor.password);

            var userConfirmPasswordField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_CONFIRM_PASSWORD_ID + " input"));
            userConfirmPasswordField.SendKeys(tutor.password);

            var userNameField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_NAME_ID + " input"));
            userNameField.SendKeys(tutor.name);

            var userLastNameField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_LAST_NAME_ID + " input"));
            userLastNameField.SendKeys(tutor.lastName);
        }

        public void fillTutorFieldsWithData(Tutor tutor)
        {
            var tutorAvailableGroup = this.driver.FindElement(By.CssSelector("#" + CHECK_BOX_AVAILABLE_GROUP_ID + " input"));
            tutorAvailableGroup.Click();

            var tutorAvailableIndividual = this.driver.FindElement(By.CssSelector("#" + CHECK_BOX_AVAILABLE_INDIVIDUAL_ID + " input"));
            tutorAvailableIndividual.Click();
        }

        public PageAccountLogin clickCreate()
        {
            var createRegisterButton = this.driver.FindElement(By.CssSelector("#" + BUTTON_REGISTER_ID + " input"));
            createRegisterButton.Click();

            return new PageAccountLogin(this.driver, PageAccountLogin.PAGE_ID);
        }
    }
}