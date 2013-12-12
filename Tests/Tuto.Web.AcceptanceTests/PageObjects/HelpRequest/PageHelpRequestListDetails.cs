using OpenQA.Selenium;

namespace Tuto.Web.AcceptanceTests.PageObjects.HelpRequest
{
    class PageHelpRequestListDetails : BasePageObject
    {
        public static string PAGE_ID = "HelpRequest_ListDetails";
        private static int details_id = 1;

        public PageHelpRequestListDetails(IWebDriver driver, string pageId) : base(driver, pageId)
        {
        }

        public PageHelpRequestListDetails display()
        {
            this.driver.Navigate().GoToUrl(this.baseUrl + "/HelpRequest/Details/" + details_id);
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
