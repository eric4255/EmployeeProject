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
    public class SupportPerson : Employee
    {
        public ShiftName Shift { get; set; } = ShiftName.One;

        #region constructors 
        public SupportPerson() { }

        public SupportPerson(string firstName, string lastName, DateTime age,
                             float currPay, string ssn, ShiftName shift)
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

        public override void SpareDetailProp1(ref string name, ref string value)
        {
            name = "Shift:";
            value = Shift.ToString();
        }
    }
}
