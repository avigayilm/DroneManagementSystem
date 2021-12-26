using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using System.Windows.Shapes;
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {


        BlApi.Ibl bl;

        private Customer Customer { get; set; }
        DroneListWindow lastW;
        bool addOrUpdate;

        public CustomerWindow(BlApi.Ibl IblObj, DroneListWindow last)// constructor to add a drone
        {
            InitializeComponent();
            bl = IblObj;
            addOrUpdate = Globals.add;
            lastW = last;
            Customer = new Customer();
            DataContext = Customer;
        }

        public CustomerWindow(DroneListWindow last, BlApi.Ibl ibl) // constructor to update a drone
        {
            InitializeComponent();
            bl = ibl;
            addOrUpdate = Globals.update;
            lastW = last;
           // Customer = bl.GetCustomer(lastW.droneToList.Id);
            UpdateGrid.Visibility = Visibility.Visible; //shows  appropriate add grid for window
            DataContext = Customer;
        }

        private void profile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new();
            op.Title = "select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
        "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                profile.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
    }
}
