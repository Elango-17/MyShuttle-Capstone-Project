/***************************************************************************************
* Project   : MyShuttle Application
* File Name : FareHistoryPageTests.cs
* Namespace : AppWeb
* 
* Description:
*   NUnit test suite for validating Fare History Page functionality in the MyShuttle application.
*   Tests cover page heading, table visibility, column headers, row data, sorting, 
*   ratings, trip dates, navigation, and internal labels. Includes setup and teardown 
*   for WebDriver management.
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
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace AppWeb
{
    /// <summary>
    /// NUnit test suite for verifying Fare History Page functionality:
    /// - Page headings and labels
    /// - Fare table structure
    /// - Data validation (fares, dates, ratings)
    /// - Navigation and error handling
    /// </summary>
    [TestFixture]
    [Category("UI")]
    [Category("FareHistory")]
    public class FareHistoryPageTests
    {
        private IFairHistory _fareHistory;

        /// <summary>
        /// Runs before each test to create a new instance of the Fare History Page.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _fareHistory = FareHistoryPageFactory.Create(headless: true);
        }

        /// <summary>
        /// Runs after each test to properly dispose of WebDriver resources.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            (_fareHistory as IDisposable)?.Dispose();
        }

        // ===== PAGE VALIDATION =====

        /// <summary>
        /// Validates that the page heading starts with "Employee Fares for".
        /// Ensures correct page is loaded.
        /// </summary>
        [Test]
        public void PageHeading_Should_Be_EmployeeFares()
        {
            StringAssert.StartsWith("Employee Fares for", _fareHistory.GetPageHeading());
        }

        /// <summary>
        /// Validates that the logged-in user's name appears in the heading text.
        /// Ensures personalization is working correctly.
        /// </summary>
        [Test]
        public void PageHeading_Should_Contain_LoggedInUserName()
        {
            string loggedInUserName = _fareHistory.GetLoggedInUserName();
            string heading = _fareHistory.GetPageHeading();
            StringAssert.Contains(loggedInUserName, heading, "Page heading should contain the logged-in username.");
        }

        /// <summary>
        /// Ensures that the fare table is visible on the page.
        /// </summary>
        [Test]
        public void FareTable_Should_Be_Visible()
        {
            Assert.IsTrue(_fareHistory.IsFareTableVisible());
        }

        /// <summary>
        /// Verifies that the "Internal Use Only" label is displayed.
        /// Ensures compliance with application requirements.
        /// </summary>
        [Test]
        public void InternalUseOnly_Label_Should_Be_Visible()
        {
            Assert.IsTrue(_fareHistory.IsInternalUseOnlyLabelVisible());
        }

        // ===== TABLE STRUCTURE =====

        /// <summary>
        /// Validates that the fare table has exactly 9 columns.
        /// </summary>
        [Test]
        public void FareTable_Should_Have_CorrectNumberOfColumns()
        {
            var columns = _fareHistory.GetAllColumnHeaders();
            Assert.AreEqual(9, columns.Count, "Fare table should have 9 columns");
        }

        /// <summary>
        /// Verifies that the column headers match the expected values.
        /// </summary>
        [Test]
        public void ColumnHeaders_Should_Match_Expected()
        {
            var expected = new List<string>
            {
                "ID", "Start", "Pickup", "End", "Dropoff",
                "Fare", "Driver", "Pass Rtg", "Drvr Rtg"
            };
            CollectionAssert.AreEqual(expected, _fareHistory.GetAllColumnHeaders());
        }

        /// <summary>
        /// Ensures that each row in the fare table contains the expected number of cells.
        /// </summary>
        [Test]
        public void EachFareRow_Should_Have_ExactNumberOfCells()
        {
            var rows = _fareHistory.GetNumberOfFareRecords();
            for (int i = 0; i < rows; i++)
            {
                var rowText = _fareHistory.GetFareDetailsByRow(i);
                var cells = rowText.Split(new[] { '\n' }, StringSplitOptions.None);
                Assert.AreEqual(1, cells.Length, $"Row {i} should have 9 cells");
            }
        }

        /// <summary>
        /// Confirms that HasColumn() returns true for existing columns.
        /// </summary>
        [Test]
        public void HasColumn_Should_Return_True_For_ExistingColumns()
        {
            foreach (var col in _fareHistory.GetAllColumnHeaders())
                Assert.IsTrue(_fareHistory.HasColumn(col));
        }

        /// <summary>
        /// Confirms that HasColumn() returns false for a non-existent column.
        /// </summary>
        [Test]
        public void HasColumn_Should_Return_False_For_NonExistingColumn()
        {
            Assert.IsFalse(_fareHistory.HasColumn("NonExistent"));
        }

        // ===== DATA VALIDATION =====

        /// <summary>
        /// Ensures that the fare table contains at least one record.
        /// </summary>
        [Test]
        public void FareTable_Should_Have_AtLeast_One_Record()
        {
            Assert.Greater(_fareHistory.GetNumberOfFareRecords(), 0);
        }

        /// <summary>
        /// Validates that each trip's pickup date is earlier than the dropoff date.
        /// </summary>
        [Test]
        public void TripDates_Should_Be_Valid()
        {
            foreach (var trip in _fareHistory.GetTripDates())
                Assert.Less(trip.Pickup, trip.Dropoff);
        }

        /// <summary>
        /// Ensures that pickup and dropoff fields can be parsed as valid DateTime values.
        /// </summary>
        [Test]
        public void PickupAndDropoff_ShouldBeValidDateTime()
        {
            foreach (var trip in _fareHistory.GetTripDates())
            {
                Assert.DoesNotThrow(() => DateTime.Parse(trip.Pickup.ToString()));
                Assert.DoesNotThrow(() => DateTime.Parse(trip.Dropoff.ToString()));
            }
        }

        /// <summary>
        /// Ensures that all fare amounts are greater than zero.
        /// </summary>
        [Test]
        public void FareAmounts_Should_Not_Be_Negative()
        {
            var fares = _fareHistory.GetFareAmounts().ToList();
            Assert.That(fares, Is.All.GreaterThan(0));
        }

        /// <summary>
        /// Verifies that drivers are returned in sorted alphabetical order.
        /// </summary>
        [Test]
        public void Drivers_Should_Be_Sorted_Alphabetically_Programmatically()
        {
            var drivers = _fareHistory.SortByDriver().ToList();
            var sorted = drivers.OrderBy(d => d).ToList();
            CollectionAssert.AreEqual(sorted, drivers);
        }

        /// <summary>
        /// Ensures passenger ratings fall within the valid range (1–5).
        /// </summary>
        [Test]
        public void PassengerRatings_Should_Be_Within_ValidRange()
        {
            foreach (var rating in _fareHistory.GetPassangerRatings())
                Assert.That(rating, Is.InRange(1, 5), "Passenger rating must be between 1 and 5");
        }

        /// <summary>
        /// Ensures driver ratings fall within the valid range (1–5).
        /// </summary>
        [Test]
        public void DriverRatings_Should_Be_Within_ValidRange()
        {
            foreach (var rating in _fareHistory.GetDriverRatings())
                Assert.That(rating, Is.InRange(1, 5), "Driver rating must be between 1 and 5");
        }

        // ===== NAVIGATION & ERROR HANDLING =====

        /// <summary>
        /// Verifies that clicking "Back to Dashboard" navigates the user to the dashboard.
        /// </summary>
        [Test]
        public void ClickBackToDashboard_Should_Navigate_To_Dashboard()
        {
            _fareHistory.ClickBackToDashboard();
            Assert.IsTrue(_fareHistory.IsDashboardVisible());
        }

        /// <summary>
        /// Validates that requesting fare details for an invalid row index throws an exception.
        /// </summary>
        [Test]
        public void GetFareDetailsByRow_Should_Throw_On_InvalidIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _fareHistory.GetFareDetailsByRow(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _fareHistory.GetFareDetailsByRow(999));
        }

        /// <summary>
        /// Ensures the "No records found" message is displayed when the table is empty.
        /// </summary>
        [Test]
        public void NoRecordMessage_Should_Be_Displayed_When_NoData()
        {
            Assert.AreEqual("No records found", _fareHistory.GetNoRecordMessage());
        }
    }
}
