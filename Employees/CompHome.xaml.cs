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

    // Engineers have degrees
    [Serializable]
    public enum DegreeName { BS, MS, PhD }
    [Serializable]
    public enum ExecTitle { CEO, CTO, CFO, VP }
    [Serializable]
    public enum ShiftName { One, Two, Three }
    [Serializable]


    public class Employee
    {


        // Field data.
        public string Name { get { return FirstName + " " + LastName; } }

        static private int empID = 1;
        private int eID;
        private float currPay;
        private DateTime empDOB;
        private string empSSN;
        DateTime today = DateTime.Now;
        protected BenefitPackage empBenefits = new BenefitPackage();

        #region Properties 
        public string FirstName { get; }
        public string LastName { get; }
        public int ID { get { return eID; } }
        public float Pay { get { return currPay; } }
        public int Age { get { return today.Year - empDOB.Year; } }
        public DateTime DateOfBirth { get { return empDOB; } }
        public string SocialSecurityNumber { get { return empSSN; } }
        public string embBenefitPackage { get { return empBenefits.ToString(); } }
        public virtual string Role { get { return GetType().ToString().Substring(10); } }
        public string GetName()
        { return FirstName; }

        // Expose object through a read-only property.
        public BenefitPackage Benefits
        {
            get { return empBenefits; }
        }
        #endregion
        
        #region Serialization customization for NextId
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // Called when the deserialization process is complete.
            if (eID > empID) empID = eID + 1;
        }
        #endregion


        // Contain a BenefitPackage object.
        public double GetBenefitCost()
        { return empBenefits.ComputePayDeduction(); }

        public virtual void GivePromotion()
        {
            // Bump benefit package from Standard to Gold, Gold to Platinum
            if (empBenefits.GetType() == typeof(BenefitPackage))
                empBenefits = new GoldBenefitPackage();
            else if (empBenefits is GoldBenefitPackage)
                empBenefits = new PlatinumBenefitPackage();
        }
        #region
        [Serializable]
        public class BenefitPackage
        {
            // Assume we have other members that represent
            // dental/health benefits, and so on.
            
            public override string ToString() { return "Standard"; }
            public virtual double ComputePayDeduction() { return 125.0; }
        }

        // Other benefit packages derive from BenefitPackage directly
        // and provide definitions for ComputePayDeduction and ToString
        [Serializable]
        sealed public class GoldBenefitPackage : BenefitPackage
        {
            public override double ComputePayDeduction() { return 150.0; }
            public override string ToString() { return "Gold"; }
        }
        sealed public class PlatinumBenefitPackage : BenefitPackage
        {
            public override double ComputePayDeduction() { return 250.0; }
            public override string ToString() { return "Platinum"; }
        }
        #endregion


        // Expose object through a custom property.

        public Employee() { }
        public Employee(string firstName, string lastName, DateTime date, float pay,  string ssn)
        {
            int id = empID;//provides unique id for each employee
            this.eID = id;
            empDOB = date;
            empSSN = ssn;
            empID++;
            FirstName = firstName;
            LastName = lastName;
        }

        public virtual void GetSpareProp1(ref string name, ref string value) { }
        public virtual void GetSpareProp2(ref string name, ref string value) { }
        public virtual void GetSpareProp3(ref string name, ref string value) { }
        public virtual void GetSpareProp4(ref string name, ref string value) { }
        public virtual void GetSpareProp5(ref string name, ref string value) { }


        #region Employee sort oders
        // Sort employees by name.
        [Serializable]
        private class NameComparer : IComparer<Employee>
        {
            // Compare the name of each object.
            int IComparer<Employee>.Compare(Employee e1, Employee e2)
            {
                if (e1 != null && e2 != null)
                    return String.Compare(e1.Name, e2.Name);
                else throw new ArgumentException("Parameter is not an Employee!");
            }
        }

        // Sort by age
        [Serializable]
        private class AgeComparer : IComparer<Employee>
        {
            // Compare the Age of each object.
            int IComparer<Employee>.Compare(Employee e1, Employee e2)
            {
                if (e1 != null && e2 != null)
                    return e1.Age.CompareTo(e2.Age);
                else throw new ArgumentException("Parameter is not an Employee!");
            }
        }

        // Sort By pay
        [Serializable]
        private class PayComparer : IComparer<Employee>
        {
            // Compare the Pay of each object.
            int IComparer<Employee>.Compare(Employee e1, Employee e2)
            {
                if (e1 != null && e2 != null)
                    return e1.Pay.CompareTo(e2.Pay);
                else
                    throw new ArgumentException("Parameter is not an Employee!");
            }
        }
