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
    public enum DroneStatuses
    {
       Available, Maintenance, Delivery
    }

    public enum WeightCategories
    {
         Light, Medium, Heavy
    }

    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : CustomWindow
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
            DroneToList selectedObject = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(bl, selectedObject).Show();
            this.Close();
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
            this.Close();
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //    private void Button_Click(object sender, RoutedEventArgs e)
        //    {
        //        new DroneWindow(IblObj).Show();
        //    }
        //}
    }
}
