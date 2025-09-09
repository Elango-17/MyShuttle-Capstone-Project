/***************************************************************************************
* Project   : MyShuttle Application
* File Name : FareHistoryPage.cs
* Namespace : AppWeb
* 
* Description:
*   This class implements the IFairHistory interface and provides concrete 
*   Selenium WebDriver interactions for the Fare History Page of the MyShuttle application.
*   Implements Page Object Model (POM) for Fare History Page.
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
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AppWeb
{
    /// <summary>
    /// Page Object Model for the Fare History page.
    /// Provides methods to interact with UI elements and retrieve fare details.
    /// </summary>
    public class FareHistoryPage : IFairHistory, IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // ===== Locators =====
        private static readonly By PageHeading = By.CssSelector("h2");
        private static readonly By FareTable = By.CssSelector("table.table-striped.table-condensed");
        private static readonly By TableRows = By.CssSelector("table.table-striped.table-condensed tbody tr.table-row");
        private static readonly By TableHeaders = By.CssSelector("tr.info th");
        private static readonly By PanelFooter = By.CssSelector(".panel-footer");
        private static readonly By NoRecordMessageCell = By.CssSelector("table.table-striped.table-condensed tbody tr td");

        /// <summary>
        /// Initializes a new instance of the <see cref="FareHistoryPage"/> class.
        /// </summary>
        public FareHistoryPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(60));
        }

        /// <summary>
        /// Ensures driver cleanup.
        /// </summary>
        public void Dispose() => _driver.Quit();


        // =========================================================================
        // Page Information
        // =========================================================================

        /// <summary>
        /// Gets the page heading text.
        /// </summary>
        public string GetPageHeading() =>
            _wait.Until(d => d.FindElement(PageHeading)).Text;

        /// <summary>
        /// Extracts the logged-in user's name from the heading.
        /// </summary>
        public string GetLoggedInUserName()
        {
            var fullHeading = _driver.FindElement(PageHeading).Text.Trim();
            return fullHeading.Split(' ').Last();
        }

        /// <summary>
        /// Verifies if fare table is visible.
        /// </summary>
        public bool IsFareTableVisible() =>
            _wait.Until(d => d.FindElement(FareTable)).Displayed;

        /// <summary>
        /// Checks if "Internal Use Only" label is visible.
        /// </summary>
        public bool IsInternalUseOnlyLabelVisible() =>
            _driver.FindElement(PanelFooter).Displayed;

        /// <summary>
        /// Checks if Dashboard is currently visible.
        /// </summary>
        public bool IsDashboardVisible() =>
            _driver.Url.Contains("login", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Checks if Login page is currently visible.
        /// </summary>
        public bool IsLoginPageVisible() =>
            _driver.Url.Contains("login", StringComparison.OrdinalIgnoreCase);


        // =========================================================================
        // Navigation
        // =========================================================================

        /// <summary>
        /// Navigates to the Fare History page.
        /// </summary>
        public void NavigateTo()
        {
            _driver.Navigate().GoToUrl("http://localhost:8080/myshuttledev/home.jsp");
            _wait.Until(ExpectedConditions.ElementIsVisible(FareTable));
        }

        /// <summary>
        /// Navigates back to Dashboard page.
        /// </summary>
        public void ClickBackToDashboard()
        {
            _driver.Navigate().Back();
            _wait.Until(d => d.Url.Contains("login", StringComparison.OrdinalIgnoreCase));
        }


        // =========================================================================
        // Data Extraction
        // =========================================================================

        /// <summary>
        /// Returns total number of fare records.
        /// </summary>
        public int GetNumberOfFareRecords() =>
            _driver.FindElements(TableRows).Count;

        /// <summary>
        /// Gets fare details of a specific row.
        /// </summary>
        public string GetFareDetailsByRow(int rowIndex)
        {
            var rows = _driver.FindElements(TableRows);
            if (rowIndex < 0 || rowIndex >= rows.Count)
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            return rows[rowIndex].Text;
        }

        /// <summary>
        /// Gets all column headers from the table.
        /// </summary>
        public IList<string> GetAllColumnHeaders() =>
            _driver.FindElements(TableHeaders).Select(e => e.Text.Trim()).ToList();

        /// <summary>
        /// Checks whether a specific column exists in the table.
        /// </summary>
        public bool HasColumn(string columnName) =>
            GetAllColumnHeaders().Any(c => c.Equals(columnName, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Returns all fare records (row texts).
        /// </summary>
        public IEnumerable<string> GetAllFareDetails() =>
            _driver.FindElements(TableRows).Select(r => r.Text);

        /// <summary>
        /// Extracts fare amounts from table rows.
        /// </summary>
        public IEnumerable<decimal> GetFareAmounts()
        {
            var rows = _driver.FindElements(TableRows);
            return rows.Select(r =>
            {
                var text = r.FindElement(By.CssSelector("td:nth-child(6)")).Text.Replace("$", "").Trim();
                return decimal.Parse(text, CultureInfo.InvariantCulture);
            });
        }

        /// <summary>
        /// Extracts passenger ratings.
        /// </summary>
        public IEnumerable<int> GetPassangerRatings()
        {
            var rows = _driver.FindElements(TableRows);
            return rows.Select(r => int.Parse(r.FindElement(By.CssSelector("td:nth-child(8)")).Text.Trim()));
        }

        /// <summary>
        /// Extracts driver ratings.
        /// </summary>
        public IEnumerable<int> GetDriverRatings()
        {
            var rows = _driver.FindElements(TableRows);
            return rows.Select(r => int.Parse(r.FindElement(By.CssSelector("td:nth-child(9)")).Text.Trim()));
        }

        /// <summary>
        /// Extracts pickup and dropoff dates.
        /// </summary>
        public IEnumerable<(DateTime Pickup, DateTime Dropoff)> GetTripDates()
        {
            var rows = _driver.FindElements(TableRows);
            return rows.Select(r =>
            {
                var pickup = DateTime.Parse(r.FindElement(By.CssSelector("td:nth-child(2)")).Text, CultureInfo.InvariantCulture);
                var dropoff = DateTime.Parse(r.FindElement(By.CssSelector("td:nth-child(4)")).Text, CultureInfo.InvariantCulture);
                return (pickup, dropoff);
            });
        }

        /// <summary>
        /// Gets fare row count.
        /// </summary>
        public int GetFareRowCount() =>
            _driver.FindElements(TableRows).Count;

        /// <summary>
        /// Returns mocked fare record count (for test).
        /// </summary>
        public int GetMockFareRecordCount() => 2;

        /// <summary>
        /// Gets all cells of a fare row.
        /// </summary>
        public IList<string> GetFareCellsByRow(int rowIndex)
        {
            var rows = _driver.FindElements(TableRows);
            if (rowIndex < 0 || rowIndex >= rows.Count)
                throw new ArgumentOutOfRangeException(nameof(rowIndex));

            var cells = rows[rowIndex].FindElements(By.TagName("td"));
            return cells.Select(c => c.Text.Trim()).ToList();
        }


        // =========================================================================
        // Sorting
        // =========================================================================

        /// <summary>
        /// Sorts drivers alphabetically.
        /// </summary>
        public IEnumerable<string> SortByDriver()
        {
            var rows = _driver.FindElements(TableRows);
            return rows.Select(r => r.FindElement(By.CssSelector("td:nth-child(7)")).Text.Trim())
                       .OrderBy(x => x)
                       .ToList();
        }

        /// <summary>
        /// Sorts fares in ascending order.
        /// </summary>
        public IEnumerable<decimal> SortByFare() =>
            GetFareAmounts().OrderBy(f => f);


        // =========================================================================
        // Utilities
        // =========================================================================

        /// <summary>
        /// Retrieves "No record" message.
        /// </summary>
        public string GetNoRecordMessage() =>
            _driver.FindElement(NoRecordMessageCell).Text;
    }
}
