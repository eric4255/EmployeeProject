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
        { return Name; }

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
        [Serializable]
        sealed public class PlatinumBenefitPackage : BenefitPackage
        {
            public override double ComputePayDeduction() { return 250.0; }
            public override string ToString() { return "Platinum"; }
        }
        #endregion


        // Expose object through a custom property.

        public Employee() { }
        public Employee(string firstName, string lastName, DateTime date, float pay, string ssn)
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
                dgEmps.ItemsSource = (List<Employee>)empList.FindAll(obj => obj is Manager);
            else if (this.employeeTypeRadioButtons.SelectedIndex == 2)
                dgEmps.ItemsSource = (List<Employee>)empList.FindAll(obj => obj is SalesPerson);
            else if (this.employeeTypeRadioButtons.SelectedIndex == 3)
                dgEmps.ItemsSource = (List<Employee>)empList.FindAll(obj => obj is Engineer || obj is SupportPerson);
            else dgEmps.ItemsSource = empList;
        }
    }
}
#endregion