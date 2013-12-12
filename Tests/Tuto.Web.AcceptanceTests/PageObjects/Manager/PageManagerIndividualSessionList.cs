
using OpenQA.Selenium;

namespace Tuto.Web.AcceptanceTests.PageObjects.Manager
{
    public class PageManagerIndividualSessionList : BasePageObject
    {
        public const string PAGE_ID = "individualsession_list";

        public PageManagerIndividualSessionList(IWebDriver driver, string pageId) : base(driver, pageId)
        {
        }

        public PageManagerIndividualSessionList display()
        {
            this.driver.Navigate().GoToUrl(this.baseUrl + "/Manager/IndividualSessionList");
            return this;
        }
        
        public bool containPageIndividualSessionListId()
        {
            try
            {
                this.driver.FindElement(By.Id(PAGE_ID));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool contain(string textToFind)
        {
            var isTextContained = this.driver.FindElement(By.Id(PAGE_ID))
                .Text
                .Contains(textToFind);

            return isTextContained;
        }
    }
}
