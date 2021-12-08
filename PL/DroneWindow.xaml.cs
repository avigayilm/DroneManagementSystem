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
    public partial class DroneWindow 
    {
        IBL.Ibl bl;
        public int StationId { get; set; }
        private IBL.BO.Drone Drone { get; set; }
        DroneListWindow lastW;

        public DroneWindow(IBL.Ibl IblObj , DroneListWindow last)// constructor to add a drone
        {
            InitializeComponent();
            bl = IblObj;
            lastW = last;
           wCbAdd.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            Drone = new();
            DataContext = Drone;
            sTCBAdd.ItemsSource = bl.GetAllStation().Select(s=> s.Id);
            addGrid.Visibility = Visibility.Visible;
        }

        public DroneWindow(DroneListWindow last, IBL.Ibl ibl) // constructor to update a drone
        {
            InitializeComponent();
            bl = ibl;
            lastW = last;
            Drone = bl.GetDrone(lastW.droneToList.Id);
            UpdateGrid.Visibility = Visibility.Visible; //shows  appropriate add grid for window
            DataContext = Drone;
            ComboUpdateOption.ItemsSource= Enum.GetValues(typeof(UpdateOptions));
        }
        /// <summary>
        /// adds the drone to list view and list in bl
        /// </summary>
        private void AddDrone()
        {
            StationId= (int)sTCBAdd.SelectedItem; //receive station id from combobox selection
            DroneLabel.Content = $"adding drone to the list";
            bl.AddDrone(Drone, StationId);
            lastW.droneToLists.Add(bl.GetAllDrones().First(x => x.Id == Drone.Id)); // after drone is updated in bl now updates listview
        }
        /// <summary>
        /// calls add drone function and displays added drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submitAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddDrone();
                MessageBox.Show(Drone.ToString(),"added Drone");
                this.Close();
            }
            catch (AddingException ex)
            {
                MessageBox.Show(ex.Message,"Adding issue");
            }
        }
        /// <summary>
        /// updates drone according to update option chosen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                lastW.droneToLists.Remove(lastW.droneToList); //updates the appropriate drone in list view 
                lastW.droneToLists.Add(bl.getDroneToList(Drone.Id));
                lastW.DronesListView.Items.Refresh();
                this.Close();
                
            }
            catch(UpdateIssueException ex)
            {
                MessageBox.Show(ex.Message, "UpdateIssue", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch(BatteryIssueException ex)
            {
                MessageBox.Show(ex.Message,"BatteryIssue", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        /// <summary>
        /// will enable adding button only if all appropriate fields were filled
        /// </summary>
        private void EnableSubmit()
        {
            if (!string.IsNullOrWhiteSpace(idTxAdd.Text) && !string.IsNullOrWhiteSpace(mTxAdd.Text) && wCbAdd.SelectedIndex != 3) 
                sumbitAdd.IsEnabled = true;
        }

        /// <summary>
        /// calls function to check whether to enable submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void idTxAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSubmit();
        }

        /// <summary>
        /// calls function to check whether to enable submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wCbAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableSubmit();
        }

        /// <summary>
        /// calls function to check whether to enable submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mTxAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableSubmit();
        }
        /// <summary>
        /// closes the window if user wishes to cancel choice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

      
        /// <summary>
        /// collapses and opens view of parcel in drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dTb_Click(object sender, RoutedEventArgs e)
        {
            if(Drone.ParcelInTrans==null)
            {
                MessageBox.Show("Drone doesn't contain a parcel", "No assigned Parcel");
                return;
            }
            if(parcelGrid.Visibility==Visibility.Hidden)
                 parcelGrid.Visibility = Visibility.Visible;
            else
                parcelGrid.Visibility = Visibility.Hidden;

        }
        /// <summary>
        /// collapses and shows the seder of parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void senderButton_Click(object sender, RoutedEventArgs e)
        {
            if (senderGrid.Visibility == Visibility.Hidden)
                senderGrid.Visibility = Visibility.Visible;
            else
                senderGrid.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// toggles collapsing and showing of receiver of parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void receiverButton_Click(object sender, RoutedEventArgs e)
        {
            if (receiverGrid.Visibility == Visibility.Hidden)
                receiverGrid.Visibility = Visibility.Visible;
            else
                receiverGrid.Visibility = Visibility.Hidden;
        }
    }
}
