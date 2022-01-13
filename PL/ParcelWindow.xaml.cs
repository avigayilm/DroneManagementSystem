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
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        public DateTime? created { get; set; }
        public Parcel parcel { get; set; }
        DroneListWindow lastW;
        BlApi.Ibl bl;


        public bool addOrUpdate
        {
            get { return (bool)GetValue(addOrUpdateProperty); }
            set { SetValue(addOrUpdateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for addOrUpdate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty addOrUpdateProperty =
            DependencyProperty.Register("addOrUpdate", typeof(bool), typeof(ParcelWindow));


        //bool addOrUpdate { get; set; }
        public ParcelWindow(BlApi.Ibl IblObj, DroneListWindow last) // to add
        {
            InitializeComponent();
            created = DateTime.Now;
            bl = IblObj;
            lastW = last;
            parcel = new();
            parcel.Sender = new();
            parcel.Receiver = new();
            DataContext = this;
            addOrUpdate = Globals.add;
            weiCBx.ItemsSource  = Enum.GetValues(typeof(BO.WeightCategories));
            prioCbx.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            sendIdCBx.ItemsSource = bl.GetAllCustomers().Select(c => c.Id);
            recIdCBx.ItemsSource = bl.GetAllCustomers().Select(c => c.Id);
        }

        public ParcelWindow( DroneListWindow last, BlApi.Ibl IblObj) // to update
        {
            InitializeComponent();
            
            bl = IblObj;
            lastW = last;
            parcel = bl.GetParcel(lastW.parcelToList.Id);
            created = parcel.Created;
            weiCBx.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            prioCbx.ItemsSource = Enum.GetValues(typeof(Priorities));
            recIdCBx.ItemsSource = bl.GetAllCustomers().Select(c => c.Id);
            UpdateGrid.Visibility = Visibility.Visible;
            DataContext = this;
            addOrUpdate = Globals.update;

        }

        public ParcelWindow(CustomerWindow last, BlApi.Ibl IblObj) // to update
        {
            InitializeComponent();

            bl = IblObj;
            parcel = bl.GetParcel(last.senderReceiver.Id);
            created = parcel.Created;
            weiCBx.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            prioCbx.ItemsSource = Enum.GetValues(typeof(Priorities));
            recIdCBx.ItemsSource = bl.GetAllCustomers().Select(c => c.Id);
            UpdateGrid.Visibility = Visibility.Visible;
            DataContext = this;
            addOrUpdate = Globals.update;

        }
        private void AddParcel()
        {
            //StationId = (int)sTCBAdd.SelectedItem; //receive station id from combobox selection
            //DroneLabel.Content = $"adding drone to the list";
            int id = bl.AddParcel(parcel);
            //wAndS.Status = (DroneStatuses)Drone.Status;
            // wAndS.Weight = (WeightCategories)Drone.Weight;
            if (lastW.parcelToLists.ContainsKey(parcel.Sender.Id))
                lastW.parcelToLists[parcel.Sender.Id].Add(bl.GetAllParcels(x => x.Id == id).First());
            else
            {
                lastW.parcelToLists.Add(parcel.Sender.Id, bl.GetAllParcels(x => x.Id == id).ToList());
            }
            lastW.CheckComboBoxesParcel();
            // after drone is updated in bl now updates listview
        }

   

        private void clickedReceiver(object sender, MouseButtonEventArgs e)
        {
            recIdCBx.Visibility = Visibility.Visible;
            ReceiverTblk.Visibility = Visibility.Collapsed;
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void sendTblk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            sendIdCBx.Visibility = Visibility.Visible;
            SenderTblk.Visibility = Visibility.Collapsed;
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (addOrUpdate == Globals.add)
                try
                {
                    AddParcel();
                    MessageBox.Show(parcel.ToString(), "added Parcel");
                    this.Close();
                }
                catch (AddingException ex)
                {
                    MessageBox.Show(ex.Message, "Adding issue");
                }
            else //update was clicked
            {
                try
                {
                    bl.UpdateParcel(parcel.Id, parcel.Receiver.Id);
                    MessageBox.Show(bl.GetParcel(parcel.Id).ToString(), "Updated parcel");
                    lastW.parcelToList.ReceiverId = parcel.Receiver.Id;
                    lastW.CheckComboBoxesParcel();
                    lastW.ParcelListView.Items.Refresh();
                    this.Close();
                }
                catch (UpdateIssueException ex)
                {
                    MessageBox.Show(ex.Message, "Updating issue");
                }
            }
        }
    }
}
