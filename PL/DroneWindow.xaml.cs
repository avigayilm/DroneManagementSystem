﻿using System;
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
        updateModel =0,SendingToCharge, ReleaseFromCharge,Assign,CollectingAParcel,DeliveringAParcel
    }
    public partial class DroneWindow //: CustomWindow
    {
        IBL.Ibl bl;
        //internal int id;
       
        public int StationId { get; set; }
        private IBL.BO.Drone Drone { get; set; }
        DroneListWindow lastW;

        double chargingTime;
        public DroneWindow(IBL.Ibl IblObj , DroneListWindow last)// to add a drone
        {
            InitializeComponent();
            bl = IblObj;
            lastW = last;
           wCbAdd.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            //note that u havent dealt with the "all" option in the enum
            Drone = new();
            DataContext = Drone;
            sTCBAdd.ItemsSource = bl.GetAllStation().Select(s=> s.Id);
            addGrid.Visibility = Visibility.Visible;
        }

        public DroneWindow(IBL.Ibl ibl, DroneListWindow last, DroneToList dr)// to update a drone
        {
            bl = ibl;
            Drone = bl.GetDrone(dr.Id);
            lastW = last;
            DataContext = Drone;
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
           
            //wCb.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            //submit.Content = "Update Drone";
           // choice = "update";
            ComboUpdateOption.ItemsSource= Enum.GetValues(typeof(UpdateOptions));
        }

        private void AddDrone()
        {
            StationId= (int)sTCBAdd.SelectedItem;
            
            
           // weight = (IBL.BO.WeightCategories)(WeightCategories)wCb.SelectedIndex;
            DroneLabel.Content = $"adding drone to the list";
            //Drone dr = new()
            //{
            //    Id = id,
            //    Weight = weight
            //};
            
            bl.AddDrone(Drone, StationId);
            lastW.droneToLists.Add(bl.GetAllDrones().First(x => x.Id == Drone.Id));
        }


        private void submitAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddDrone();
                MessageBox.Show(Drone.ToString(),"added Drone");
                //   new DroneListWindow(bl).Show();
                this.Close();
            }
            catch (AddingException ex)
            {
                MessageBox.Show(ex.Message,"Adding issue");
                // new DroneListWindow(bl).Show();
                //this.Close();
            }
        }
        private void submitUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
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
                                bl.ReleasingDroneFromCharge(Drone.Id);
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
                
                MessageBox.Show(bl.GetDrone(Drone.Id).ToString(),"Updated Drone");
                //new DroneListWindow(bl).Show();
                int index = lastW.droneToLists.ToList().FindIndex(x => x.Id == Drone.Id);
                lastW.droneToLists[index] = bl.getDroneToList(Drone.Id);
                lastW.DronesListView.Items.Refresh();
                this.Close();// replace old dronelist window
                
            }
            catch(UpdateIssueException ex)
            {
                MessageBox.Show(ex.Message, "UpdateIssue", MessageBoxButton.OK, MessageBoxImage.Warning);
               // new DroneListWindow(bl).Show();
               // this.Close();
            }
            catch(BatteryIssueException ex)
            {
                MessageBox.Show(ex.Message,"BatteryIssue", MessageBoxButton.OK, MessageBoxImage.Warning);
                //new DroneListWindow(bl).Show();
                //this.Close();
            }
            catch(DroneChargeException ex)
            {
                MessageBox.Show(ex.Message,"DroneCharge Issue", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch(RetrievalException ex)
            {
                MessageBox.Show(ex.Message,"Retrieval Issue", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch(DeliveryIssueException ex)
            {
                MessageBox.Show(ex.Message,"Delivery Issue", MessageBoxButton.OK, MessageBoxImage.Warning);
            }      
        }
        private void mTx_TextChanged(object sender, TextChangedEventArgs e)
        {
            submitUpdate.IsEnabled = true;
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

        //private void sTx_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    EnableSubmit();
        //}
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
          //  new DroneListWindow(bl).Show();
            this.Close();
           
        }

        //private void UpdateOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
          
        //    //UpdateOptions inputedOption = (UpdateOptions)ComboUpdateOption.SelectedItem;
        //    //if (inputedOption==UpdateOptions.updateModel)
        //    //{
        //    //    mTx.IsReadOnly = false;
               
        //    //}
        //}

        

        private void sTCBAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationId = (int)sTCBAdd.SelectedItem;
        }

        private void mTxAdd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dTb_Click(object sender, RoutedEventArgs e)
        {
            if(parcelGrid.Visibility==Visibility.Hidden)
                 parcelGrid.Visibility = Visibility.Visible;
            else
                parcelGrid.Visibility = Visibility.Hidden;

        }

        private void senderButton_Click(object sender, RoutedEventArgs e)
        {
            if (senderGrid.Visibility == Visibility.Hidden)
                senderGrid.Visibility = Visibility.Visible;
            else
                senderGrid.Visibility = Visibility.Hidden;
        }

        private void receiverButton_Click(object sender, RoutedEventArgs e)
        {
            if (receiverGrid.Visibility == Visibility.Hidden)
                receiverGrid.Visibility = Visibility.Visible;
            else
                receiverGrid.Visibility = Visibility.Hidden;
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
