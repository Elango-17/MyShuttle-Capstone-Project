/***************************************************************************************
* Project   : MyShuttle Application
* File Name : ILoginPage.cs
* Namespace : AppOperations
* 
* Description:
*   This interface defines the contract for Login Page operations within the 
*   MyShuttle application. It provides methods to interact with and validate 
*   various login page elements such as logo, login form, input fields, and 
*   authentication results.
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

namespace AppOperations
{
    /// <summary>
    /// Defines the operations and validations that can be performed 
    /// on the Login Page of the MyShuttle application.
    /// </summary>
    public interface ILoginPage
    {
        /// <summary>
        /// Checks whether the application logo is visible on the login page.
        /// </summary>
        /// <returns><c>true</c> if the logo is visible; otherwise, <c>false</c>.</returns>
        bool IsLogoVisible();

        /// <summary>
        /// Verifies if the login form (username & password fields) is displayed.
        /// </summary>
        /// <returns><c>true</c> if the login form is visible; otherwise, <c>false</c>.</returns>
        bool IsLoginFormVisible();

        /// <summary>
        /// Enters the username into the username input field.
        /// </summary>
        /// <param name="username">The username to be entered.</param>
        void EnterUsername(string username);

        /// <summary>
        /// Enters the password into the password input field.
        /// </summary>
        /// <param name="password">The password to be entered.</param>
        void EnterPassword(string password);

        /// <summary>
        /// Simulates clicking the login button.
        /// </summary>
        void ClickLogin();

        /// <summary>
        /// Retrieves the error message displayed after a failed login attempt.
        /// </summary>
        /// <returns>A string containing the error message text.</returns>
        string GetErrorMessage();

        /// <summary>
        /// Checks if the login button is currently enabled for user interaction.
        /// </summary>
        /// <returns><c>true</c> if the login button is enabled; otherwise, <c>false</c>.</returns>
        bool IsLoginButtonEnabled();

        /// <summary>
        /// Determines whether the login attempt was successful.
        /// </summary>
        /// <returns><c>true</c> if login is successful; otherwise, <c>false</c>.</returns>
        bool IsLoginSuccessful();
    }
}
