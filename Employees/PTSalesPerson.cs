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
    sealed class PTSalesPerson : SalesPerson
    {
        public PTSalesPerson(string firstName, string lastName, DateTime age,
                             float currPay, string ssn, int numbOfSales)
          : base(firstName, lastName, age, currPay, ssn, numbOfSales)
        {
        }
        public override void SpareDetailProp1(ref string name, ref string value)
        {
            name = "Sales Number:";
            value = SalesNumber.ToString();
        }
    }
}
