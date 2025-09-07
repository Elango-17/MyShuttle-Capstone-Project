/***************************************************************************************
* Project   : MyShuttle Application
* File Name : LoginPageTests.cs
* Namespace : AppWeb
* 
* Description:
*   NUnit test suite for validating Login Page functionality in the MyShuttle application.
*   Tests cover UI elements (logo, form, login button), login workflow (success/failure), 
*   and error messages. Implements setup and teardown for WebDriver management.
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

using AppOperations;
using Utilities;
using System;

namespace AppWeb
{
    /// <summary>
    /// NUnit Test Fixture for validating the Login Page functionality.
    /// </summary>
    [TestFixture]
    public class LoginPageTests
    {
        private ILoginPage _loginPage;

        /// <summary>
        /// Initializes the login page before each test execution.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _loginPage = LogiPageFactory.Create(headless: true);
        }

        /// <summary>
        /// Cleans up the driver and disposes resources after each test execution.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (_loginPage is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        // =====================================================================
        // UI Visibility Tests
        // =====================================================================

        /// <summary>
        /// Ensures that the application logo is visible on the login page.
        /// </summary>
        [Test]
        public void Logo_Should_Be_Visible()
        {
            Assert.IsTrue(_loginPage.IsLogoVisible());
        }

        /// <summary>
        /// Ensures that the login form is visible on the login page.
        /// </summary>
        [Test]
        public void LoginForm_Should_Be_Visible()
        {
            Assert.IsTrue(_loginPage.IsLoginFormVisible());
        }

        /// <summary>
        /// Ensures that the login button is initially disabled before entering credentials.
        /// </summary>
        [Test]
        public void LoginButton_Should_Be_Disabled_Initially()
        {
            Assert.IsFalse(_loginPage.IsLoginButtonEnabled());
        }

        // =====================================================================
        // Credential Entry Tests
        // =====================================================================

        /// <summary>
        /// Verifies that entering both username and password enables the login button.
        /// </summary>
        [Test]
        public void EnterUsername_And_Password_Should_Enable_LoginButton()
        {
            _loginPage.EnterUsername("fred");
            _loginPage.EnterPassword("fredPassword");
            Assert.IsTrue(_loginPage.IsLoginButtonEnabled());
        }

        // =====================================================================
        // Authentication Tests
        // =====================================================================

        /// <summary>
        /// Ensures that login fails with invalid credentials, 
        /// and the appropriate error message is displayed.
        /// </summary>
        [Test]
        public void Login_Should_Fail_With_Invalid_Credentials()
        {
            _loginPage.EnterUsername("free");
            _loginPage.EnterPassword("freepassword");
            _loginPage.ClickLogin();

            string error = _loginPage.GetErrorMessage();

            Assert.That(error, Does.Contain("Sorry, please back up and try again"));
            Assert.That(error, Does.Contain("We couldn't find your email and password."));
            Assert.IsFalse(_loginPage.IsLoginSuccessful());
        }

        /// <summary>
        /// Ensures that login succeeds with valid credentials.
        /// </summary>
        [Test]
        public void Login_Should_Succeed_With_Valid_Credentials()
        {
            _loginPage.EnterUsername("fred");
            _loginPage.EnterPassword("fredpassword");
            _loginPage.ClickLogin();

            Assert.IsTrue(_loginPage.IsLoginSuccessful());
        }
    }
}
