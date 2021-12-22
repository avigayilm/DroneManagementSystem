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
using IBL.BO;

namespace PL
{
    public enum DroneStatuses
    {
        Available, Maintenance, Delivery, All
    }

    public enum WeightCategories
    {
        Light, Medium, Heavy, All
    }

    public enum Priorities
    {
        Normal , Fast, Emergency, All
    }
    public enum ParcelStatuses
    {
        Created, Assigned, PickedUp, Delivered,All
    }
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow
    {
        IBL.Ibl bl;
        public ObservableCollection<DroneToList> droneToLists;
        public ObservableCollection<ParcelToList> parcelToLists;
        public ObservableCollection<CustomerToList> customerToLists;
        public ObservableCollection<StationToList> stationToLists;
        public DroneToList droneToList;
        public ParcelToList parcelToList;
        public StationToList stationToList;
        public CustomerToList customerToList;
        public DroneListWindow(IBL.Ibl IblObj)
        {
            InitializeComponent();

            bl = IblObj;
            droneToLists = new();
            parcelToLists = new();
            customerToLists = new();
            stationToLists = new();
            List<DroneToList> tempDroneToLists = bl.GetAllDrones().OrderBy(d => d.Weight).ToList();
            List<ParcelToList> tempParcelToLists = bl.GetAllParcels().OrderBy(p => p.Weight).ToList();
            List<CustomerToList> tempCustomerToLists = bl.GetAllCustomers().ToList();
            List<StationToList> tempStationToLists = bl.GetAllStation().ToList();
            foreach (var dronetolist in tempDroneToLists)
            {
                droneToLists.Add(dronetolist);
            }
            foreach (var parcelToList in tempParcelToLists)
            {
                parcelToLists.Add(parcelToList);
            }

            foreach (var stationToList in tempStationToLists)
            {
                stationToLists.Add(stationToList);
            }
            foreach (var customerToList in tempCustomerToLists)
            {
                customerToLists.Add(customerToList);
            }
            //DronesListView.ItemsSource = droneToLists;
            StationListView.ItemsSource = stationToLists;
            CustomerListView.ItemsSource = customerToLists;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StatusSelectorParcel.ItemsSource = Enum.GetValues(typeof(ParcelStatuses));
            WeightSelectorParcel.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PrioritySelectorParcel.ItemsSource = Enum.GetValues(typeof(Priorities));
            droneToLists.CollectionChanged += DroneToLists_CollectionChanged;
            parcelToLists.CollectionChanged += ParcelToLists_CollectionChanged;
        }


        private void DroneToLists_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            checkComboBoxesDrone();
        }

        private void checkComboBoxesDrone()
        {
            int wInd = WeightSelector.SelectedIndex;
            int sInd = StatusSelector.SelectedIndex;
            if (wInd == 3 && sInd == 3)
                DronesListView.ItemsSource = droneToLists;
            if (wInd == 3 && sInd != 3)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Status == StatusSelector.SelectedIndex);//.OrderBy(i=> i.);
            if (wInd != 3 && sInd == 3)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Weight == WeightSelector.SelectedIndex);
            if (wInd != 3 && sInd != 3)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Status == StatusSelector.SelectedIndex && (int)X.Weight == WeightSelector.SelectedIndex);
        }
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkComboBoxesDrone();                                                                                                                                                                                                              //being called th                                                                                                   //check the combo boxes...
        }

        private void DroneListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(this, bl).Show();
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, this).Show();
            //this.Close();
        }
        private void CancelDrone_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowParcelInTransfer_Click(object sender, RoutedEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            Drone drone = bl.GetDrone(droneToList.Id);
            if (drone.ParcelInTrans != null)
                MessageBox.Show(drone.ParcelInTrans.ToString(), $"Parcel of drone {drone.Id}");
            else
                MessageBox.Show("Drone has no parcel");
        }

        private void DeleteDrone_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deleted the drone");
        }

        // parcelwindow

        private void ParcelToLists_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            checkComboBoxesParcel();
        }

        private void checkComboBoxesParcel()
        {
            int wIndp = WeightSelectorParcel.SelectedIndex;
            int sIndp = StatusSelectorParcel.SelectedIndex;
            int pIndp = PrioritySelectorParcel.SelectedIndex;
            if (wIndp == 3&& sIndp==4 && pIndp==3)
                ParcelListView.ItemsSource = parcelToLists;
            //if (wIndp == 3 && sIndp != 4)
            //    ParcelListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Status == StatusSelector.SelectedIndex);
            //if (wIndp != 3 && sIndp == 3)
            //    ParcelListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Weight == WeightSelector.SelectedIndex);
            //if (wIndp != 3 && sIndp != 3)
            //    ParcelListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Status == StatusSelector.SelectedIndex && (int)X.Weight == WeightSelector.SelectedIndex);
        }
        private void StatusSelectorParcel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkComboBoxesParcel();                                                                                                                                                                                                              //being called th                                                                                                   //check the combo boxes...
        }

        private void ParcelListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(this, bl).Show();
        }

        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, this).Show();
            //this.Close();
        }
        private void CancelParcel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowParcel_Click(object sender, RoutedEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            Drone drone = bl.GetDrone(droneToList.Id);
            if (drone.ParcelInTrans != null)
                MessageBox.Show(drone.ParcelInTrans.ToString(), $"Parcel of drone {drone.Id}");
            else
                MessageBox.Show("Drone has no parcel");
        }

        private void ShowSender_Click(object sender, RoutedEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            Drone drone = bl.GetDrone(droneToList.Id);
            if (drone.ParcelInTrans != null)
                MessageBox.Show(drone.ParcelInTrans.ToString(), $"Parcel of drone {drone.Id}");
            else
                MessageBox.Show("Drone has no parcel");
        }

        private void ShowReceiver_Click(object sender, RoutedEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            Drone drone = bl.GetDrone(droneToList.Id);
            if (drone.ParcelInTrans != null)
                MessageBox.Show(drone.ParcelInTrans.ToString(), $"Parcel of drone {drone.Id}");
            else
                MessageBox.Show("Drone has no parcel");
        }

        private void ShowDrone_Click(object sender, RoutedEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            Drone drone = bl.GetDrone(droneToList.Id);
            if (drone.ParcelInTrans != null)
                MessageBox.Show(drone.ParcelInTrans.ToString(), $"Parcel of drone {drone.Id}");
            else
                MessageBox.Show("Drone has no parcel");
        }

        private void DeleteParcel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deleted the Parcel");
        }

        //stationwindow

        private void StationListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(this, bl).Show();
        }

        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, this).Show();
            //this.Close();
        }
        private void CancelStation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // customerWindow

        private void CustomerListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(this, bl).Show();
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, this).Show();
            //this.Close();
        }
        private void CancelCustomer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
