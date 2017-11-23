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
    public class Executive : Manager
    {
        public ExecTitle Title { get; set; } = ExecTitle.VP;


        public Executive() : base()
        {
            empBenefits = new GoldBenefitPackage();
            StockOptions = 10000;
        }
        private List<Employee> _reports = new List<Employee>();

        public Executive(string firstName, string lastName, DateTime age, float currPay,
                         string ssn, int numbOfOpts = 10000, ExecTitle title = ExecTitle.VP)
          : base(firstName, lastName, age, currPay, ssn, numbOfOpts)
        {
            // Title defined by the Executive class.
            Title = title;
            empBenefits = new GoldBenefitPackage();
        }

        public override string Role { get { return base.Role + ", " + Title; } }
        public override void SpareDetailProp1(ref string name, ref string value)
        {
            name = "Stock Options:";
            value = StockOptions.ToString();
        }

        public override void SpareDetailProp2(ref string name, ref string value)
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

        // Methods for adding/removing reports
        public override void AddReport(Employee newReport)
        {
            // Check for proper report to Executive
            if (newReport is Manager || newReport is SalesPerson)
            {
                _reports.Add(newReport);
            }
            else
            {
                Console.WriteLine("AddReport Error: {0} is not a Manager or SalesPerson, and cannot report to an Executive",
                                  newReport.Name);
            }
        }

        public override void RemoveReport(Employee emp)
        {
            // Remove report
            _reports.Remove(emp);
        }
    }
}
