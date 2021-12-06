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

    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow
    {
        IBL.Ibl bl;
        public ObservableCollection<DroneToList> droneToLists;
        public DroneListWindow(IBL.Ibl IblObj)
        {
            InitializeComponent();
            bl = IblObj;
            droneToLists = new();
            List<DroneToList> tempDroneToLists = bl.GetAllDrones().ToList();
            foreach(var dronetolist in tempDroneToLists)
            {
                droneToLists.Add(dronetolist);
            }
            DronesListView.ItemsSource = droneToLists;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            droneToLists.CollectionChanged += DroneToLists_CollectionChanged;
        }

        private void DroneToLists_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            checkComboBoxes();
        }

        private void checkComboBoxes()
        {
            int wInd = WeightSelector.SelectedIndex;
            int sInd = StatusSelector.SelectedIndex;
            if (wInd == 3 && sInd == 3)
                DronesListView.ItemsSource = droneToLists;
            if (wInd == 3 && sInd != 3)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Status == StatusSelector.SelectedIndex);//.OrderBy(i=> i.);
            if(wInd != 3 && sInd == 3)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Weight == WeightSelector.SelectedIndex);
            if (wInd != 3 && sInd != 3)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(X => (int)X.Status == StatusSelector.SelectedIndex && (int)X.Weight == WeightSelector.SelectedIndex);
        }
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkComboBoxes();                                                                                                                                                                                                              //being called th                                                                                                   //check the combo boxes...
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkComboBoxes();
        }

        private void DroneListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList selectedObject = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(bl,this, selectedObject).Show();
          //  this.Close();
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, this).Show();
            //this.Close();
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void DronesListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    DronesListView.ItemsSource= 
        //}




        //    private void Button_Click(object sender, RoutedEventArgs e)
        //    {
        //        new DroneWindow(IblObj).Show();
        //    }
        //}
    }
}
