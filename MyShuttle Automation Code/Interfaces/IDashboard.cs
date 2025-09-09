/***************************************************************************************
* Project   : MyShuttle Application
* File Name : IDashboard.cs
* Namespace : AppOperations
* 
* Description:
*   This interface defines the contract for Dashboard operations within the 
*   MyShuttle application. It provides methods to verify dashboard visibility, 
*   interact with navigation elements (Fare History, Sign Out), and validate 
*   user login state.
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
    /// on the Dashboard of the MyShuttle application.
    /// </summary>
    public interface IDashboard
    {
        /// <summary>
        /// Checks whether the Dashboard is currently visible to the user.
        /// </summary>
        /// <returns><c>true</c> if the Dashboard is visible; otherwise, <c>false</c>.</returns>
        bool IsDashboardVisible();

        /// <summary>
        /// Navigates to the Fare History page from the Dashboard.
        /// </summary>
        void ClickFareHistory();

        /// <summary>
        /// Performs the sign-out action for the current user.
        /// </summary>
        void ClickSignOut();

        /// <summary>
        /// Determines whether a user is currently logged into the system.
        /// </summary>
        /// <returns><c>true</c> if a user session is active; otherwise, <c>false</c>.</returns>
        bool IsUserLoggedIn();

        /// <summary>
        /// Checks if the "Internal Use Only" label is visible on the Dashboard.
        /// </summary>
        /// <returns><c>true</c> if the label is visible; otherwise, <c>false</c>.</returns>
        bool IsInternalUseOnlyLabelVisible();
    }
}
