/***************************************************************************************
* Project   : MyShuttle Application
* File Name : PageFactories.cs
* Namespace : Utilities
* 
* Description:
* This file defines static factory classes for instantiating page objects
* in the MyShuttle application using Selenium WebDriver.
* 
* It includes:
* - DriverManager            : Centralized driver creation and configuration.
* - LogiPageFactory          : Provides LoginPage objects and login automation.
* - DashboardPageFactory     : Provides DashboardPage objects after login.
* - FareHistoryPageFactory   : Provides FareHistoryPage objects after login and navigation.
* 
* Configuration:
* - Reads BaseUrl and Browser type from `appsettings.json`.
* - Supports Chrome, Firefox, and Edge browsers.
* - Allows optional headless mode for CI/CD pipelines or non-UI runs.
* 
* Authors:
* - Elangovan
* - Gayathri
* - Teja
* 
* License:
* MIT License
* Copyright (c) 2025 MyShuttle Team
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

using AppOperations;
using AppWeb;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Utilities
{
    /// <summary>
    /// Manages the creation of Selenium WebDriver instances based on configuration.
    /// </summary>
    public static class DriverManager
    {
        private static readonly IConfiguration _config;

        static DriverManager()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        /// <summary>
        /// Creates and configures a Selenium <see cref="IWebDriver"/> instance 
        /// based on the browser and base URL defined in <c>appsettings.json</c>.
        /// </summary>
        /// <param name="headless">
        /// If set to <c>true</c>, the browser runs in headless mode (no UI window).  
        /// This is useful for CI/CD pipelines or automated environments where 
        /// displaying a browser window is not required.
        /// </param>
        /// <returns>
        /// A fully initialized <see cref="IWebDriver"/> instance, navigated to the configured BaseUrl.
        /// </returns>
        /// <remarks>
        /// Supported browsers (from <c>appsettings.json</c>):
        /// <list type="bullet">
        ///   <item><description><c>chrome</c> - Uses the latest version of ChromeDriver.</description></item>
        ///   <item><description><c>firefox</c> - Uses a FirefoxDriver matching the installed browser version.</description></item>
        ///   <item><description><c>edge</c> - Uses the latest version of EdgeDriver.</description></item>
        /// </list>
        /// 
        /// <para>
        /// If <c>headless = true</c>:
        /// <list type="bullet">
        ///   <item><description>Chrome adds <c>--headless</c>, <c>--disable-gpu</c>, and <c>--window-size</c> arguments.</description></item>
        ///   <item><description>Firefox adds <c>--headless</c> argument.</description></item>
        ///   <item><description>Edge adds <c>headless</c> argument.</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown when an unsupported browser value is specified in <c>appsettings.json</c>.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when <c>BaseUrl</c> is missing or not set in <c>appsettings.json</c>.
        /// </exception>
        public static IWebDriver CreateDriver(bool headless = false)
        {
            string browser = _config["Browser"]?.ToLower();
            IWebDriver driver;

            switch (browser)
            {
                case "chrome":
                    new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.Latest);
                    var chromeOptions = new ChromeOptions();
                    if (headless)
                    {
                        chromeOptions.AddArgument("--headless");
                        chromeOptions.AddArgument("--disable-gpu");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                    }
                    driver = new ChromeDriver(chromeOptions);
                    break;

                case "firefox":
                    new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig(), VersionResolveStrategy.MatchingBrowser);
                    var firefoxOptions = new FirefoxOptions();
                    if (headless)
                        firefoxOptions.AddArgument("--headless");
                    driver = new FirefoxDriver(firefoxOptions);
                    break;

                case "edge":
                    new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig(), VersionResolveStrategy.Latest);
                    var edgeOptions = new EdgeOptions();
                    if (headless)
                        edgeOptions.AddArgument("headless");
                    driver = new EdgeDriver(edgeOptions);
                    break;

                default:
                    throw new NotSupportedException(
                        $"Browser '{browser}' is not supported. Use Chrome, Firefox, or Edge.");
            }

            string baseUrl = _config["BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new Exception("BaseUrl is not set in appsettings.json");
            }

            driver.Navigate().GoToUrl(baseUrl);
            return driver;
        }
    }

    // ================================================================
    // Page Factories
    // ================================================================

    /// <summary>
    /// Factory for creating <see cref="LoginPage"/> instances and performing login.
    /// </summary>
    public static class LogiPageFactory
    {
        /// <summary>
        /// Creates a new <see cref="LoginPage"/> instance.
        /// </summary>
        /// <param name="headless">If true, browser runs in headless mode.</param>
        /// <returns>An <see cref="ILoginPage"/> implementation.</returns>
        public static ILoginPage Create(bool headless = false)
        {
            IWebDriver driver = DriverManager.CreateDriver(headless);
            return new LoginPage(driver);
        }

        /// <summary>
        /// Performs login using given or default credentials.
        /// </summary>
        /// <param name="driver">The active WebDriver instance.</param>
        /// <param name="username">Username for login (default: fred).</param>
        /// <param name="password">Password for login (default: fredpassword).</param>
        internal static void PerformLogin(IWebDriver driver, string username = "fred", string password = "fredpassword")
        {
            var loginPage = new LoginPage(driver);
            loginPage.EnterUsername(username);
            loginPage.EnterPassword(password);
            loginPage.ClickLogin();
        }
    }

    /// <summary>
    /// Factory for creating <see cref="DashboardPage"/> instances after login.
    /// </summary>
    public static class DashboardPageFactory
    {
        /// <summary>
        /// Creates a new <see cref="DashboardPage"/> instance after logging in.
        /// </summary>
        /// <param name="headless">If true, browser runs in headless mode.</param>
        /// <param name="username">Username for login (default: fred).</param>
        /// <param name="password">Password for login (default: fredpassword).</param>
        /// <returns>An <see cref="IDashboard"/> implementation.</returns>
        public static IDashboard Create(bool headless = false, string username = "fred", string password = "fredpassword")
        {
            IWebDriver driver = DriverManager.CreateDriver(headless);
            LogiPageFactory.PerformLogin(driver, username, password);
            return new DashboardPage(driver);
        }
    }

    /// <summary>
    /// Factory for creating <see cref="FareHistoryPage"/> instances after login and navigation.
    /// </summary>
    public static class FareHistoryPageFactory
    {
        /// <summary>
        /// Creates a new <see cref="FareHistoryPage"/> instance by logging in and navigating
        /// from the dashboard.
        /// </summary>
        /// <param name="headless">If true, browser runs in headless mode.</param>
        /// <param name="username">Username for login (default: fred).</param>
        /// <param name="password">Password for login (default: fredpassword).</param>
        /// <returns>An <see cref="IFairHistory"/> implementation.</returns>
        public static IFairHistory Create(bool headless = false, string username = "fred", string password = "fredpassword")
        {
            IWebDriver driver = DriverManager.CreateDriver(headless);
            LogiPageFactory.PerformLogin(driver, username, password);
            var dashboardPage = new DashboardPage(driver);
            dashboardPage.ClickFareHistory();
            return new FareHistoryPage(driver);
        }


        /// <summary>
        /// Provides safe wrappers for locating web elements with Selenium WebDriver.
        /// </summary>
        public static class WebElementHelper
        {
            /// <summary>
            /// Tries to find an element by a given locator with an optional timeout.
            /// </summary>
            /// <param name="driver">The Selenium WebDriver instance.</param>
            /// <param name="by">The locator strategy.</param>
            /// <param name="timeoutInSeconds">Maximum wait time in seconds (default: 5).</param>
            /// <returns>The located <see cref="IWebElement"/> if found; otherwise, <c>null</c>.</returns>
            public static IWebElement? SafeFindElement(IWebDriver driver, By by, int timeoutInSeconds = 5)
            {
                try
                {
                    if (timeoutInSeconds > 0)
                    {
                        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                        return wait.Until(drv => drv.FindElement(by));
                    }
                    return driver.FindElement(by);
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (WebDriverTimeoutException)
                {
                    return null;
                }
            }

            /// <summary>
            /// Checks whether an element exists on the page.
            /// </summary>
            /// <param name="driver">The Selenium WebDriver instance.</param>
            /// <param name="by">The locator strategy.</param>
            /// <returns><c>true</c> if the element is present; otherwise, <c>false</c>.</returns>
            public static bool IsElementPresent(IWebDriver driver, By by)
            {
                return SafeFindElement(driver, by) != null;
            }
        }
    }
}
