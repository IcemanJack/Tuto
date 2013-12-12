using System;
using OpenQA.Selenium;

namespace Tuto.Web.AcceptanceTests.PageObjects
{
    public class BasePageObject
    {
        protected string baseUrl = "http://calinours.local";
        protected IWebDriver driver;
        protected string pageId;

        public BasePageObject(IWebDriver driver, string pageId)
        {
            this.driver = driver;
            this.pageId = pageId;
        }

        public bool containsPageId()
        {
            try
            {
                this.driver.FindElement(By.Id(this.pageId));
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool containsPageId(String pageId)
        {
            try
            {
                this.driver.FindElement(By.Id(pageId));
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}