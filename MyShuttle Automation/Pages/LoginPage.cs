/***************************************************************************************
* Project   : MyShuttle Application
* File Name : LoginPage.cs
* Namespace : AppWeb
* 
* Description:
*   This class implements the ILoginPage interface and provides concrete 
*   Selenium WebDriver interactions for the Login Page of the MyShuttle application.
*   It supports actions such as entering credentials, clicking login, 
*   and validating UI elements (logo, form, button, error messages, etc.).
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


using System;
using AppOperations;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AppWeb
{
    /// <summary>
    /// Represents the Login Page of the MyShuttle application and provides 
    /// methods to interact with and validate its elements using Selenium WebDriver.
    /// </summary>
    public class LoginPage : ILoginPage
    {
        private readonly IWebDriver _driver;

        // ===== Locators =====
        private readonly By logoLocator = By.CssSelector("img[src*='logologin.png']");
        private readonly By loginFormLocator = By.CssSelector("form[action='login'][method='post']");
        private readonly By usernameInput = By.Id("email"); // Updated from "username" to "email"
        private readonly By passwordInput = By.Id("password");
        private readonly By loginButton = By.CssSelector("input[type='submit'][value='Log in']");
        private readonly By errorMessageLocator = By.ClassName("error-message"); // Fallback if markup changes

        // ===== Constructor =====
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        /// <param name="driver">The Selenium WebDriver instance used for browser automation.</param>
        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // ===== Actions =====

        /// <summary>
        /// Enters the provided username (email) into the username input field.
        /// </summary>
        /// <param name="username">The username/email to be entered.</param>
        public void EnterUsername(string username)
        {
            var usernameField = _driver.FindElement(usernameInput);
            usernameField.Clear();
            usernameField.SendKeys(username);
        }

        /// <summary>
        /// Enters the provided password into the password input field.
        /// </summary>
        /// <param name="password">The password to be entered.</param>
        public void EnterPassword(string password)
        {
            var passwordField = _driver.FindElement(passwordInput);
            passwordField.Clear();
            passwordField.SendKeys(password);
        }

        /// <summary>
        /// Clicks the login button to submit the login form.
        /// </summary>
        public void ClickLogin()
        {
            _driver.FindElement(loginButton).Click();
        }

        // ===== Validations =====

        /// <summary>
        /// Checks if the application logo is visible on the login page.
        /// </summary>
        /// <returns><c>true</c> if the logo is visible; otherwise, <c>false</c>.</returns>
        public bool IsLogoVisible()
        {
            return _driver.FindElement(logoLocator).Displayed;
        }

        /// <summary>
        /// Checks if the login form is visible on the login page.
        /// </summary>
        /// <returns><c>true</c> if the login form is visible; otherwise, <c>false</c>.</returns>
        public bool IsLoginFormVisible()
        {
            return _driver.FindElement(loginFormLocator).Displayed;
        }

        /// <summary>
        /// Checks whether the login button is currently enabled.
        /// </summary>
        /// <returns><c>true</c> if the login button is enabled; otherwise, <c>false</c>.</returns>
        public bool IsLoginButtonEnabled()
        {
            return _driver.FindElement(loginButton).Enabled;
        }

        /// <summary>
        /// Determines if the login was successful by checking the page URL.
        /// </summary>
        /// <returns><c>true</c> if login is successful; otherwise, <c>false</c>.</returns>
        public bool IsLoginSuccessful()
        {
            return _driver.Url.Contains("login");
        }

        // ===== Messages =====

        /// <summary>
        /// Retrieves the error message displayed after a failed login attempt.
        /// Combines header and paragraph text if available.
        /// </summary>
        /// <returns>A string containing the error message, or empty if none exists.</returns>
        public string GetErrorMessage()
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                var errorHeader = wait.Until(d => d.FindElement(By.CssSelector(".jumbotron h2")));
                var errorParagraph = _driver.FindElement(By.CssSelector(".jumbotron p"));

                return $"{errorHeader.Text} {errorParagraph.Text}".Trim();
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }
    }
}
