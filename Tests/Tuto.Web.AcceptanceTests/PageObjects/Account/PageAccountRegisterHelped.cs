using OpenQA.Selenium;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.AcceptanceTests.Utilities;

namespace Tuto.Web.AcceptanceTests.PageObjects.Account
{
    public class PageAccountRegisterHelped : BasePageObject
    {
        private const string TEXT_BOX_MAIL_ID = "txt_mail";
        private const string TEXT_BOX_PASSWORD_ID = "txt_password";
        private const string TEXT_BOX_CONFIRM_PASSWORD_ID = "txt_confirmPassword";
        private const string TEXT_BOX_NAME_ID = "txt_name";
        private const string TEXT_BOX_LAST_NAME_ID = "txt_lastName";

        private const string BUTTON_REGISTER_ID = "btn_submit";
        public static string PAGE_ID = "Account_Register_Helped";

        public PageAccountRegisterHelped(IWebDriver driver, string pageId) : base(driver, pageId)
        {
            
        }

        public PageAccountRegisterHelped display()
        {
            this.driver.Navigate().GoToUrl(this.baseUrl + "/Account/RegisterHelped");
            return this;
        }

        public void fillUserFieldsWithData(Helped helped)
        {
            var userMailField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_MAIL_ID));
            userMailField.SendKeys(helped.mail);

            var userPasswordField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_PASSWORD_ID));
            userPasswordField.SendKeys(helped.password);

            var userConfirmPasswordField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_CONFIRM_PASSWORD_ID));
            userConfirmPasswordField.SendKeys(helped.password);

            var userNameField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_NAME_ID));
            userNameField.SendKeys(helped.name);

            var userLastNameField = this.driver.FindElement(By.CssSelector("#" + TEXT_BOX_LAST_NAME_ID));
            userLastNameField.SendKeys(helped.lastName);
        }

        public void fillCalendarWithData(ScheduleBlock[] blocks)
        {
            var calendarTestObject = new WeekCalendarJsTestObject(this.driver);
            var test = calendarTestObject.pageIsContainingCalendar();
            foreach (var scheduleBlock in blocks)
            {
                calendarTestObject.createCalendarCase(scheduleBlock.weekDay, scheduleBlock.startTime, scheduleBlock.startTime + 1);
            }
        }

        public PageAccountLogin clickCreate()
        {
            var createRegisterButton = this.driver.FindElement(By.CssSelector("#" + BUTTON_REGISTER_ID));
            createRegisterButton.Click();

            return new PageAccountLogin(this.driver, PageAccountLogin.PAGE_ID);
        }
    }
}