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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    /// 
    public partial class LoginWindow : Window
    {
        BlApi.Ibl IblObj = BlApi.BlFactory.GetBl();
        public string userName 
        {
            get { return (string)GetValue(userNameProperty); }
            set { SetValue(userNameProperty, value); }
        }
        public static readonly DependencyProperty userNameProperty =
            DependencyProperty.Register("userName", typeof(string), typeof(LoginWindow));
        string password;
        bool userType;
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;
           // Closing += LoginWindow_Closing;
        }

        //private void LoginWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    IblObj.changechargeSlots();
            
        //}

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            new CustomerInterface( this, IblObj).Show();
           // this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                password = textPass.Password;
                userType = IblObj.Login(userName, password);
                if (userName == "Manager")
                {
                    new DroneListWindow(IblObj).Show();
                    textPass.Clear();
                    txtusername.Clear();
                   // this.Close();
                }
                else
                {
                    new CustomerInterface(IblObj, this).Show();
                    txtusername.Text = "";
                    textPass.Clear();
                }
                

            }
            catch(BO.LoginBLException ex)
            {
                MessageBox.Show(ex.Message,"LoginIssue", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
