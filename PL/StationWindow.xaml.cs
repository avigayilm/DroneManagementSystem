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
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {


        BlApi.Ibl bl;
        private Station Station { get; set; }
        List<DroneInCharge> tempDroneInCharge { get; set; }
        DroneListWindow lastW;
        public bool addOrUpdate
        {
            get { return (bool)GetValue(addOrUpdateProperty); }
            set { SetValue(addOrUpdateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for addOrUpdate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty addOrUpdateProperty =
            DependencyProperty.Register("addOrUpdate", typeof(bool), typeof(StationWindow));
        public ObservableCollection<DroneInCharge> DroneChargeObservable = new();

        public StationWindow(BlApi.Ibl IblObj, DroneListWindow last)// constructor to add a station
        {
            InitializeComponent();
            bl = IblObj;
            addOrUpdate = Globals.add;
            lastW = last;
            Station = new Station();
            DataContext = this;
            UpdateGrid.Visibility = Visibility.Hidden;
        }

        public StationWindow(DroneListWindow last, BlApi.Ibl ibl) // constructor to update a station
        {
            InitializeComponent();
            bl = ibl;
            addOrUpdate = Globals.update;
            lastW = last;
            Station = bl.GetStation(lastW.stationToList.Id);
           // droneInChargeList = new();
            tempDroneInCharge = bl.getAllDroneInCharge(Station.Id).Item1.ToList();
            if (tempDroneInCharge.Count != 0)
            {
                foreach (var droneInCharge in tempDroneInCharge)
                {
                    DroneChargeObservable.Add(droneInCharge);
                }
                DronesInchargeListview.ItemsSource = DroneChargeObservable;
            }
            UpdateGrid.Visibility = Visibility.Visible; //shows  appropriate add grid for window
            DataContext = this;
        }

        private void droneInChargeList_Click(object sender, RoutedEventArgs e)
        {
            if (DronesInchargetxt.Visibility == Visibility.Visible)// if it's already visible
            {
                DronesInchargetxt.Visibility = Visibility.Hidden;
                DronesInchargeListview.Visibility = Visibility.Hidden;
            }
            else// if it's hidden
            {
                if (tempDroneInCharge.Count == 0)
                {
                    MessageBox.Show("No Drones in Charge");
                }
                else
                {
                    DronesInchargetxt.Visibility = Visibility.Visible;
                    DronesInchargeListview.Visibility = Visibility.Visible;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateorAddButton_Click(object sender, RoutedEventArgs e)
        {
            if(addOrUpdate==Globals.add)// if we add
            {
                try
                {
                
                    bl.AddStation(Station);
                    MessageBox.Show(Station.ToString(), "added station");
                    if(lastW.stationToLists.ContainsKey(Station.AvailableChargeSlots))
                        lastW.stationToLists[Station.AvailableChargeSlots].Add(bl.GetAllStation(c => c.Id == Station.Id).Single());
                    else
                    {
                        lastW.stationToLists.Add(Station.AvailableChargeSlots, bl.GetAllStation(s => s.Id == Station.Id).ToList());
                    }
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
                    bl.UpdateStation(Station.Id, Station.Name, Station.AvailableChargeSlots);
                    MessageBox.Show(bl.GetStation(Station.Id).ToString(), "Updated Station");
                    lastW.stationToList.Name = Station.Name;
                    lastW.stationToList.AvailableChargeSlots = Station.AvailableChargeSlots;
                    lastW.StationListView.Items.Refresh();
                    this.Close();
                }
                catch (UpdateIssueException ex)
                {
                    MessageBox.Show(ex.Message, "UpdateIssue", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}