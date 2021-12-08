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
        updateModel =0,SendingToCharge, ReleaseFromCharge,Assign,CollectingAParcel,DeliveringAParcel
    }
    public partial class DroneWindow //: CustomWindow
    {
        IBL.Ibl bl;
        //internal int id;
       
        public int StationId { get; set; }
        private IBL.BO.Drone Drone { get; set; }
        DroneListWindow lastW;

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

        public DroneWindow(DroneListWindow last, IBL.Ibl ibl)// to update a drone
        {
            InitializeComponent();
            bl = ibl;
            lastW = last;
            Drone = bl.GetDrone(lastW.droneToList.Id);
           

            UpdateGrid.Visibility = Visibility.Visible;
            DataContext = Drone;
            
         
           
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
                MessageBox.Show(Drone.ToString());
                //   new DroneListWindow(bl).Show();
                this.Close();
            }
            catch (AddingException ex)
            {
                MessageBox.Show(ex.Message);
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
                
                MessageBox.Show(bl.GetDrone(Drone.Id).ToString());
                //new DroneListWindow(bl).Show();

                lastW.droneToLists.Remove(lastW.droneToList);
                lastW.droneToLists.Add(bl.getDroneToList(Drone.Id));
                lastW.DronesListView.Items.Refresh();
                this.Close();// replace old dronelist window
                
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
        }
        private void mTx_TextChanged(object sender, TextChangedEventArgs e)
        {
            submitUpdate.IsEnabled = true;
        }
        private void EnableSubmit()
        {
            if (!string.IsNullOrWhiteSpace(idTxAdd.Text) && !string.IsNullOrWhiteSpace(mTxAdd.Text) && wCbAdd.SelectedIndex != 3) 
                sumbitAdd.IsEnabled = true;
        }

        private void idTxAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSubmit();
        }

        private void wCbAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableSubmit();
        }

        private void mTxAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSubmit();
        }
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

      

        private void dTb_Click(object sender, RoutedEventArgs e)
        {
            parcelGrid.Visibility = Visibility.Visible;

        }

        private void senderButton_Click(object sender, RoutedEventArgs e)
        {
            senderGrid.Visibility = Visibility.Visible;
        }

        private void receiverButton_Click(object sender, RoutedEventArgs e)
        {
            receiverGrid.Visibility = Visibility.Visible;
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
