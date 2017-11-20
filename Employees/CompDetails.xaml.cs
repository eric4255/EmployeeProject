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
                string nameStock = "";
                string valueStock= "";
                string nameReport = "";
                string valueReport = "";

                emp.GetSpareProp1(ref nameStock, ref valueStock);
                this.SpareProp1Name.Content = nameStock;
                this.SpareProp1Value.Content = valueStock;
                SpareProp1.Visibility = Visibility.Visible;


                emp.GetSpareProp2(ref nameReport, ref valueReport);
                this.SpareProp2Name.Content = nameReport;
                this.SpareProp2Value.Content = valueReport;
                SpareProp2.Visibility = Visibility.Visible;
            }
        }
    }
}
