/***************************************************************************************
* Project   : MyShuttle Application
* File Name : PageFactories.cs
* Namespace : Utilities
* 
* Description:
*   This file contains static factory classes for creating instances of page objects
*   in the MyShuttle application using Selenium WebDriver. It includes:
*       - LogiPageFactory : LoginPage
*       - DashboardPageFactory : DashboardPage
*       - FareHistoryPageFactory : FareHistoryPage
*   Provides optional headless mode and dynamic credentials for login automation.
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
using OpenQA.Selenium.Chrome;
using AppOperations;
using AppWeb;
using MySql.Data.MySqlClient;

namespace Utilities
{
    /// <summary>
    /// Factory class for creating instances of LoginPage and common driver utilities.
    /// </summary>
    public static class LogiPageFactory
    {
        /// <summary>
        /// Creates an instance of LoginPage.
        /// </summary>
        /// <param name="headless">Run Chrome in headless mode if true.</param>
        /// <returns>ILoginPage instance.</returns>
        public static ILoginPage Create(bool headless = false)
        {
            IWebDriver driver = InitializeDriver(headless);
            driver.Navigate().GoToUrl("http://localhost:8080/myshuttledev/");
            return new LoginPage(driver);
        }

        /// <summary>
        /// Initializes a ChromeDriver instance with optional headless mode.
        /// </summary>
        /// <param name="headless">Run Chrome in headless mode if true.</param>
        /// <returns>IWebDriver instance.</returns>
        internal static IWebDriver InitializeDriver(bool headless)
        {
            var options = new ChromeOptions();
            if (headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");
            }

            return new ChromeDriver(options);
        }

        /// <summary>
        /// Performs login using the provided driver and credentials.
        /// </summary>
        /// <param name="driver">WebDriver instance.</param>
        /// <param name="username">Username for login. Default is "fred".</param>
        /// <param name="password">Password for login. Default is "fredpassword".</param>
        internal static void PerformLogin(IWebDriver driver, string username = "fred", string password = "fredpassword")
        {
            var loginPage = new LoginPage(driver);
            loginPage.EnterUsername(username);
            loginPage.EnterPassword(password);
            loginPage.ClickLogin();
        }
    }

    /// <summary>
    /// Factory class for creating instances of DashboardPage.
    /// </summary>
    public static class DashboardPageFactory
    {
        /// <summary>
        /// Creates an instance of DashboardPage after performing login.
        /// </summary>
        /// <param name="headless">Run Chrome in headless mode if true.</param>
        /// <param name="username">Username for login. Default is "fred".</param>
        /// <param name="password">Password for login. Default is "fredpassword".</param>
        /// <returns>IDashboard instance.</returns>
        public static IDashboard Create(bool headless = false, string username = "fred", string password = "fredpassword")
        {
            IWebDriver driver = LogiPageFactory.InitializeDriver(headless);
            driver.Navigate().GoToUrl("http://localhost:8080/myshuttledev/");

            // Perform login
            LogiPageFactory.PerformLogin(driver, username, password);

            return new DashboardPage(driver);
        }
    }

    /// <summary>
    /// Factory class for creating instances of FareHistoryPage.
    /// </summary>
    public static class FareHistoryPageFactory
    {
        /// <summary>
        /// Creates an instance of FareHistoryPage after performing login and navigation.
        /// </summary>
        /// <param name="headless">Run Chrome in headless mode if true.</param>
        /// <param name="username">Username for login. Default is "fred".</param>
        /// <param name="password">Password for login. Default is "fredpassword".</param>
        /// <returns>IFairHistory instance.</returns>
        public static IFairHistory Create(bool headless = false, string username = "fred", string password = "fredpassword")
        {
            IWebDriver driver = LogiPageFactory.InitializeDriver(headless);
            driver.Navigate().GoToUrl("http://localhost:8080/myshuttledev/");

            // Perform login
            LogiPageFactory.PerformLogin(driver, username, password);

            // Navigate to Fare History from Dashboard
            var dashboardPage = new DashboardPage(driver);
            dashboardPage.ClickFareHistory();

            return new FareHistoryPage(driver);
        }
    }
}
