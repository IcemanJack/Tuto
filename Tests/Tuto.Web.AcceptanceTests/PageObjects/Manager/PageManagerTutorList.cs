using OpenQA.Selenium;

namespace Tuto.Web.AcceptanceTests.PageObjects.Manager
{
    public class PageManagerTutorList : BasePageObject
    {
        public static string PAGE_ID = "Manager_Tutor_List";

        public PageManagerTutorList(IWebDriver driver, string pageId) : base(driver, pageId)
        {

        }

        public PageManagerTutorList display()
        {
            this.driver.Navigate().GoToUrl(this.baseUrl + "/Manager/Tutors/list");
            return this;
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
