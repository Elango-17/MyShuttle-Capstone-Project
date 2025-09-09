/***************************************************************************************
* Project   : MyShuttle Application
* File Name : IFairHistory.cs
* Namespace : AppOperations
* 
* Description:
*   This interface defines the contract for Fare History Page operations within the 
*   MyShuttle application. It provides methods to interact with and validate the 
*   fare history table, retrieve fare details, perform sorting operations, and handle 
*   navigation and session state.
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
using System.Collections.Generic;

namespace AppOperations
{
    /// <summary>
    /// Defines the operations and validations that can be performed 
    /// on the Fare History Page of the MyShuttle application.
    /// </summary>
    public interface IFairHistory
    {
        // ===== Page Info =====

        /// <summary>
        /// Retrieves the heading text of the Fare History page.
        /// </summary>
        /// <returns>A string containing the page heading.</returns>
        string GetPageHeading();

        /// <summary>
        /// Gets the username of the currently logged-in user.
        /// </summary>
        /// <returns>A string containing the logged-in user's name.</returns>
        string GetLoggedInUserName();

        /// <summary>
        /// Checks if the Fare table is visible on the page.
        /// </summary>
        /// <returns><c>true</c> if the Fare table is visible; otherwise, <c>false</c>.</returns>
        bool IsFareTableVisible();

        /// <summary>
        /// Retrieves the total number of fare records displayed in the table.
        /// </summary>
        /// <returns>An integer representing the number of fare records.</returns>
        int GetNumberOfFareRecords();

        /// <summary>
        /// Retrieves the details of a fare record based on the given row index.
        /// </summary>
        /// <param name="rowIndex">The index of the row (0-based).</param>
        /// <returns>A string containing fare details for the specified row.</returns>
        string GetFareDetailsByRow(int rowIndex);

        /// <summary>
        /// Retrieves all column headers of the Fare History table.
        /// </summary>
        /// <returns>A list of strings containing the column headers.</returns>
        IList<string> GetAllColumnHeaders();

        /// <summary>
        /// Checks if the Fare History table contains a specific column.
        /// </summary>
        /// <param name="columnName">The name of the column to search for.</param>
        /// <returns><c>true</c> if the column exists; otherwise, <c>false</c>.</returns>
        bool HasColumn(string columnName);

        /// <summary>
        /// Checks if the "Internal Use Only" label is displayed on the page.
        /// </summary>
        /// <returns><c>true</c> if the label is visible; otherwise, <c>false</c>.</returns>
        bool IsInternalUseOnlyLabelVisible();

        // ===== Navigation & Session =====

        /// <summary>
        /// Navigates to the Fare History page.
        /// </summary>
        void NavigateTo();

        /// <summary>
        /// Navigates back to the Dashboard page.
        /// </summary>
        void ClickBackToDashboard();

        /// <summary>
        /// Checks if the Login Page is currently visible.
        /// </summary>
        /// <returns><c>true</c> if the Login Page is visible; otherwise, <c>false</c>.</returns>
        bool IsLoginPageVisible();

        /// <summary>
        /// Checks if the Dashboard is currently visible.
        /// </summary>
        /// <returns><c>true</c> if the Dashboard is visible; otherwise, <c>false</c>.</returns>
        bool IsDashboardVisible();

        /// <summary>
        /// Retrieves the number of rows present in the Fare table.
        /// </summary>
        /// <returns>An integer representing the number of rows.</returns>
        int GetFareRowCount();

        /// <summary>
        /// Retrieves the number of mock fare records for testing purposes.
        /// </summary>
        /// <returns>An integer representing the number of mock records.</returns>
        int GetMockFareRecordCount();

        // ===== Sorting / Data Extraction =====

        /// <summary>
        /// Retrieves all fare details from the table.
        /// </summary>
        /// <returns>An enumerable collection of fare detail strings.</returns>
        IEnumerable<string> GetAllFareDetails();

        /// <summary>
        /// Sorts fare records by fare amount.
        /// </summary>
        /// <returns>An enumerable collection of decimal values sorted by fare.</returns>
        IEnumerable<decimal> SortByFare();

        /// <summary>
        /// Sorts fare records by driver name.
        /// </summary>
        /// <returns>An enumerable collection of driver names sorted alphabetically.</returns>
        IEnumerable<string> SortByDriver();

        /// <summary>
        /// Retrieves all fare amounts from the table.
        /// </summary>
        /// <returns>An enumerable collection of decimal values representing fares.</returns>
        IEnumerable<decimal> GetFareAmounts();

        /// <summary>
        /// Retrieves all passenger ratings from the fare records.
        /// </summary>
        /// <returns>An enumerable collection of passenger rating integers.</returns>
        IEnumerable<int> GetPassangerRatings();

        /// <summary>
        /// Retrieves all driver ratings from the fare records.
        /// </summary>
        /// <returns>An enumerable collection of driver rating integers.</returns>
        IEnumerable<int> GetDriverRatings();

        /// <summary>
        /// Retrieves the trip pickup and drop-off dates for all fare records.
        /// </summary>
        /// <returns>
        /// An enumerable collection of tuples containing Pickup and Dropoff DateTime values.
        /// </returns>
        IEnumerable<(DateTime Pickup, DateTime Dropoff)> GetTripDates();

        // ===== Messages / Edge Cases =====

        /// <summary>
        /// Retrieves the "No Record" message displayed when no fares are available.
        /// </summary>
        /// <returns>A string containing the no record message.</returns>
        string GetNoRecordMessage();
    }
}
