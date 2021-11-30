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

    public enum UpdateOptions
    {
        updateModel,SendingToCharge, ReleaseFromCharge,Delivery,CollectingAParcel,DeliveringAParcel
    }
    public partial class DroneWindow : Window
    {
        IBL.Ibl bl;
        internal int id;
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
            ComboUpdateOption.Visibility = Visibility.Collapsed;// doesn't show the update option
            choice = "add";
            //DronesListView.ItemsSource = bl.GetAllDrones();
            //StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            //WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));

        }

        public DroneWindow(IBL.Ibl ibl, DroneToList dr)// to update a drone
        {
            InitializeComponent();
            bl = ibl;
            idTx.Text = (dr.Id).ToString();
            wCb.SelectedIndex = (int)dr.Weight;
            statCb.SelectedIndex = (int)dr.Status;
            dTx.Text = (dr.ParcelId).ToString();
            lnTx.Text = (dr.Loc.Longitude).ToString();
            ltTx.Text = (dr.Loc.Latitude).ToString();
            mTx.Text = dr.Model;
            submit.Content = "Update Drone";
            choice = "update";
            ComboUpdateOption.ItemsSource= Enum.GetValues(typeof(UpdateOptions));
        }

        private void AddDrone()
        {
            id = (int.Parse(idTx.Text));
            StationId = (int.Parse(sTx.Text));
            
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
            try
            {
                UpdateOptions inputedOption = (UpdateOptions)ComboUpdateOption.SelectedItem;
                if (choice == "add")
                    AddDrone();
                if (choice == "update")
                {
                    switch (inputedOption)
                    {
                        case UpdateOptions.SendingToCharge:
                            {

                                bl.SendingDroneToCharge(int.Parse(idTx.Text));
                                break;
                            }
                        case UpdateOptions.ReleaseFromCharge:
                            {
                                bl.ReleasingDroneFromCharge(int.Parse(idTx.Text), double.Parse(addMinutestxt.Text));
                                break;
                            }
                        case UpdateOptions.Delivery:
                            {
                                bl.AssignParcelToDrone(int.Parse(idTx.Text));
                                break;
                            }
                        case UpdateOptions.CollectingAParcel:
                            {
                                bl.CollectingParcelByDrone(int.Parse(idTx.Text));
                                break;
                            }
                        case UpdateOptions.DeliveringAParcel:
                            {
                                bl.DeliverParcelByDrone(int.Parse(idTx.Text));
                                break;
                            }
                        case UpdateOptions.updateModel:
                            {
                                bl.UpdateDrone(int.Parse(idTx.Text), mTx.Text);
                                break;
                            }
                    }
                }
                MessageBox.Show(droneTemp.ToString());
            }
            catch(AddingException ex)
            {
                _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        //switch (messageBoxResult)       
        //        {
        //            case MessageBoxResult.None:
        //                break;
        //            case MessageBoxResult.OK:
        //                Close();             
        //                break;
        //            case MessageBoxResult.Cancel:
        //                break;
        //            case MessageBoxResult.Yes:
        //                break;
        //            case MessageBoxResult.No:
        //                break;
        //            default:
        //                break;
        //        }
            }
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

        private void idTx_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSubmit();
        }

        private void wCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableSubmit();
        }

        private void sTx_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSubmit();
        }

        private void UpdateOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableAllKeysbutId();
            UpdateOptions inputedOption = (UpdateOptions)ComboUpdateOption.SelectedItem;
            if(inputedOption==UpdateOptions.ReleaseFromCharge)
            {
                addMinutestxt.Visibility = Visibility.Visible;
                addMinutes.Visibility = Visibility.Visible;
            }
            if(inputedOption==UpdateOptions.updateModel)
            {
                mTx.IsEnabled = true;

            }
            if(idTx.Text != string.Empty)
            {
                submit.IsEnabled = true;
            }
        }

        private void EnableAllKeysbutId()
        {
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
            sTb.IsEnabled = false;
            sTx.IsEnabled = false;
            wTb.IsEnabled = false;
            wCb.IsEnabled = false;
        }
    }
}
