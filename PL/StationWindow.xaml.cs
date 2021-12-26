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
    public partial class StationWindow : Window
    {


        BlApi.Ibl bl;
        private Station Station { get; set; }
        DroneListWindow lastW;
        bool addOrUpdate;
        public ObservableCollection<DroneInCharge> DroneChargeObservable;

        public StationWindow(BlApi.Ibl IblObj, DroneListWindow last)// constructor to add a drone
        {
            InitializeComponent();
            bl = IblObj;
            addOrUpdate = Globals.add;
            lastW = last;
            Station = new Station();
            DataContext = Station;
            UpdateGrid.Visibility = Visibility.Hidden;
        }

        public StationWindow(DroneListWindow last, BlApi.Ibl ibl) // constructor to update a drone
        {
            InitializeComponent();
            bl = ibl;
            addOrUpdate = Globals.update;
            lastW = last;
            Station = bl.GetStation(lastW.droneToList.Id);
           // droneInChargeList = new();
            List<DroneInCharge> tempDroneInCharge = bl.getAllDroneInCharge(Station.Id).Item1.ToList();
            foreach (var droneInCharge in tempDroneInCharge)
            {
                DroneChargeObservable.Add(droneInCharge);
            }
            DronesInchargeListview.ItemsSource = DroneChargeObservable;
            UpdateGrid.Visibility = Visibility.Visible; //shows  appropriate add grid for window
            DataContext = Station;
        }

        private void droneInChargeList_Click(object sender, RoutedEventArgs e)
        {
            if (DronesInchargetxt.Visibility == Visibility.Visible)
            {
                DronesInchargetxt.Visibility = Visibility.Hidden;
                DronesInchargeListview.Visibility = Visibility.Hidden;
            }
            else
            {
                DronesInchargetxt.Visibility = Visibility.Visible;
                DronesInchargeListview.Visibility = Visibility.Visible;
            }
        }
    }
}