#endregion
        public virtual void DisplayStats()
        {
            Console.WriteLine("Name: {0}", Name);
            Console.WriteLine("Role: {0}", GetType().ToString().Substring(10));
            Console.WriteLine("ID: {0}", ID);
            Console.WriteLine("Age: {0}", Age);
            Console.WriteLine("Pay: {0:C}", Pay);
            Console.WriteLine("SSN: {0}", SocialSecurityNumber);
            Console.WriteLine("Benefits: {0}", empBenefits);
        }
        #region Class methods 
        public virtual void GiveBonus(float amount)
        { currPay += amount; }

        // Static, read-only properties to return Employee Name, Age, or Pay comparers
        public static IComparer<Employee> SortByName { get; } = new NameComparer();
        public static IComparer<Employee> SortByAge { get; } = new AgeComparer();
        public static IComparer<Employee> SortByPay { get; } = new PayComparer();
    }
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
        public override void GetSpareProp1(ref string name, ref string value)
        {
            name = "Stock Options:";
            value = StockOptions.ToString();
        }


        public override void GetSpareProp2(ref string name, ref string value)
        {
            name = "Reports:";
            foreach (Employee report in _reports)
            {
                //value = report.GetName();
                value += report.GetName() + " ";
            }
        }
    }

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
            foreach (Employee report in _reports)
            {
                //value = report.GetName();
                value+= report.GetName() +" ";
            } 
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
    [Serializable]
    sealed class PTSalesPerson : SalesPerson
    {
        public PTSalesPerson(string firstName, string lastName, DateTime age,
                             float currPay,  string ssn, int numbOfSales)
          : base(firstName, lastName, age, currPay, ssn, numbOfSales)
        {
        }
        public override void GetSpareProp4(ref string name, ref string value)
        {
            name = "Number of Sales:";
            value = SalesNumber.ToString();
        }
    }
    [Serializable]
    public class Engineer : Employee
    {
        public DegreeName HighestDegree { get; set; } = DegreeName.BS;

        #region constructors 
        public Engineer() { }

        public Engineer(string firstName, string lastName, DateTime age,
                       float currPay,  string ssn, DegreeName degree)
          : base(firstName, lastName, age, currPay,  ssn)
        {
            // This property is defined by the Engineer class.
            HighestDegree = degree;
        }
        #endregion
        public override string Role { get { return base.Role; } }

        public override void GetSpareProp3(ref string name, ref string value)
        {
            name = "Degree:";
            value = HighestDegree.ToString();
        }
    }
    [Serializable]
    class SalesPerson : Employee
    {
        #region constructors 
        public SalesPerson() { }

        // As a general rule, all subclasses should explicitly call an appropriate
        // base class constructor.
        public SalesPerson(string firstName, string lastName, DateTime age,
          float currPay,  string ssn, int numbOfSales)
          : base(firstName, lastName, age, currPay, ssn)
        {
            // This belongs with us!
            SalesNumber = numbOfSales;
        }
        #endregion

        public int SalesNumber { get; set; }

        // A salesperson's bonus is influenced by the number of sales.
        public override sealed void GiveBonus(float amount)
        {
            int salesBonus = 0;
            if (SalesNumber >= 0 && SalesNumber <= 100)
                salesBonus = 10;
            else
            {
                if (SalesNumber >= 101 && SalesNumber <= 200)
                    salesBonus = 15;
                else
                    salesBonus = 20;
            }
            base.GiveBonus(amount * salesBonus);
        }

        // A SalesPerson gets an extra 300 on promotion
        public override sealed void GivePromotion()
        {
            base.GivePromotion();
            GiveBonus(300);
        }

        public override void DisplayStats()
        {
            base.DisplayStats();
            Console.WriteLine("Number of sales: {0:N0}", SalesNumber);
        }

        public override void GetSpareProp4(ref string name, ref string value)
        {
            name = "Number of Sales:";
            value = SalesNumber.ToString();
        }
    }

    [Serializable]
    public class SupportPerson : Employee
    {
        public ShiftName Shift { get; set; } = ShiftName.One;

        #region constructors 
        public SupportPerson() { }

        public SupportPerson(string firstName,string lastName, DateTime age,
                             float currPay,  string ssn, ShiftName shift)
          : base(firstName, lastName, age, currPay, ssn)
        {
            // This property is defined by the SupportPerson class.
            Shift = shift;
        }
        #endregion

        public override void DisplayStats()
        {
            base.DisplayStats();
            Console.WriteLine("Shift: {0}", Shift);
        }

        public override void GetSpareProp5(ref string name, ref string value)
        {
            name = "Shift:";
            value = Shift.ToString();
        }
    }
    //change the EmployeeList class to try to load Employees from an 
    //Employees.dat file (if present) or use the Employees created in 
    //the method InitialEmployees from assignment 7, and then save the 
    //Employees to the file Employees.dat so that the next run of your 
    //program will pick up the Employees saved in Employees.dat.


    [Serializable]
    public class EmployeeList : List<Employee>
    {
        static BinaryFormatter binFormat = new BinaryFormatter();

        string filename="Employees.dat";

        static List<Employee> InitialEmployees()
        {
            Executive dan = new Executive("Dan", "Doe", DateTime.Parse("3/20/1963"), 200000, "121-12-1211", 50000, ExecTitle.CEO);
            Executive connie = new Executive("Connie", "Chung", DateTime.Parse("2/5/1972"), 150000, "229-67-7898", 40000, ExecTitle.CFO);
            Manager chucky = new Manager("Chucky", "Kirkland", DateTime.Parse("4/23/1967"), 100000, "333-23-2322", 9000);
            Manager mary = new Manager("Mary", "Tompson", DateTime.Parse("5/9/1963"), 200000, "121-12-1211", 9500);
            Engineer bob = new Engineer("Bob", "Pirs", DateTime.Parse("6/30/1986"), 120000, "334-24-2422", DegreeName.MS);
            SalesPerson fran = new SalesPerson("Fran", "Smith", DateTime.Parse("7/5/1975"), 80000, "932-32-3232", 31);
            PTSalesPerson sam = new PTSalesPerson("Sam", "Abbot", DateTime.Parse("8/11/1984"), 20000, "525-76-5030", 20);
            PTSalesPerson sally = new PTSalesPerson("Sally", "McRae", DateTime.Parse("9/12/1979"), 30000, "913-43-4343", 10);
            SupportPerson mike = new SupportPerson("Mike", "Roberts", DateTime.Parse("10/31/1975"), 15000, "229-67-7898", ShiftName.One);
            SupportPerson steve = new SupportPerson("Steve", "Kinny", DateTime.Parse("11/21/1982"), 80000, "913-43-4343", ShiftName.Two);


            // Bonuses and promotions
            dan.GiveBonus(1000);
            bob.GiveBonus(500);
            sally.GiveBonus(400);
            dan.GivePromotion();
            chucky.GivePromotion();
            fran.GivePromotion();


            // Add reports - just report error and continue on exception
            try
            {
                dan.AddReport(chucky);
                dan.AddReport(mary);
                connie.AddReport(fran);
                connie.AddReport(sally);
                mary.AddReport(sam);
                mary.AddReport(mike);
                chucky.AddReport(bob);
                chucky.AddReport(steve);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when adding reports: {e.Message}");
            }

            Console.Write("\nReports by Age: ");

            return new List<Employee>() { dan, connie, chucky, mary, bob, fran,
                                          sam, sally, mike, steve};
        }


        public EmployeeList(string filename)
        {
            this.filename = filename;

            if (File.Exists(filename))
            {
                AddRange(LoadEmployeesFromBinaryFile(filename));
            }
            else
            {
                List<Employee> emps = InitialEmployees();
                SaveEmployeesAsBinary(filename, emps);
                AddRange(emps);
            }


        }

        #region Save/Load LIST of Employees
        static List<Employee> LoadEmployeesFromBinaryFile(string fileName)
        {
            using (Stream fStream = File.OpenRead(fileName))
            {

                return (List<Employee>)binFormat.Deserialize(fStream);
            }
        }


        static void SaveEmployeesAsBinary(string fileName, List<Employee> emps)
        {
            using (Stream fStream = new FileStream(fileName,
              FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                binFormat.Serialize(fStream, emps);
            }
        }

        #endregion
    }

    [Serializable]
    public partial class CompHome : Page
    {
        const string filename = "Employees.dat";
        static EmployeeList empList = new EmployeeList(filename);

        public CompHome()
        {

            InitializeComponent();


            // Select the All radio button
            this.employeeTypeRadioButtons.SelectedIndex = 0;

            // Set event handler for radio button changes
            this.employeeTypeRadioButtons.SelectionChanged += new SelectionChangedEventHandler(employeeTypeRadioButtons_SelectionChanged);

            // Fill the Employees data grid
            dgEmps.ItemsSource = empList;
        }

        private void GoBackButton(object sender, RoutedEventArgs e)
        {
            // Add the following line of code.    
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            // Show Employee details if one selected
            if (dgEmps.SelectedIndex >= 0)
            {
                this.NavigationService.Navigate(new CompDetails(this.dgEmps.SelectedItem));
            }
        }
        // Handle changes to Employee type radio buttons
        void employeeTypeRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Only choices are All (0) or Executives (1)
            if (this.employeeTypeRadioButtons.SelectedIndex == 1)
                dgEmps.ItemsSource = (List<Employee>)empList.FindAll(obj => obj is Executive||obj  is Manager);
            else if(this.employeeTypeRadioButtons.SelectedIndex == 2)
                dgEmps.ItemsSource = (List<Employee>)empList.FindAll(obj => obj is SalesPerson|| obj is PTSalesPerson);
            else if (this.employeeTypeRadioButtons.SelectedIndex == 3)
                dgEmps.ItemsSource = (List<Employee>)empList.FindAll(obj => obj is Engineer || obj is SupportPerson);
            else dgEmps.ItemsSource = empList;
        }
    }
}
#endregion