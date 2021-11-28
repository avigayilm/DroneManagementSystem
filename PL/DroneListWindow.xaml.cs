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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.Ibl bl;
        public DroneListWindow(IBL.Ibl IblObj)
        {
            InitializeComponent();
            bl = IblObj;
            DronesListView.ItemsSource = bl.GetAllDrones();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = bl.GetAllDrones(X => (int)X.Status == StatusSelector.SelectedIndex);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = bl.GetAllDrones(d => (int)d.Weight == WeightSelector.SelectedIndex);
        }
        private void DroneListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.ListView list = (System.Windows.Controls.ListView)sender;
            DroneToList selectedObject = (DroneToList)list.SelectedItem;
            new DroneWindow(bl, selectedObject).Show();
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(IblObj).Show();
        }
    }
}
