using OpenQA.Selenium;

namespace Tuto.Web.AcceptanceTests.PageObjects.Manager
{
    class PageManagerGroupSessionList : BasePageObject
    {
        public const string PAGE_ID = "Manager_Group_Session_List";

        public PageManagerGroupSessionList(IWebDriver driver, string pageId) : base(driver, pageId)
        {
        }

        public PageManagerGroupSessionList display()
        {
            this.driver.Navigate().GoToUrl(this.baseUrl + "/Manager/GroupSessions/list");
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
