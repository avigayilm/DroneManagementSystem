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
        Normal, Fast, Emergency, All
    }
    public enum ParcelStatuses
    {
        Created, Assigned, PickedUp, Delivered, All
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
    public partial class ListWindow
    {
        BlApi.Ibl bl;
      
        public ObservableCollection<DroneToList> droneToListsOb; 
        public Dictionary<string, List<ParcelToList>> parcelToLists;
        public ObservableCollection<CustomerToList> customerToLists;
        public ObservableCollection<StationToList> stationToLists;
        public DroneToList droneToList;
        public ParcelToList parcelToList;
        public StationToList stationToList;
        public CustomerToList customerToList;

        public ListWindow(BlApi.Ibl IblObj)
        {
            InitializeComponent();

            bl = IblObj;

            droneToListsOb = new(from d in bl.GetAllDrones() select d);
                             

            DronesListView.ItemsSource = droneToListsOb;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StatusSelector.SelectedIndex = 3;
            WeightSelector.SelectedItem = 3;
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
        }

        /// <summary>
        /// upon clicking wil show the receiver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        }


  

      

      
        //private void DronesListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    droneToLists.OrderBy(x => x.Id);
        //}

        #region parcel
       

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
            new LoginWindow().Show();
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
            if (pIndp != 3 && sIndp == 4)
                ParcelListView.ItemsSource = parcelToLists.SelectMany(x => x.Value.Where(p => (int)p.Priority == pIndp));
            //ParcelListView.ItemsSource = parcelToLists.ToList().FindAll(X => (int)X.priority == PrioritySelectorParcel.SelectedIndex);
            if (pIndp != 3 && sIndp != 4)
                ParcelListView.ItemsSource = parcelToLists.SelectMany(x => x.Value.Where(p => (int)p.ParcelStatus == sIndp && (int)p.Priority == pIndp));
            //ParcelListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Status == StatusSelector.SelectedIndex && (int)X.priority == PrioritySelectorParcel.SelectedIndex);
        }

        #endregion parcel
        #region drone


        /// <summary>
        /// determines how to filter the list
        /// </summary>
        public void checkComboBoxesDrone()
        {

            DroneStatuses sInd = (DroneStatuses)StatusSelector.SelectedIndex;
            WeightCategories wInd = (WeightCategories)WeightSelector.SelectedIndex;
            if (wInd == WeightCategories.All && sInd == DroneStatuses.All)
            {
                DronesListView.ItemsSource = droneToListsOb;
            }
            if (wInd == WeightCategories.All && sInd != DroneStatuses.All)
            {
                DronesListView.ItemsSource = droneToListsOb.Where<DroneToList>(x => (DroneStatuses)x.Status == (DroneStatuses)sInd);
            }
            if (wInd != WeightCategories.All && sInd == DroneStatuses.All)
            {
                DronesListView.ItemsSource = droneToListsOb.Where<DroneToList>(x => (WeightCategories)x.Status == (WeightCategories)wInd);
            }
            if (wInd != WeightCategories.All && sInd != DroneStatuses.All)
            {
                DronesListView.ItemsSource = droneToListsOb.Where<DroneToList>(x => (WeightCategories)x.Status == (WeightCategories)wInd
                && (DroneStatuses)x.Status == (DroneStatuses)sInd);
            }
        }


        private void ShowDrone_Click(object sender, RoutedEventArgs e)
        {
            parcelToList = (ParcelToList)ParcelListView.SelectedItem;
            Parcel parcel = bl.GetParcel(parcelToList.Id);
            if (parcel.Dr.Id != 0)
            {
                droneToList = new();
                droneToList.Id = parcel.Dr.Id;// To show the correct parcel for the next window
                new DroneWindow(this, bl).Show();
            }
            else
                MessageBox.Show("Parcel has no Drone");

        }

        private void DroneToLists_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            checkComboBoxesDrone();
        }

        /// <summary>
        /// will check drone selectors if one is changed
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
            //because we use the station list in some case of updating - if the stationlist isnt initialized it will be initailize here
            if (stationToLists == null)
            {
                IEnumerable<StationToList> temp  = (from st in bl.GetAllStation()
                        select st);
                stationToLists = new(temp);
            }
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
            new LoginWindow().Show();
        }

        /// <summary>
        /// deletes teh drone, but not actually from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDownDrone(object sender, MouseButtonEventArgs e)
        {
            try
            {

                FrameworkElement framework = sender as FrameworkElement;
                DroneToList CurrentDrone = framework.DataContext as DroneToList;
                //WeightAndStatus weightstatus = new WeightAndStatus { Weight = (PL.WeightCategories)CurrentDrone.Weight, Status = (PL.DroneStatuses)CurrentDrone.Status };
                bl.DeleteDrone(CurrentDrone.Id);
                droneToListsOb.Remove(CurrentDrone);
                // droneToLists[weightstatus].RemoveAll(i => i.Id == CurrentDrone.Id);
                DronesListView.Items.Refresh();
                //checkComboBoxesDrone();

            }
            catch (RetrievalException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion drone
        #region station

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
            new LoginWindow().Show();
        }

        private void StationTab_MouseEnter(object sender, MouseEventArgs e)
        {
            if (stationToLists == null)
            {
                //IEnumerable<StationToList> temp = bl.GetAllStation();
                var temp = (from st in bl.GetAllStation()
                        select st);
                stationToLists = new(temp);
            }

            StationListView.ItemsSource = stationToLists;//.Values.SelectMany(x => x);
        }

        private void Image_MouseDownStation(object sender, MouseButtonEventArgs e)
        {
            try
            {
                FrameworkElement framework = sender as FrameworkElement;
                StationToList CurrentStation = framework.DataContext as StationToList;
                bl.DeleteStation(CurrentStation.Id);
                int index = stationToLists.IndexOf(CurrentStation);
                stationToLists.RemoveAt(index);
                // stationToLists[CurrentStation.AvailableChargeSlots].RemoveAll(i => i.Id == CurrentStation.Id);
                StationListView.Items.Refresh();

            }
            catch (RetrievalException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// updates charge slots in station for listView- receivces number to add to occupied slots and take of available ones
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="num"></param>
        internal void UpdateChargeSlots(int stationId, int num)
        {
            StationToList tempSt = stationToLists.First(s => s.Id == stationId);
            int index = stationToLists.IndexOf(tempSt);
            stationToLists.Remove(tempSt);
            tempSt.OccupiedSlots += num;
            tempSt.AvailableChargeSlots -= num;
            stationToLists.Insert(index, tempSt);
        }

        #endregion station

        #region customer

        private void CustomerTab_MouseEnter(object sender, MouseEventArgs e)
        {
            // customerToLists = new();

            if (customerToLists == null)
            {
                customerToLists = new(from cus in bl.GetAllCustomers()
                                      select cus);
                //    bl.GetAllCustomers().ToList();
                //foreach (var customerToList in tempCustomerToLists)
                //{
                //    customerToLists.Add(customerToList);
                //}
                CustomerListView.ItemsSource = customerToLists;
            }
        }

        private void Image_MouseDownCustomer(object sender, MouseButtonEventArgs e)
        {
            try
            {
                FrameworkElement framework = sender as FrameworkElement;
                CustomerToList CurrentCustomer = framework.DataContext as CustomerToList;
                bl.DeleteCustomer(CurrentCustomer.Id);
                customerToLists.Remove(CurrentCustomer);
                CustomerListView.Items.Refresh();

            }
            catch (RetrievalException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
            new LoginWindow().Show();
        }



        #endregion customer

     
        private void PrioritySelectorParcel_SelectionChanged(object sender, SelectionChangedEventArgs e) //gets called when one combobox is changed
        {
            CheckComboBoxesParcel();
        }




        private void logout_Click(object sender, RoutedEventArgs e) //will logout
        {
            new LoginWindow().Show();
            this.Close();

        }



        /// <summary>
        /// will semi delete parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDownParcel(object sender, MouseButtonEventArgs e)
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
            catch (RetrievalException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e) 
        {
            DronesListView.Items.Refresh();
        }


        private void ShowSenderParcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Image_MouseEnter_4(object sender, MouseEventArgs e)
        {
            DronesListView.Items.Refresh();
        }


        

      
       

    }
}

