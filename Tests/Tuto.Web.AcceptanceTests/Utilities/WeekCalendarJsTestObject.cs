using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Tuto.DataLayer.Models;

namespace Tuto.Web.AcceptanceTests.Utilities
{
    public class WeekCalendarJsTestObject
    {
        private const string CALENDAR_ID = "calendar";
        private const int BOX_SIZE = 60; // eventbox size
        private const int STARTS_AT_HOUR = 8;

        private readonly IJavaScriptExecutor jsExecutor;
        private readonly IWebDriver webDriver;

        public WeekCalendarJsTestObject(IWebDriver theDriver)
        {
            if (!(theDriver is IJavaScriptExecutor))
            {
                throw new Exception("The driver must support JavaScript script execution on client side");
            }
            
            this.jsExecutor = (IJavaScriptExecutor) theDriver;
            this.webDriver = theDriver;

            if (!this.pageIsContainingCalendar())
            {
                throw new Exception("The page source must contain a calendar to test with");
            }
        }

        public bool pageIsContainingCalendar()
        {
            bool result = false;
            try
            {
                this.webDriver.FindElement(By.Id(CALENDAR_ID));
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public void createCalendarCase(WeekDay weekDay, int hourFrom, int hourTo)
        {
            string columnCssSelector = "#calendar .wc-grid-row-events .day-" + (int)weekDay;
            int mouseDownYPos = this.convertHoursToPos(hourFrom);
            int mouseUpYPos = this.convertHoursToPos(hourTo);

            // trigger mouse down
            this.jsExecutor.ExecuteScript(this.getRowJsTriggerString(columnCssSelector, "mousedown", mouseDownYPos));

            // trigger mouse up
            this.jsExecutor.ExecuteScript(this.getRowJsTriggerString(columnCssSelector, "mouseup", mouseUpYPos));
        }

        public bool containsEvent(WeekDay weekDay, int hourFrom, int hourTo)
        {
            try
            {
                string eventCasesSelector = "#calendar .wc-grid-row-events .day-" + (int)weekDay + " .wc-cal-event";
                IReadOnlyList<IWebElement> events = this.webDriver.FindElements(By.CssSelector(eventCasesSelector));

                // we're sure we're in the good day-* column of the calendar since the css selector implicitly specifies it
                foreach (IWebElement webElement in events)
                {
                    // hour check
                    string topCssValueStr = webElement.GetCssValue("top");
                    string heightCssValueStr = webElement.GetCssValue("height");
                    int topCssValue = int.Parse(topCssValueStr.Substring(0, topCssValueStr.Length - 2));
                    int heightCssValue = int.Parse(heightCssValueStr.Substring(0, heightCssValueStr.Length - 2));

                    int eventHourFrom = this.convertPosToHours(topCssValue);
                    int eventHourTo = this.convertPosToHours(heightCssValue);
                    if (hourFrom == eventHourFrom && hourTo == eventHourTo)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int convertHoursToPos(int hour)
        {
            return (hour - 1) * BOX_SIZE; // each boxes are 30px height
        }

        private int convertPosToHours(int yPos)
        {
            return ((int)Math.Round((double)(yPos / BOX_SIZE))) + STARTS_AT_HOUR; // the calendar starts at 8am
        }

        private string getRowJsTriggerString(string selector, string eventType, int yPos)
        {
            return @"$('" + selector + "').trigger('" + eventType + "', { pageY: " + yPos + " });";
        }
    }
}