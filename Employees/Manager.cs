using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;

namespace Employees
{
    [Serializable]
    public class Manager : Employee, IEnumerable<Employee>
    {
        #region constructors 
        public Manager() { }

        public Manager(string firstName, string lastName, DateTime age,
                       float currPay, string ssn, int numbOfOpts)
          : base(firstName, lastName, age, currPay, ssn)
        {
            // This property is defined by the Manager class.
            StockOptions = numbOfOpts;
        }
        #endregion

        #region Constants, data members and properties
        // Add a private member for reports
        public const int MaxReports = 5;
        private List<Employee> _reports = new List<Employee>();

        // Stock options unique to Managers
        public int StockOptions { get; set; }

        #endregion
        public override void GetSpareProp1(ref string name, ref string value)
        {
            name = "Stock Options:";
            value = StockOptions.ToString();
        }

        public override void GetSpareProp2(ref string name, ref string value)
        {
            name = "Reports:";
            value = reports();
        }

        private string reports()
        {
            string temp = "";
            foreach (Employee report in _reports)
            {
                temp += string.Format("{0}, ", report.GetName());
            }
            if (temp.Length > 0)
                return temp.Remove(temp.Length - 2);
            else
                return temp;
        }


        #region Exceptions
        // Exception raised when adding more than MaxReports to a Manager
        [System.Serializable]
        public class AddReportException : ApplicationException
        {
            // Standard exception constructors
            public AddReportException() { }
            public AddReportException(string message)
                : base(message) { }
            public AddReportException(string message, Exception inner)
                : base(message, inner) { }
            protected AddReportException(System.Runtime.Serialization.SerializationInfo info,
                                  System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
        #endregion

        #region Class Methods
        // Enumerate reports for Manager
        public IEnumerator<Employee> GetEnumerator() { return _reports.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        // Enumerate reports, sorted by Employee Name, Age, and Pay
        public IEnumerable<Employee> ReportsByName() { return GetReports(Employee.SortByName); }
        public IEnumerable<Employee> ReportsByAge() { return GetReports(Employee.SortByAge); }
        public IEnumerable<Employee> ReportsByPay() { return GetReports(Employee.SortByPay); }

        // Enumerator to return reports in passed sort order 
        // (null indicating no sort)
        private IEnumerable<Employee> GetReports(IComparer<Employee> sortOrder = null)
        {
            // Sort reports if sort order non-null
            if (sortOrder != null) _reports.Sort(sortOrder);

            // Enumerate reports in specified order
            return this;
        }

        // Methods for adding/removing reports
        public virtual void AddReport(Employee newReport)
        {
            // Local to hold error string, if found
            string errorString = null;

            // Check number of reports
            if (_reports.Count >= MaxReports)
                errorString = string.Format("Manager already has {0} reports.", MaxReports);
            else if (_reports.IndexOf(newReport) >= 0)
                errorString = "Employee already reports to manager";
            else if (this == newReport)
                errorString = "Manager can not report to himself/herself";

            // Create an exception if we found an error
            if (errorString != null)
            {
                Exception ex = new AddReportException(errorString);

                // Add Manager custom data dictionary
                ex.Data.Add("Manager", this.Name);

                // Also add report that failed to be added, and throw exception
                ex.Data.Add("New Report", newReport.Name);
                throw ex;

            }

            // Only add report if not already a report and not same as this
            else
            {
                // Put Employee in empty position
                _reports.Add(newReport);
            }
        }

        public virtual void RemoveReport(Employee emp)
        {
            // Remove report
            _reports.Remove(emp);
        }

        // Display Manager with stock options and list of reports
        public override void DisplayStats()
        {
            base.DisplayStats();
            Console.WriteLine("Stock Options: {0:N0}", StockOptions);

            // Print out reports on a single line
            Console.Write("Reports: ");
            foreach (Employee emp in this)
                Console.Write("{0} ", emp.Name);
            Console.WriteLine();
        }
        #endregion
    }
}

