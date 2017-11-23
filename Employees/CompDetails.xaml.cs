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

namespace Employees
{
    /// <summary>
    /// Interaction logic for CompDetails.xaml
    /// </summary>
    public partial class CompDetails : Page
    {
        public CompDetails()
        {
            InitializeComponent();
        }

        public CompDetails(Object data) : this()
        {
            this.DataContext = data;

            if (data is Employee)
            {
                Employee emp = (Employee)data;
                string name1 = "";
                string value1 = "";
                string name2 = "";
                string value2 = "";

                emp.SpareDetailProp1(ref name1, ref value1);
                emp.SpareDetailProp2(ref name2, ref value2);

                SpareProp1Name.Content = name1;
                SpareProp1Value.Content = value1;
                SpareProp2Name.Content = name2;
                SpareProp2Value.Content = value2;
            }
        }
    }
}
