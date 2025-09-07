/***************************************************************************************
* Project   : MyShuttle Application
* File Name : DashboardPage.cs
* Namespace : AppWeb
* 
* Description:
*   This class implements the IDashboard interface and provides concrete 
*   Selenium WebDriver interactions for the Dashboard Page of the MyShuttle application. 
*   It supports navigation (Fare History, Sign Out) and validation of 
*   dashboard visibility, login state, and internal labels.
* 
* Author(s) :
*   - Elangovan
*   - Gayathri
*   - Teja
* 
* License  : MIT License
* 
* Copyright (c) 2025 MyShuttle Team (Elangovan, Gayathri, Teja)
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
***************************************************************************************/


using OpenQA.Selenium;
using AppOperations;

namespace AppWeb
{
    /// <summary>
    /// Represents the Dashboard Page of the MyShuttle application 
    /// and provides methods to interact with and validate its elements 
    /// using Selenium WebDriver.
    /// </summary>
    public class DashboardPage : IDashboard
    {
        private readonly IWebDriver _driver;

        // ===== Locators =====
        private By dashboardTitle => By.XPath("//h2[text()='Dashboard']");
        private By fareHistoryButton => By.LinkText("Access Your Fare History");
        private By signOutButton => By.XPath("//a[contains(@href,'logout.jsp')]");
        private By internalUseLabel => By.XPath("//h5[contains(text(),'Internal Use Only')]");

        // ===== Constructor =====
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardPage"/> class.
        /// </summary>
        /// <param name="driver">The Selenium WebDriver instance used for browser automation.</param>
        public DashboardPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // ===== Navigation Actions =====

        /// <summary>
        /// Navigates to the Fare History page by clicking the corresponding link.
        /// </summary>
        public void ClickFareHistory()
        {
            _driver.FindElement(fareHistoryButton).Click();
        }

        /// <summary>
        /// Signs out the current user by clicking the sign-out link.
        /// </summary>
        public void ClickSignOut()
        {
            _driver.FindElement(signOutButton).Click();
        }

        // ===== Validations =====

        /// <summary>
        /// Checks if the Dashboard is currently visible.
        /// </summary>
        /// <returns><c>true</c> if the Dashboard is visible; otherwise, <c>false</c>.</returns>
        public bool IsDashboardVisible()
        {
            try
            {
                return _driver.FindElement(dashboardTitle).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if a user is currently logged in.
        /// </summary>
        /// <remarks>
        /// The check is based on the current URL. 
        /// Adjust logic if your application uses different routes for dashboard or login.
        /// </remarks>
        /// <returns><c>true</c> if the user appears logged in; otherwise, <c>false</c>.</returns>
        public bool IsUserLoggedIn()
        {
            try
            {
                string currentUrl = _driver.Url.ToLower();
                return currentUrl.Contains("dashboard") ||
                       currentUrl.Contains("login") ||
                       currentUrl.Contains("home.jsp");
            }
            catch (WebDriverException)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the "Internal Use Only" label is visible on the Dashboard.
        /// </summary>
        /// <returns><c>true</c> if the label is visible; otherwise, <c>false</c>.</returns>
        public bool IsInternalUseOnlyLabelVisible()
        {
            try
            {
                return _driver.FindElement(internalUseLabel).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
