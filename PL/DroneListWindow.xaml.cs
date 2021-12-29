using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
using BlApi;

/// <summary>
/// window to show all the lists with tabs
/// </summary>
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
    /// struct to use for the dictionary
    /// </summary>
    public struct WeightAndStatus
    {
        public WeightCategories Weight { get; set; }
        public DroneStatuses Status { get; set; }
    }

 

    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow
    {
        BlApi.Ibl bl;
        public Dictionary<WeightAndStatus, List<DroneToList>> droneToLists;
        public Dictionary<string,List<ParcelToList>> parcelToLists;
        public ObservableCollection<CustomerToList> customerToLists;
        public Dictionary<int,List<StationToList>> stationToLists;
        public DroneToList droneToList;
        public ParcelToList parcelToList;
        public StationToList stationToList;
        public CustomerToList customerToList;

        public DroneListWindow(BlApi.Ibl IblObj)
        {
            InitializeComponent();

            bl = IblObj;
            droneToLists = new Dictionary<WeightAndStatus, List<DroneToList>>();
            IEnumerable<DroneToList> temp = bl.GetAllDrones();
            droneToLists = (from droneToList in temp
                            group droneToList by    
                            new WeightAndStatus()
                            {
                                Status = (DroneStatuses)droneToList.Status,
                                Weight = (WeightCategories)droneToList.Weight
                            }).ToDictionary(x => x.Key, x => x.ToList());//Grouping the drones
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            //StatusSelectorParcel.ItemsSource = Enum.GetValues(typeof(ParcelStatuses));
            //PrioritySelectorParcel.ItemsSource = Enum.GetValues(typeof(Priorities));
            StatusSelector.SelectedIndex = 3;
            WeightSelector.SelectedItem = 3;
            //PrioritySelectorParcel.SelectedIndex = 3;
            //StatusSelectorParcel.SelectedIndex = 4;
        }

        /// <summary>
        /// if selectors are changed the drone list will be updated accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneToLists_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            checkComboBoxesDrone();
        }
        /// <summary>
        /// determines how to filter the list
        /// </summary>
        public void checkComboBoxesDrone()
        {
            DroneStatuses sInd = (DroneStatuses)StatusSelector.SelectedIndex;
            WeightCategories wInd = (WeightCategories)WeightSelector.SelectedIndex;
            if (wInd == WeightCategories.All && sInd == DroneStatuses.All)
                DronesListView.ItemsSource = from el in droneToLists.Values.SelectMany(x => x)
                                             orderby el.Weight, el.Status
                                             select el;
            if (wInd == WeightCategories.All && sInd != DroneStatuses.All)
                DronesListView.ItemsSource = droneToLists
                    .Where(x => x.Key
                    .Status == (DroneStatuses)sInd).SelectMany(x => x.Value);

            if (wInd != WeightCategories.All && sInd == DroneStatuses.All)
                DronesListView.ItemsSource = droneToLists
                    .Where(x => x.Key
                    .Weight == (WeightCategories)wInd).SelectMany(x => x.Value);
            if (wInd != WeightCategories.All && sInd != DroneStatuses.All)
                DronesListView.ItemsSource = droneToLists
                    .Where(x => x.Key
                    .Status == (DroneStatuses)sInd && x.Key.Weight == (WeightCategories)wInd)
                    .SelectMany(x => x.Value);
        }

        /// <summary>
        /// will check selectors if one is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkComboBoxesDrone();                                                                                                                                                                                                              //being called th                                                                                                   //check the combo boxes...
        }
        /// <summary>
        /// if a drone is double clicked will call update function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(this, bl).Show();
        }
        /// <summary>
        /// if add button is selected wil open adding window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, this).Show();
            //this.Close();
        }
        /// <summary>
        /// if the cancel button is clicked it will close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelDrone_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// shows parcel for contextmenu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowParcelInTransfer_Click(object sender, RoutedEventArgs e)
        {
            droneToList = (DroneToList)DronesListView.SelectedItem;
            if (droneToList.ParcelId != 0)
            {
                parcelToList = new();
                parcelToList.Id = droneToList.ParcelId;// To show the correct parcel for the next window
                new ParcelWindow(this, bl).Show();
            }

        //    Drone drone = bl.GetDrone(droneToList.Id);
        //    if (drone.ParcelInTrans != null)
        //        MessageBox.Show(drone.ParcelInTrans.ToString(), $"Parcel of drone {drone.Id}");
           else
               MessageBox.Show("Drone has no parcel");
        }

        // parcelTab
        /// <summary>
        /// allows user to cancel and return to main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSender_Click(object sender, RoutedEventArgs e)
        {
            parcelToList = (ParcelToList)ParcelListView.SelectedItem;
            if (parcelToList.SenderId != null)
            {
                customerToList = new();
                customerToList.Id = parcelToList.SenderId;// To show the correct parcel for the next window
                new CustomerWindow(this, bl).Show();
            }
            else
                MessageBox.Show("Parcel has no sender");
            //droneToList = (DroneToList)DronesListView.SelectedItem;
            //Drone drone = bl.GetDrone(droneToList.Id);
            //if (drone.ParcelInTrans != null)
            //    MessageBox.Show(drone.ParcelInTrans.ToString(), $"Parcel of drone {drone.Id}");
            //else
            //    MessageBox.Show("Drone has no parcel");
        }

        private void ShowReceiver_Click(object sender, RoutedEventArgs e)
        {
            parcelToList = (ParcelToList)ParcelListView.SelectedItem;
            if (parcelToList.ReceiverId != null)
            {
                customerToList = new();
                customerToList.Id = parcelToList.ReceiverId;// To show the correct parcel for the next window
                new CustomerWindow(this, bl).Show();
            }
            else
                MessageBox.Show("Parcel has no receiver");
            //droneToList = (DroneToList)DronesListView.SelectedItem;
            //Drone drone = bl.GetDrone(droneToList.Id);
            //if (drone.ParcelInTrans != null)
            //    MessageBox.Show(drone.ParcelInTrans.ToString(), $"Parcel of drone {drone.Id}");
            //else
            //    MessageBox.Show("Drone has no parcel");
        }

        private void ShowDrone_Click(object sender, RoutedEventArgs e)
        {
            parcelToList = (ParcelToList)ParcelListView.SelectedItem;
            Parcel parcel=bl.GetParcel(parcelToList.Id);
            if (parcel.Dr.Id != 0)
            {
                droneToList = new();
                droneToList.Id = parcel.Dr.Id;// To show the correct parcel for the next window
                new DroneWindow(this, bl).Show();
            }
            else
                MessageBox.Show("Parcel has no Drone");
            //droneToList = (DroneToList)DronesListView.SelectedItem;
            //Drone drone = bl.GetDrone(droneToList.Id);
            //if (drone.ParcelInTrans != null)
            //    MessageBox.Show(drone.ParcelInTrans.ToString(), $"Parcel of drone {drone.Id}");
            //else
            //    MessageBox.Show("Drone has no parcel");
        }

        

        //stationwindow

        private void StationListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            stationToList = (StationToList)StationListView.SelectedItem;
            new StationWindow(this, bl).Show();
        }

        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(bl, this).Show();
            //this.Close();
        }
        private void CancelStation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // customerWindow

        private void CustomerListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            customerToList = (CustomerToList)CustomerListView.SelectedItem;
            new CustomerWindow(this, bl).Show();
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, this).Show();
            //this.Close();
        }
        private void CancelCustomer_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    

    

       

       

        

        private void DroneTab_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        //private void DronesListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    droneToLists.OrderBy(x => x.Id);
        //}

        #region parcel
        //private void MouseEnterParcelTab(object sender, MouseEventArgs e)
        //{
            
        //    IEnumerable<ParcelToList> temp = bl.GetAllParcels();
        //    parcelToLists = (from parceltolist in temp
        //                     group parceltolist by
        //                     parceltolist.SenderId
        //                    ).ToDictionary(x => x.Key, x => x.ToList());
        //    ParcelListView.ItemsSource = parcelToLists.Values.SelectMany(x => x);
        //    StatusSelectorParcel.ItemsSource = Enum.GetValues(typeof(ParcelStatuses));
        //    StatusSelectorParcel.SelectedIndex = 4;

        //    //WeightSelectorParcel.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        //    PrioritySelectorParcel.ItemsSource = Enum.GetValues(typeof(Priorities));
        //    PrioritySelectorParcel.SelectedIndex = 3;
        //}
        private void AddParcelClicked(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(bl, this).Show();
        }
        private void ParcelToLists_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CheckComboBoxesParcel();
        }

        private void DeleteParcel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deleted the Parcel");
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

        private void StatusSelectorParcel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckComboBoxesParcel();                                                                                                                                                                                                              //being called th                                                                                                   //check the combo boxes...
        }
        private void ParcelListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            parcelToList = (ParcelToList)ParcelListView.SelectedItem;
            new ParcelWindow(this, bl).Show();
        }
        public void CheckComboBoxesParcel()
        {
            //int wIndp = WeightSelectorParcel.SelectedIndex;
            int sIndp = StatusSelectorParcel.SelectedIndex;
            int pIndp = PrioritySelectorParcel.SelectedIndex;
            if (sIndp == 4 && pIndp == 3)
                ParcelListView.ItemsSource = from el in parcelToLists.Values.SelectMany(x => x)
                                             orderby el.Priority, el.ParcelStatus
                                             select el;
            if (pIndp == 3 && sIndp != 4)
                ParcelListView.ItemsSource = parcelToLists.SelectMany(x => x.Value.Where(p => (int)p.ParcelStatus == sIndp));
                   // .Where(x => x.Value.Where(p=> (int)p.ParcelStatus == sIndp).).SelectMany(x => x.Value);
            //ParcelListView.ItemsSource = parcelToLists.ToList().FindAll(X => (int)X.Status == StatusSelector.SelectedIndex);
            if(pIndp != 3 && sIndp == 4)
                ParcelListView.ItemsSource = parcelToLists.SelectMany(x => x.Value.Where(p => (int)p.Priority == pIndp));
            //ParcelListView.ItemsSource = parcelToLists.ToList().FindAll(X => (int)X.priority == PrioritySelectorParcel.SelectedIndex);
            if (pIndp != 3 && sIndp != 3)
                ParcelListView.ItemsSource = parcelToLists.SelectMany(x => x.Value.Where(p => (int)p.ParcelStatus == sIndp && (int)p.Priority == pIndp));
            //ParcelListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Status == StatusSelector.SelectedIndex && (int)X.priority == PrioritySelectorParcel.SelectedIndex);
        }

        #endregion parcel

        private void PrioritySelectorParcel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckComboBoxesParcel();
        }

        private void StationTab_MouseEnter(object sender, MouseEventArgs e)
        {
            IEnumerable<StationToList> temp = bl.GetAllStation();
            stationToLists = (from stationtolist in temp
                              group stationtolist by
                              stationtolist.AvailableChargeSlots
                            ).ToDictionary(x => x.Key, x => x.ToList());
            StationListView.ItemsSource = stationToLists.Values.SelectMany(x => x);
        }

       

        private void CustomerTab_MouseEnter(object sender, MouseEventArgs e)
        {
            customerToLists = new();
            List<CustomerToList> tempCustomerToLists = bl.GetAllCustomers().ToList();
            foreach (var customerToList in tempCustomerToLists)
            {
                customerToLists.Add(customerToList);
            }
            CustomerListView.ItemsSource = customerToLists;
        }

        private void ParcelTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IEnumerable<ParcelToList> temp = bl.GetAllParcels();
            parcelToLists = (from parceltolist in temp
                             group parceltolist by
                             parceltolist.SenderId
                            ).ToDictionary(x => x.Key, x => x.ToList());
            ParcelListView.ItemsSource = parcelToLists.Values.SelectMany(x => x); /// order by!!!

            StatusSelectorParcel.ItemsSource = Enum.GetValues(typeof(ParcelStatuses));
            PrioritySelectorParcel.ItemsSource = Enum.GetValues(typeof(Priorities));
            StatusSelectorParcel.SelectedIndex = 4;

            //WeightSelectorParcel.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            PrioritySelectorParcel.SelectedIndex = 3;

            
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                FrameworkElement framework = sender as FrameworkElement;
                ParcelToList CurrentParcel = framework.DataContext as ParcelToList;
                if (CurrentParcel.ParcelStatus == (BO.ParcelStatuses)ParcelStatuses.Created)// You could only delete if not assigned
                {
                    bl.DeleteParcel(CurrentParcel.Id);
                    parcelToLists[CurrentParcel.SenderId].RemoveAll(i => i.Id == CurrentParcel.Id);
                    ParcelListView.Items.Refresh();
                    CheckComboBoxesParcel();
                }
                else
                {
                    MessageBox.Show("Can't Delete parcel, it's in process", "Delete Exception", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            catch(RetrievalException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void DronesListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    droneToLists.OrderBy(x => x.Id);
        //}
    }
}

