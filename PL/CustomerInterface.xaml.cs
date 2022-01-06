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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;
using BO;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerInterface.xaml
    /// </summary>
    public partial class CustomerInterface : Window
    {
        BlApi.Ibl bl;
        public LoginWindow lastW;
        public ObservableCollection<ParcelAtCustomer> sendParcels;
        public ObservableCollection<ParcelAtCustomer> receivedParcels;
        public ObservableCollection<ParcelToList> confirmParcels;
        //public ObservableCollection<ParcelToList> parcelToLists;
        public Customer me { get; set; }
        public Parcel parcel { get; set; }
        public int parcelId;
        public ParcelToList parcelToList { get; set; }
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
            //  SentparcelsList.ItemsSource = me.SentParcels;
            //  receivedparcelsList.ItemsSource = me.ReceivedParcels;
            confirmParcels = new();
            IEnumerable<ParcelToList> tempParcels= bl.GetAllParcels(x => ((x.ReceiverId == me.Id && x.Delivered == null && x.PickedUp!=null)|| (x.SenderId == me.Id&&x.PickedUp==null&& x.Assigned!=null))).OrderBy(p=>p.ParcelStatus);// list wist parcels that arent yet deliveres or picked up
            foreach(var parcelToList in tempParcels)
            {
                confirmParcels.Add(parcelToList);
            }
            AllParcels.ItemsSource = confirmParcels;
            sendParcels = new();
            foreach (var customerInParcel in me.SentParcels)
            {
                sendParcels.Add(customerInParcel);
            }
            SentparcelsList.ItemsSource = sendParcels;
            receivedParcels = new();
            foreach (var customerInParcel in me.ReceivedParcels)
            {
                receivedParcels.Add(customerInParcel);
            }
            SentparcelsList.ItemsSource = sendParcels;
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
                parcel.Sender = bl.GetCustomerInParcel(me.Id);
                parcel.Receiver = bl.GetCustomerInParcel(parcel.Receiver.Id);
                parcel.Created = DateTime.Now;
                //parcel.Dr = new();
                //parcel.Dr.Loc = new();
                int parcelId = bl.AddParcel(parcel);
                
                sendParcels.Add(bl.GetParcelAtCustomer(parcelId));
                MessageBox.Show(parcel.ToString(), "added Parcel");
                
               
            }
            catch (AddingException ex)
            {
                MessageBox.Show(ex.Message, "Adding issue");
            }

        }

        private void ReceivedParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeliverParcelByDrone(parcelId);
            }
            catch(DeliveryIssueException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PickedUpParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.CollectingParcelByDrone(parcelId);
            }
            catch(DeliveryIssueException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(RetrievalException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AllParcels_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            parcelToList = (ParcelToList)AllParcels.SelectedItem;
            parcel = bl.GetParcel(parcelToList.Id);
            if (parcelToList.ReceiverId == me.Id)// if I am supposed to receive the parcel
            { 
                var Result = MessageBox.Show($"Is Package{parcelToList.Id}delivered?", "DeliveryStatus", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    parcelToList.ParcelStatus = BO.ParcelStatuses.Delivered;
                    FrameworkElement framework = sender as FrameworkElement;
                    ParcelToList CurrentParcel = framework.DataContext as ParcelToList;
                    confirmParcels.Remove(parcelToList);
                    bl.DeliverParcelByDrone(parcel.Dr.Id);
                   
                }
                else if (Result == MessageBoxResult.No)
                {
                    Environment.Exit(0);
                }
            }
            else if(parcelToList.SenderId==me.Id)// if I am supposed to send the parcel
            {
                var Result = MessageBox.Show($"Is Package{parcelToList.Id}pickedup?", "PickUpStatus", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    parcelToList.ParcelStatus = BO.ParcelStatuses.PickedUp;
                    FrameworkElement framework = sender as FrameworkElement;
                    ParcelToList CurrentParcel = framework.DataContext as ParcelToList;
                    confirmParcels.Remove(parcelToList);
                    bl.CollectingParcelByDrone(parcel.Dr.Id);
                    //confirmParcels.Items.Refresh();
                }

                else if (Result == MessageBoxResult.No)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
