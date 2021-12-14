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
using BlApi;

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

    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow
    {
        BlApi.Ibl bl;
        public ObservableCollection<DroneToList> droneToLists;
        public DroneToList droneToList;
        public DroneListWindow(BlApi.Ibl IblObj)
        {
            InitializeComponent();
            bl = IblObj;
            droneToLists = new ObservableCollection<DroneToList>();
            List<DroneToList> tempDroneToLists = bl.GetAllDrones().ToList();
            foreach (var dronetolist in tempDroneToLists)
            {
                droneToLists.Add(dronetolist);
            }
            DronesListView.ItemsSource = droneToLists;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            droneToLists.CollectionChanged += DroneToLists_CollectionChanged;
        }

        /// <summary>
        /// if selectors are changed the drone list will be updated accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneToLists_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            checkComboBoxes();
        }
        /// <summary>
        /// determines how to filter the list
        /// </summary>
        private void checkComboBoxes()
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

        /// <summary>
        /// will check selectors if one is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkComboBoxes();                                                                                                                                                                                                              //being called th                                                                                                   //check the combo boxes...
        }

        /// <summary>
        /// will check selectors if one is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkComboBoxes();
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
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        /// <summary>
        /// allows user to cancel and return to main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
