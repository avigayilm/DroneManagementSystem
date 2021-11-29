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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.Ibl bl;
        private int id;
        private int StationId;
        private IBL.BO.WeightCategories weight;
        private IBL.BO.Drone droneTemp ;
        string choice;
        public DroneWindow(IBL.Ibl IblObj)// to add a drone
        {
            InitializeComponent();
            bl = IblObj;
            mTb.IsEnabled = false;
            ltTb.IsEnabled = false;
            lnTb.IsEnabled = false;
            dTb.IsEnabled = false;
            statTb.IsEnabled = false;
            mTx.IsEnabled = false;
            statCb.IsEnabled = false;
            dTx.IsEnabled = false;
            lnTx.IsEnabled = false;
            ltTx.IsEnabled = false;
            mTb.IsEnabled = false;
            submit.Content = "Add Drone";
            //DronesListView.ItemsSource = bl.GetAllDrones();
            //StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            //WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));

        }

        public DroneWindow(IBL.Ibl ibl, DroneToList dr)// to update a drone
        {
            InitializeComponent();
            bl = ibl;
            submit.Content = "Update Drone";
        }

        private void AddDrone()
        {
            id = (int.Parse(idTx.Text));
            StationId = (int.Parse(sTx.Text));
            wCb.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            weight = (IBL.BO.WeightCategories)(WeightCategories)wCb.SelectedIndex;
            DroneLabel.Content = $"adding drone{id.ToString()} to the list";
            droneTemp = new()
            {
                Id = id,
                Weight = weight
            };
            bl.AddDrone(droneTemp, StationId);
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
           AddDrone();
           MessageBox.Show(droneTemp.ToString());
        }

        private void mTx_TextChanged(object sender, TextChangedEventArgs e)
        {
            submit.IsEnabled = true;
        }
        private void EnableSubmit()
        {
            if (idTx.Text != string.Empty && sTx.Text != string.Empty && wCb.SelectedIndex > 1)
                submit.IsEnabled = true;
        }
    }
}
