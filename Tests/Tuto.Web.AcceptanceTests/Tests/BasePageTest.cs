using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Users;
using Tuto.TestUtility.AutoFixture;

namespace Tuto.Web.AcceptanceTests.Tests
{
    [TestClass]
    public class BasePageTest
    {
        protected static IWebDriver driver;
        protected static Fixture fixture;
        protected static Tutor tutor;
        protected static Helped helped;
        protected static DataLayer.Models.Users.Manager manager;
        
        
        protected static void classInitializier()
        {
            setFireFoxDriver();
            seedTestDataToDatabase();
            driver.Quit();

            fixture = new Fixture();
            fixture.Customizations.Add(new VirtualMembersOmitter());

            tutor = fixture.Create<Tutor>();
            helped = fixture.Create<Helped>();
            manager = fixture.Create<DataLayer.Models.Users.Manager>();
        }

        protected static void classCleanUp()
        {
            
        }

        [TestInitialize]
        public void start()
        {
            setFireFoxDriver();
        }

        [TestCleanup]
        public void close()
        {
            driver.Quit();
        }

        protected static void setChromeDriver()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
        }

        protected static void setFireFoxDriver()
        {
            driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
        }

        private static void seedTestDataToDatabase()
        {
            driver.Navigate().GoToUrl("http://calinours.local/CI");
            driver.FindElement(By.CssSelector("a")).Click();
        }

    }
}

