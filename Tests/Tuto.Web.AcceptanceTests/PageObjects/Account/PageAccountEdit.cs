using OpenQA.Selenium;
using Tuto.DataLayer.Models.Users;

namespace Tuto.Web.AcceptanceTests.PageObjects
{
    class PageAccountEdit : BasePageObject
    {
        private const string DIV_SUCCESS_MESSAGE_ID = "div_SuccessMessage";

        private const string TEXT_BOX_NEWNAME_ID = "txt_NewName";
        private const string TEXT_BOX_NEWLASTNAME_ID = "txt_NewLastName";
        private const string TEXT_BOX_NEWEMAIL_ID = "txt_NewEmail";
        private const string TEXT_BOX_CONFIRMNEWEMAIL_ID = "txt_ConfirmNewEmail";
        private const string TEXT_BOX_NEWPASSWORD_ID = "txt_NewPassword";
        private const string TEXT_BOX_CONFITMEDNEWPASSWORD_ID = "txt_ConfirmNewPassword";

        private const string TEXT_BOX_PASSWORD_ID = "txt_Password";
        private const string TEXT_BOX_EMAIL_ID = "txt_Email";

        private const string BUTTON_SAVE_ID = "btn_Save";
        private const string BUTTON_HOME_ID = "btn_Home";
        public static string PAGE_ID = "Account_Edit_Tutor";

        public PageAccountEdit(IWebDriver driver, string pageId) : base(driver, pageId)
        {

        }

        public PageAccountEdit display()
        {
            this.driver.Navigate().GoToUrl(this.baseUrl + "/Account/Edit");
            return this;
        }

        public void clickSave()
        {
            var saveEditButton = this.driver.FindElement(By.CssSelector("#" + BUTTON_SAVE_ID));
            saveEditButton.Click();
        }

        public void clickHome()
        {
            var homeEditButton = this.driver.FindElement(By.CssSelector("#" + BUTTON_HOME_ID));
            homeEditButton.Click();
        }

        public void fillUserEmailFieldWithData(User user)
        {
            var userPasswordField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_EMAIL_ID));
            userPasswordField.SendKeys(user.mail);
        }

        public void fillUserPasswordFieldWithData(User user)
        {
            var userPasswordField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_PASSWORD_ID));
            userPasswordField.SendKeys(user.password);
        }

        public void fillUserNewNameFieldsWithData(string name)
        {
            var userNewNameField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_NEWNAME_ID));
            userNewNameField.SendKeys(name);
        }

        public void fillUserNewLastNameFieldsWithData(string lastName)
        {
            var userNewLastNameField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_NEWLASTNAME_ID));
            userNewLastNameField.SendKeys(lastName);
        }

        public void fillUserNewPasswordFieldsWithData(string password)
        {
            var userNewPasswordField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_NEWPASSWORD_ID));
            userNewPasswordField.SendKeys(password);

            var userConfirmedNewPasswordField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_CONFITMEDNEWPASSWORD_ID));
            userConfirmedNewPasswordField.SendKeys(password);
        }

        public void fillUserNewEmailFieldsWithData(string email)
        {
            var userNewEmailField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_NEWEMAIL_ID));
            userNewEmailField.SendKeys(email);
        }

        public void fillUserConfirmNewEmailFieldsWithData(string email)
        {
            var userNewEmailField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_CONFIRMNEWEMAIL_ID));
            userNewEmailField.SendKeys(email);
        }

        public bool containsSuccessMessage()
        {
            var divExists = this.driver.FindElement(By.CssSelector("#" + DIV_SUCCESS_MESSAGE_ID));
            if (divExists != null)
            {
                return true;
            }
            return false;
        }
    }
}
