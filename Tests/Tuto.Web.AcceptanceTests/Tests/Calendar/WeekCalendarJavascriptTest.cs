using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.DataLayer.Models;
using Tuto.Web.AcceptanceTests.Utilities;

namespace Tuto.Web.AcceptanceTests.Tests.Calendar
{
    public class WeekCalendarJavascriptTest : BasePageTest
    {
        [ClassInitialize]
        public static void classInit(TestContext context)
        {
            BasePageTest.classInitializier();
        }

        [ClassCleanup]
        public static void classClean()
        {
            BasePageTest.classCleanUp();
        }

        
        public void weekcalendar_test_click()
        {
            driver.Navigate().GoToUrl("http://calinours.local/Account/RegisterHelped");
            var jsTest = new WeekCalendarJsTestObject(driver);
            jsTest.createCalendarCase(WeekDay.TUESDAY, 8, 10);
            Assert.IsTrue(jsTest.containsEvent(WeekDay.TUESDAY, 8, 10));
        }
    }
}