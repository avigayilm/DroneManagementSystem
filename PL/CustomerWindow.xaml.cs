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

        public Customer Customer { get; set; }
        DroneListWindow lastW;
        LoginWindow lastLogin;
        public bool addOrUpdate { get; set; }
        string imgSrc { get; set; }
        //public bool addOrUpdate
        //{
        //    get { return (bool)GetValue(addOrUpdateProperty); }
        //    set { SetValue(addOrUpdateProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for addOrUpdate.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty addOrUpdateProperty =
        //    DependencyProperty.Register("addOrUpdate", typeof(bool), typeof(StationWindow));
        //public IEnumerable<ParcelAtCustomer> senderParcelsObserverable ;
        //public IEnumerable<ParcelAtCustomer> receiverParcelsObserverable ;
        //List<ParcelAtCustomer> tempSenderParcels { get; set; }
        //List<ParcelAtCustomer> tempReceiverParcels { get; set; }

        public CustomerWindow(BlApi.Ibl IblObj, DroneListWindow last)// constructor to add a Customer
        {
            InitializeComponent();
            bl = IblObj;
            addOrUpdate = Globals.add;
            lastW = last;
            Customer = new Customer();
            Customer.Loc = new();
            imgSrc = @"C:\Users\Hudis\source\repos\HudiF\DotNet5782_9033_6996\PL\Icons\Profile.png";
            DataContext = this;
            UpdateGrid.Visibility = Visibility.Hidden;
        }

        //public CustomerWindow(BlApi.Ibl IblObj, LoginWindow last)// constructor to register a Customer
        //{
        //    InitializeComponent();
        //    bl = IblObj;
        //    addOrUpdate = Globals.add;
        //    Customer = new Customer(); 
        //    Customer.Loc = new();
        //    imgSrc = bl.getPic()
        //    DataContext = this;
        //    idTbx.Text = last.userName;
        //    idTbx.IsReadOnly = true;
        //    UpdateGrid.Visibility = Visibility.Hidden;
        //    UpdateorAddButton.Content = "Register";
        //}
        public CustomerWindow(DroneListWindow last, BlApi.Ibl ibl) // constructor to update a customer
        {
            InitializeComponent();
            bl = ibl;
            addOrUpdate = Globals.update;
            lastW = last;
            Customer = bl.GetCustomer(lastW.customerToList.Id);
            UpdateGrid.Visibility = Visibility.Visible; //shows  appropriate add grid for window
            try {
                imgSrc = bl.getPic(Customer.Id);
            }
            catch(Exception ex)
            {
                imgSrc = @"C:\Users\Hudis\source\repos\HudiF\DotNet5782_9033_6996\PL\Icons\Profile.png";
            }
            imgSrc = bl.getPic(Customer.Id);
            DataContext = this;

            //senderParcelsObserverable = from item in Customer.SentParcels
            //                            let temp = GetParcelAtCustomer(item.Id)//droneBL.FirstOrDefault(curDrone => curDrone.Id == item.DroneId)
            //                            select temp;

            //tempSenderParcels = Customer.SentParcels;
            //tempReceiverParcels = Customer.ReceivedParcels;
            //if (tempSenderParcels.Count != 0)
            //{
            //    foreach (var senderParcel in tempSenderParcels)
            //    {
            //        senderParcelsObserverable.Add(senderParcel);
            //    }
               SentparcelsList.ItemsSource = Customer.SentParcels;
            //}
            //if (tempReceiverParcels.Count != 0)
            //{
            //    foreach (var receiverParcel in tempReceiverParcels)
            //    {
            //        receiverParcelsObserverable.Add(receiverParcel);
            //    }
                receivedparcelsList.ItemsSource = Customer.ReceivedParcels;// receiverParcelsObserverable;
           // }

            UpdateGrid.Visibility = Visibility.Visible; //shows  appropriate add grid for window
        }

       

        private void sentButton_Click(object sender, RoutedEventArgs e)
        {
            if (sentList.Visibility == Visibility.Visible)// if it's already visible
            {
                sentList.Visibility = Visibility.Hidden;
                SentparcelsList.Visibility = Visibility.Hidden;
            }
            else// if it's hidden
            {
                if (Customer.SentParcels.Count() == 0)
                {
                    MessageBox.Show($"{Customer.Name} hasn't sent any parcels","SentParcels", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    sentList.Visibility = Visibility.Visible;
                    SentparcelsList.Visibility = Visibility.Visible;
                }
            }
        }

        private void Receivedbutton_Click(object sender, RoutedEventArgs e)
        {
            if (receivedList.Visibility == Visibility.Visible)// if it's already visible
            {
                receivedList.Visibility = Visibility.Hidden;
                receivedparcelsList.Visibility = Visibility.Hidden;
            }
            else// if it's hidden
            {
                if (Customer.ReceivedParcels.Count() == 0)
                {
                    MessageBox.Show($"{Customer.Name} hasn't received any parcels","ReceivedParcels", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    receivedList.Visibility = Visibility.Visible;
                    receivedparcelsList.Visibility = Visibility.Visible;
                }
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateorAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (addOrUpdate == Globals.add)// if we add
            {
                try
                {

                    bl.AddCustomer(Customer);
                    MessageBox.Show(Customer.ToString(), "added Customer");
                    lastW.customerToLists.Add(bl.GetAllCustomers(c => c.Id == Customer.Id).Single());
                    lastW.CustomerListView.Items.Refresh();
                    this.Close();
                }
                catch (AddingException ex)
                {
                    MessageBox.Show(ex.Message, "Adding issue");
                }
            }
            else// if we update
            {
                try
                {
                    bl.UpdateCustomer(Customer.Id, Customer.Name, Customer.PhoneNumber);
                    MessageBox.Show(bl.GetCustomer(Customer.Id).ToString(), "Updated Customer");
                    lastW.customerToList.Name = Customer.Name;
                    lastW.customerToList.PhoneNumber = Customer.PhoneNumber;
                    lastW.CustomerListView.Items.Refresh();
                    this.Close();
                }
                catch (UpdateIssueException ex)
                {
                    MessageBox.Show(ex.Message, "UpdateIssue", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void profile_MouseEnter(object sender, MouseEventArgs e)
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
