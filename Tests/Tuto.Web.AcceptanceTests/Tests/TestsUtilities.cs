using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Ploeh.AutoFixture;
using Tuto.TestUtility.AutoFixture;

namespace Tuto.Web.AcceptanceTests.Tests
{
    public class TestsUtilities
    {
        private const string TEXT_BOX_MAIL_ID = "txt_mail";
        private const string TEXT_BOX_PASSWORD_ID = "txt_password";
        private const string BUTTON_LOGIN_ID = "btn_login";

        public static void bypassAuthentification(IWebDriver theDriver, string username, string password)
        {
            theDriver.Navigate().GoToUrl("http://calinours.local/Account/Login");
            var userMailField = theDriver.FindElement(By.CssSelector("#" + TEXT_BOX_MAIL_ID + " input"));
            userMailField.SendKeys(username);

            var userPasswordField = theDriver.FindElement(By.CssSelector("#" + TEXT_BOX_PASSWORD_ID + " input"));
            userPasswordField.SendKeys(password);

            var loginButton = theDriver.FindElement(By.CssSelector("#" + BUTTON_LOGIN_ID + " input"));
            loginButton.Click();
        }

    }
}

