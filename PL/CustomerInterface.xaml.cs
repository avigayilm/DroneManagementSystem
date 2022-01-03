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
using System.Net.Mail;
using System.Net;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerInterface.xaml
    /// </summary>
    public partial class CustomerInterface : Window
    {
        BlApi.Ibl bl;
        LoginWindow lastW;
        Customer me;
        Parcel parcel;
        public CustomerInterface(BlApi.Ibl IblObj, LoginWindow last)
        {
            bl = IblObj;
            InitializeComponent();
            weiCBx.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            prioCbx.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            recIdCBx.ItemsSource = bl.GetAllCustomers().Select(c => c.Id);
            lastW = last;
            me=bl.GetCustomer(lastW.userName);
            parcel = new();
            parcel.Sender = new();
            parcel.Receiver = new();
            DataContext = this;
            SentparcelsList.ItemsSource = me.SentParcels;
            receivedparcelsList.ItemsSource = me.ReceivedParcels;
        }

        private void Email()
        {
            MailMessage email = new()
            {
                From = new MailAddress(@"deliveriesskyhigh@gmail.com"),
                Body = msgTxt.Text,
                Subject = nameTxt.Text,
            };
            email.To.Add(@"deliveriesskyhigh@gmail.com");
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(@"deliveriesskyhigh@gmail.com", "dronesskyhi"),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtp.SendCompleted += Smtp_SendCompleted;
            smtp.SendMailAsync(email);
            return;
        }

        private void Smtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Email();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddParcel(parcel);
                MessageBox.Show(parcel.ToString(), "added Parcel");
                this.Close();
            }
            catch (AddingException ex)
            {
                MessageBox.Show(ex.Message, "Adding issue");
            }

        }
    }
}
