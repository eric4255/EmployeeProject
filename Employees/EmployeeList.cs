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
    public class EmployeeList : List<Employee>
    {
        static BinaryFormatter binFormat = new BinaryFormatter();

        string filename = "Employees.dat";

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

            return new List<Employee>() { dan, connie, chucky, mary, bob, fran, sam, sally, mike, steve };
            //return new List<Employee>() { dan, connie, chucky };
        }


        public EmployeeList(string filename)
        {
            this.filename = filename;
            File.Delete(filename);

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
}
