using OpenQA.Selenium;

namespace Tuto.Web.AcceptanceTests.PageObjects.GroupSession
{
    class PageGroupSessionList : BasePageObject
    {
        public const string PAGE_ID = "Group_Session_List";

        public PageGroupSessionList(IWebDriver driver, string pageId) : base(driver, pageId)
        {
        }

        public PageGroupSessionList display()
        {
            this.driver.Navigate().GoToUrl(this.baseUrl + "/GroupSession");
            return this;
        }
        
        public bool containPageGroupSessionListId()
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
