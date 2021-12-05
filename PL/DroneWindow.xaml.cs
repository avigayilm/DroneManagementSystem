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
        updateModel,SendingToCharge, ReleaseFromCharge,Assign,CollectingAParcel,DeliveringAParcel
    }
    public partial class DroneWindow : CustomWindow
    {
        IBL.Ibl bl;
        //internal int id;
       
        public int StationId;
       
        private IBL.BO.WeightCategories weight;
        private IBL.BO.Drone Drone ;
        string choice;
        double chargingTime;
        public DroneWindow(IBL.Ibl IblObj, DroneListWindow last)// to add a drone
        {
            InitializeComponent();
            bl = IblObj;
            statCb.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            wCb.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            wCb.SelectedIndex = 3;
            statCb.SelectedIndex = 3;
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

        }

        public DroneWindow(IBL.Ibl ibl, DroneListWindow last, DroneToList dr)// to update a drone
        {
            bl = ibl;
            Drone = bl.GetDrone(dr.Id);
            DataContext = Drone;
    
            InitializeComponent();
            EnableAllKeys(); 
            statCb.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            wCb.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            //idTx.Text = (dr.Id).ToString();
            //wCb.SelectedIndex= (int)dr.Weight;
            //statCb.SelectedIndex = (int)dr.Status;
            //dTx.Text = (dr.ParcelId).ToString();
            //lnTx.Text = (dr.Loc.Longitude).ToString();
            //ltTx.Text = (dr.Loc.Latitude).ToString();
            //mTx.Text = dr.Model;
            submit.Content = "Update Drone";
            choice = "update";
            ComboUpdateOption.ItemsSource= Enum.GetValues(typeof(UpdateOptions));
        }

        private void AddDrone()
        {
            //id = (int.Parse(idTx.Text));
            StationId = (int.Parse(sTx.Text));
            
           // weight = (IBL.BO.WeightCategories)(WeightCategories)wCb.SelectedIndex;
            DroneLabel.Content = $"adding drone to the list";
            //Drone dr = new()
            //{
            //    Id = id,
            //    Weight = weight
            //};
            bl.AddDrone(Drone, StationId);
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {  
                if (choice == "add")
                {
                    AddDrone();
                    MessageBox.Show(Drone.ToString());
                    new DroneListWindow(bl).Show();
                    this.Close();
                }
                if (choice == "update")
                {
                    UpdateOptions inputedOption = (UpdateOptions)ComboUpdateOption.SelectedItem;
                    switch (inputedOption)
                    {
                        case UpdateOptions.SendingToCharge:
                            {

                                bl.SendingDroneToCharge(Drone.Id);
                                break;
                            }
                        case UpdateOptions.ReleaseFromCharge:
                            {
                                bl.ReleasingDroneFromCharge(Drone.Id, chargingTime);
                                break;
                            }
                        case UpdateOptions.Assign:
                            {
                                bl.AssignParcelToDrone(Drone.Id);
                                break;
                            }
                        case UpdateOptions.CollectingAParcel:
                            {
                                bl.CollectingParcelByDrone(Drone.Id);
                                break;
                            }
                        case UpdateOptions.DeliveringAParcel:
                            {
                                bl.DeliverParcelByDrone(Drone.Id);
                                break;
                            }
                        case UpdateOptions.updateModel:
                            {
                                bl.UpdateDrone(Drone.Id, mTx.Text);
                                break;
                            }
                    }
                }
                MessageBox.Show(Drone.ToString());
                //new DroneListWindow(bl).Show();
                this.Close();// replace old dronelist window
                
            }
            catch (AddingException ex)
            {
                MessageBox.Show(ex.Message);
               // new DroneListWindow(bl).Show();
                //this.Close();
             }
            catch(UpdateIssueException ex)
            {
                MessageBox.Show(ex.Message);
               // new DroneListWindow(bl).Show();
               // this.Close();
            }
            catch(BatteryIssueException ex)
            {
                MessageBox.Show(ex.Message);
                //new DroneListWindow(bl).Show();
                //this.Close();
            }
            catch(DroneChargeException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(RetrievalException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(DeliveryIssueException ex)
            {
                MessageBox.Show(ex.Message);
            }
             //   _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
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
        private void mTx_TextChanged(object sender, TextChangedEventArgs e)
        {
            submit.IsEnabled = true;
        }
        private void EnableSubmit()
        {
          //  if (!string.IsNullOrWhiteSpace(idTx.Text) && sTx.Text != string.Empty && wCb.SelectedIndex > -1) ;
            //  submit.IsEnabled = true;
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
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).Show();
            this.Close();
           
        }

        private void UpdateOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableAllKeys();
            UpdateOptions inputedOption = (UpdateOptions)ComboUpdateOption.SelectedItem;
            if (inputedOption == UpdateOptions.ReleaseFromCharge)
            {
                //addMinutestxt.Visibility = Visibility.Visible;
                //addMinutes.Visibility = Visibility.Visible;
            }
            if (inputedOption==UpdateOptions.updateModel)
            {
                mTx.IsEnabled = true;

            }
            if(idTx.Text != string.Empty)
            {
                submit.IsEnabled = true;
            }
        }

        private void EnableAllKeys()
        {
            idTx.IsEnabled = false;
            idTb.IsEnabled = false;
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

        //private void add_Click(object sender, RoutedEventArgs e)
        //{
        //    AddDrone();
        //    MessageBox.Show(droneTemp.ToString());
        //    new DroneListWindow(bl).Show();
        //    this.Close();
        //}
    }
}
