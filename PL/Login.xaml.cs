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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    ///

    public enum ManagerOrCustomer
    {
        Manager = 0, Customer
    }
    public partial class Login : Window
    {
        public bool ManagerCustomer { get; set; }
        BlApi.Ibl IblObj = BlApi.BlFactory.GetBl();
        bool register;


        public Login()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Manager_Click(object sender, RoutedEventArgs e)
        {
       
            ManagerCustomer = Convert.ToBoolean((int)ManagerOrCustomer.Manager);
            Label.Content = "Password";
            IdInput.Visibility = Visibility.Visible;
            PassInput.Visibility = Visibility.Collapsed;
        }

        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            ManagerCustomer = Convert.ToBoolean((int)ManagerOrCustomer.Customer);
            Label.Content = "ID";
            PassInput.Visibility = Visibility.Collapsed;
            IdInput.Visibility = Visibility.Visible;
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerCustomer == false)
            {
                while (PassInput.Password != "1234")
                {
                    MessageBox.Show("Password is incorrect", "Incorrect Password", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                //while(IblObj.customer)
                //{
                //    MessageBox.Show("Id isn't registered incorrect", "Incorrect Password", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }
            new DroneListWindow(IblObj).Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
        //    register = true;
        //   new CustomerWindow(IblObj, this).Show();
        //    this.Close();
        }
    }
}
