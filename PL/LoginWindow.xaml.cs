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
        BlApi.Ibl bl;
        string username;
        string password;
        bool userType;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
           // bl.Register();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                password = textPass.Password;
                userType=bl.Login(username, password);
                
            }
            catch(BO.LoginBLException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
