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

    public class WeightAndStatus
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
        public ObservableCollection<IGrouping<WeightAndStatus, DroneToList>> droneToLists;
        public DroneToList droneToList;
        public DroneListWindow(BlApi.Ibl IblObj)
        {
            InitializeComponent();
            bl = IblObj;
            droneToLists = new ObservableCollection<IGrouping<WeightAndStatus, DroneToList>>();

            //IEnumerable<DroneToList> temp = bl.GetAllDrones();

            //var temp = /*(ObservableCollection<IGrouping<WeightAndStatus, DroneToList>>)*/
            //               (from droneToList in bl.GetAllDrones()
            //                group droneToList by new WeightAndStatus { Weight = droneToList.Weight, Status = droneToList.Status });
            //droneToLists= new ObservableCollection<IGrouping<WeightAndStatus, DroneToList>>(temp);

            (from droneToList in bl.GetAllDrones()

             group droneToList by

             new WeightAndStatus()

             {
                
                 Status = (DroneStatuses)droneToList.Status,

                 Weight =(WeightCategories)droneToList.Weight

             }).ToList().ForEach(x => droneToLists.Add(x));


            //var item = 
            //              (from droneToList in bl.GetAllDrones()
            //               group droneToList by  droneToList.Status into NewGroup
            //               orderby NewGroup.Sum(x => x.Battery)
            //               select new
            //               {
            //                   key = droneToList.Status,
            //                   avg = NewGroup.Average(x => x.Battery),
            //                   sum = NewGroup.Sum(x => x.Battery),
            //                   sumMulti = (from l in NewGroup where l.Battery > 20 select l.Battery * 5).Sum()
            //               });
            //List<string> vs = new List<string>();
            //string s = vs.Aggregate((x, y) => x.Length > y.Length ? x : y);

            DronesListView.ItemsSource = droneToLists.SelectMany(x=>x);
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
            DroneStatuses sInd = (DroneStatuses)StatusSelector.SelectedItem;
            WeightCategories wInd =(WeightCategories) WeightSelector.SelectedItem;
            if (wInd == WeightCategories.All && sInd == DroneStatuses.All)
                DronesListView.ItemsSource = droneToLists.SelectMany(x => x);
            if (wInd == WeightCategories.All && sInd != DroneStatuses.All)
                DronesListView.ItemsSource = droneToLists.Where(x => x.Key.Status == (DroneStatuses)sInd).SelectMany(x => x);
                    
            if (wInd != WeightCategories.All && sInd == DroneStatuses.All)
                DronesListView.ItemsSource = droneToLists.Where(x => x.Key.Weight == (WeightCategories)wInd).SelectMany(x => x);
            if (wInd != WeightCategories.All && sInd != DroneStatuses.All)
                DronesListView.ItemsSource = droneToLists.Where(x => x.Key.Status == (DroneStatuses)sInd && x.Key.Weight == (WeightCategories)wInd).SelectMany(x => x);
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

        /// <summary>
        /// allows user to cancel and return to main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void DronesListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    droneToLists.OrderBy(x => x.Id);
        //}
    }
}
