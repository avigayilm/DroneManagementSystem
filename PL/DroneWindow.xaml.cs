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
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>

    public enum UpdateOptions
    {
        updateModel = 0, SendingToCharge, ReleaseFromCharge, Assign, CollectingAParcel, DeliveringAParcel
    }
    public partial class DroneWindow
    {
        BlApi.Ibl bl;
        public int StationId { get; set; }
        private Drone Drone { get; set; }
        DroneListWindow lastW;

        public DroneWindow(BlApi.Ibl IblObj, DroneListWindow last)// constructor to add a drone
        {
            InitializeComponent();
            bl = IblObj;
            lastW = last;
            wCbAdd.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            Drone = new Drone();
            DataContext = Drone;
            //sTCBAdd.ItemsSource = bl.GetAllStation().Select(s=> s.Id);
            sTCBAdd.ItemsSource = bl.GetAllStation().Select(s => s.Id);
            addGrid.Visibility = Visibility.Visible;
        }

        public DroneWindow(DroneListWindow last, BlApi.Ibl ibl) // constructor to update a drone
        {
            InitializeComponent();
            bl = ibl;
            lastW = last;
            Drone = bl.GetDrone(lastW.droneToList.Id);
            UpdateGrid.Visibility = Visibility.Visible; //shows  appropriate add grid for window
            DataContext = Drone;
            ComboUpdateOption.ItemsSource = Enum.GetValues(typeof(UpdateOptions));
        }
        /// <summary>
        /// adds the drone to list view and list in bl
        /// </summary>
        private void AddDrone()
        {
            StationId = (int)sTCBAdd.SelectedItem; //receive station id from combobox selection
            DroneLabel.Content = $"adding drone to the list";
            bl.AddDrone(Drone, StationId);
            foreach (var item in lastW.droneToLists.Where(x => x.Key.Status == (DroneStatuses)Drone.Status && x.Key.Weight == (WeightCategories)Drone.Weight))
            {
                item.Append(bl.GetAllDrones().First(x => x.Id == Drone.Id));
                break;
            }// after drone is updated in bl now updates listview
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
                MessageBox.Show(Drone.ToString(), "added Drone");
                this.Close();
            }
            catch (AddingException ex)
            {
                MessageBox.Show(ex.Message, "Adding issue");
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

                MessageBox.Show(bl.GetDrone(Drone.Id).ToString(), "Updated Drone");
                //lastW.droneToLists = (from dronetolist in lastW.droneToLists
                //                      where dronetolist.Key.Status == Drone.Status && dronetolist.Key.Weight == Drone.Weight
                //                      );
                lastW.droneToLists[lastW.DronesListView.SelectedIndex] = (IGrouping<WeightAndStatus, DroneToList>)lastW.droneToList;



                //lastW.droneToLists = (ObservableCollection<IGrouping<WeightAndStatus, DroneToList>>)
                //           (from droneToList in bl.GetAllDrones()
                //            group droneToList by new WeightAndStatus { Weight = (WeightCategories)droneToList.Weight, Status = (DroneStatuses)droneToList.Status });

                lastW.droneToLists.Remove();
                //(from droneToList in bl.GetAllDrones()

                // group droneToList by

                // new WeightAndStatus()

                // {

                //     Status = (DroneStatuses)droneToList.Status,

                //     Weight = (WeightCategories)droneToList.Weight

                // }).ToList().ForEach(x => lastW.droneToLists.Add(x));


                //foreach (IGrouping<WeightAndStatus, DroneToList> item in lastW.droneToLists.Where(x => x.Key.Status == Drone.Status && x.Key.Weight == Drone.Weight))
                //{


                //    foreach (var dro in item)
                //    {
                //        if (/*bl.GetAllDrones().First(x => x.Id == Drone.Id) == dro.Id*/ true)
                //        {
                //            item.Remove(dro);
                //            break;
                //        }

                //    }

                //}

                //foreach (var item in lastW.droneToLists.Where(x => x.Key.Status == Drone.Status && x.Key.Weight == Drone.Weight))
                //{
                //    item.(bl.GetAllDrones().First(x => x.Id == Drone.Id));
                //    break;
                //}// aft
                //lastW.droneToLists.Remove(lastW.droneToList); //updates the appropriate drone in list view 
                //lastW.droneToLists.Add(bl.getDroneToList(Drone.Id));
                lastW.DronesListView.Items.Refresh();
                this.Close();

            }
            catch (UpdateIssueException ex)
            {
                MessageBox.Show(ex.Message, "UpdateIssue", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (BatteryIssueException ex)
            {
                MessageBox.Show(ex.Message, "BatteryIssue", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (DroneChargeException ex)
            {
                MessageBox.Show(ex.Message, "DroneCharge Issue", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (RetrievalException ex)
            {
                MessageBox.Show(ex.Message, "Retrieval Issue", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (DeliveryIssueException ex)
            {
                MessageBox.Show(ex.Message, "Delivery Issue", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (Drone.ParcelInTrans == null)
            {
                MessageBox.Show("Drone doesn't contain a parcel", "No assigned Parcel");
                return;
            }
            if (parcelGrid.Visibility == Visibility.Hidden)
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
