using System;
using OpenQA.Selenium;

namespace Tuto.Web.AcceptanceTests.PageObjects
{
    class PageManagerHelpRequestList : BasePageObject
    {
        public static string PAGE_ID = "Manager_HelpRequests_List";
        public static string BUTTON_CLASS_CONSULT_TUTOR_REPORT = "consultTutorReport";
        public static string BUTTON_CLASS_CONSULT_HELPED_REPORT = "consultHelpedReport";

        private static string tutor = "Tutor";
        private static string helped = "Helped";

        public PageManagerHelpRequestList(IWebDriver driver, string pageId)
            : base(driver, pageId)
        {

        }

        public PageManagerHelpRequestList display()
        {
            this.driver.Navigate().GoToUrl(this.baseUrl + "/Manager/HelpRequests");
            return this;
        }

        //public bool reportToPopUpIsVisible();

        public bool clickConsultAs(String studentType)
        {
            string className = "";
            if (studentType.Equals(tutor))
            {
                className = "." + BUTTON_CLASS_CONSULT_TUTOR_REPORT;
            }
            else if (studentType.Equals(helped))
            {
                className = "." + BUTTON_CLASS_CONSULT_HELPED_REPORT;
            }

            if (!this.containsClass(className))
            {
                return false;
            }

            var btnConsultReport = this.driver.FindElement(By.ClassName(className));
            btnConsultReport.Click();
            return true;
        }
        
        private bool containsClass(String classname)
        {
            try
            {
                var consultTutorReport = this.driver.FindElement(By.ClassName(classname));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
