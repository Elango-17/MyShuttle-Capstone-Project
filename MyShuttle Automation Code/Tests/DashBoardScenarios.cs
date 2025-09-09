/***************************************************************************************
* Project   : MyShuttle Application
* File Name : DashboardPageTests.cs
* Namespace : AppWeb
* 
* Description:
*   NUnit test suite for validating Dashboard Page functionality in the MyShuttle application.
*   Tests cover dashboard visibility, internal labels, user login state, navigation to Fare History,
*   and Sign Out behavior. Includes setup and teardown for WebDriver management.
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


using NUnit.Framework;
using AppOperations;
using Utilities;
using System;

namespace AppWeb
{
    /// <summary>
    /// Contains automated tests for verifying Dashboard page functionality,
    /// including visibility, navigation, and session handling.
    /// </summary>
    [TestFixture]
    [Category("UI")]
    [Category("Dashboard")]
    public class DashboardPageTests
    {
        private IDashboard _dashboard;

        /// <summary>
        /// Initializes the test by logging in and navigating to the Dashboard.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Use the DashboardPageFactory to create and log in
            _dashboard = DashboardPageFactory.Create(headless: true);
        }

        /// <summary>
        /// Cleans up resources after each test by disposing of the Dashboard instance.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (_dashboard is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Verifies that the Dashboard page is visible after a successful login.
        /// </summary>
        [Test]
        public void Dashboard_Should_Be_Visible()
        {
            Assert.IsTrue(_dashboard.IsDashboardVisible(),
                "Dashboard page should be visible after login.");
        }

        /// <summary>
        /// Verifies that the "Internal Use Only" label is displayed on the Dashboard.
        /// </summary>
        [Test]
        public void InternalUseOnly_Label_Should_Be_Visible()
        {
            Assert.IsTrue(_dashboard.IsInternalUseOnlyLabelVisible(),
                "Internal Use Only label should be visible on dashboard.");
        }

        /// <summary>
        /// Ensures the user session is still valid and the user is logged in
        /// when viewing the Dashboard.
        /// </summary>
        [Test]
        public void User_Should_Be_LoggedIn_On_Dashboard()
        {
            Assert.IsTrue(_dashboard.IsUserLoggedIn(),
                "User should be logged in when on the dashboard.");
        }

        /// <summary>
        /// Validates that navigating to Fare History from the Dashboard
        /// does not affect the logged-in state of the user.
        /// </summary>
        [Test]
        public void FareHistory_Should_Navigate_When_Clicked()
        {
            _dashboard.ClickFareHistory();
            Assert.IsTrue(_dashboard.IsUserLoggedIn(),
                "User should remain logged in after accessing fare history.");
        }

        /// <summary>
        /// Ensures that clicking the Sign Out option logs the user out successfully.
        /// </summary>
        [Test]
        public void SignOut_Should_Log_User_Out()
        {
            _dashboard.ClickSignOut();
            Assert.IsFalse(_dashboard.IsUserLoggedIn(),
                "User should be logged out after clicking Sign Out.");
        }
    }
}
