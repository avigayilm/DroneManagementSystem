using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
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
using System.ComponentModel;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    //public partial class Check : ValidationRule, IDataErrorInfo, INotifyPropertyChanged// I notifypropertychanged is an interface so that we can make varaibles update on events. meaning we make them dependency property
    //{
    //    BlApi.Ibl bl;
    //    DroneToList droneToList = new();
    //    public int MinimumCharacters { get; set; }

    //    // this is just to gove red border
    //    public override ValidationResult Validate(object value, CultureInfo cultureInfo)//value is the string written in he textbox
    //    {
    //        string charString = value as string; //casting to string
    //        if (charString.Length < MinimumCharacters)
    //            return new ValidationResult(false, $" no {MinimumCharacters} characters.");// there was an error give tis message as error
    //        // we can add here a lot of iff statement
    //        return new ValidationResult(true, null);
    //    }
    //    public string UserName { get; set; }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public int Id2
    //    {
    //        get { return droneToList.Id; }
    //        set
    //        {
    //            Id2 = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));// invoke show that something changed in the dictionary
    //        }
    //    }
    //    public string Error => throw new NotImplementedException();
    //    public string this[string columnName] => throw new NotImplementedException();
    //}
        //public Dictionary<string, string> ErrorMessages { get; private set; } = new Dictionary<string, string>

        //public string this[string name]
        //{
        //    get
        //    {
        //        string result = null;
        //        switch (name)
        //        {
        //            case "Password":// name of variable that gets error
        //                if (string.IsNullOrWhiteSpace(Password))
        //                {
        //                    result = "Password cannot be empty";// message that will be printed
        //                }
        //                else if (User.Password.Length < 8)
        //                {
        //                    result = " Password must not be less than 8 digits";
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //        if (ErrorMessages.ContainsKey(name))// checks if a key exists in the dictionary
        //        {
        //            ErrorMessages[name] = result;

        //        }
        //        else if(result!=null)// if the result is not null and the key doens't exist yet we add it to the dictionary
        //        {
        //            ErrorMessages.Add(name, result);// name=key, and result=messages
        //        }
        //        return result;
        //    }
        //}


    
    public enum UpdateOptions
    {
        updateModel = 0, SendingToCharge, ReleaseFromCharge, Assign, CollectingAParcel, DeliveringAParcel
    }
    public partial class DroneWindow
    {
        BlApi.Ibl bl;
        WeightAndStatus wAndS;

        public bool AutoManual// true if auto, false if manual
        {
            get { return (bool)GetValue(AutoManualProperty); }
            set { SetValue(AutoManualProperty, value); }
        }
        public static readonly DependencyProperty AutoManualProperty =
            DependencyProperty.Register("AutoManual", typeof(bool), typeof(DroneWindow));
        public int StationId { get; set; }
        private Drone Drone { get; set; }
        DroneListWindow lastW;
        BackgroundWorker AutoRun;
        private void UpdatedTask() => AutoRun.ReportProgress(0); // invokes report progress action for thread updating
        private bool chekEnd() => AutoRun.CancellationPending; // returns whether to cancel yet or not
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
            addGrid.Visibility = Visibility.Hidden;
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
            wAndS.Status = (DroneStatuses)Drone.Status;
            wAndS.Weight = (WeightCategories)Drone.Weight;
            if (lastW.droneToLists.ContainsKey(wAndS))
                lastW.droneToLists[wAndS].Add(bl.GetAllDrones().First(x => x.Id == Drone.Id));
            else
            {
                lastW.droneToLists.Add(wAndS, bl.GetAllDrones().Where(x => x.Id == Drone.Id).ToList());
            }
            //lastW.droneToLists.ContainsKey(wAndS)? 
            //    lastW.droneToLists[wAndS].Add(bl.GetAllDrones().First(x => x.Id == Drone.Id)) :
            //    lastW.droneToLists.Add(wAndS, bl.GetAllDrones().TakeWhile(x => x.Id == Drone.Id).ToList());
            lastW.checkComboBoxesDrone();
            // after drone is updated in bl now updates listview
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
                DroneToList tempForUpdate = lastW.droneToList;
                UpdateOptions inputedOption = (UpdateOptions)ComboUpdateOption.SelectedItem;
                switch (inputedOption)
                {
                    case UpdateOptions.SendingToCharge:
                        {

                            int tempStationId = bl.SendingDroneToCharge(Drone.Id);
                            lastW.stationToLists.Values
                                .First(s => s.Exists(st => st.Id == tempStationId))
                                .First(s => s.Id == tempStationId)
                                .AvailableChargeSlots -= 1;
                            lastW.StationListView.Items.Refresh();
                            break;
                        }
                    case UpdateOptions.ReleaseFromCharge:
                        {
                           int tempStationId = bl.ReleasingDroneFromCharge(Drone.Id);
                            lastW.stationToLists.Values
                                .First(s => s.Exists(st => st.Id == tempStationId))
                                .First(s => s.Id == tempStationId)
                                .AvailableChargeSlots += 1;
                            lastW.StationListView.Items.Refresh();
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

                lastW.droneToList.Model = Drone.Model;
                if(lastW.droneToList.Status != Drone.Status)//if the status has changed and so the drone belongs to a  differant key in dicitonary
                {
                    
                    wAndS.Status = (DroneStatuses)Drone.Status;
                    wAndS.Weight = (WeightCategories)Drone.Weight;
                    lastW.droneToLists[wAndS].Remove(tempForUpdate); //remove the drone from its original placing
                    wAndS.Status = (DroneStatuses)lastW.droneToList.Status;
                    wAndS.Weight = (WeightCategories)lastW.droneToList.Weight;
                    if (lastW.droneToLists.ContainsKey(wAndS)) // if the dicionary holds a key for the updated drone add it htere
                        lastW.droneToLists[wAndS].Add(lastW.droneToList);
                    else // if it doesnt- create the key for the updated drone
                    {
                        List<BO.DroneToList> temp = new();
                        temp.Add(lastW.droneToList);
                        lastW.droneToLists.Add(wAndS ,temp);
                    }
                }
                //var item = lastW.droneToLists.Where(i => i.Key.Status == (DroneStatuses)Drone.Status
                //&& i.Key.Weight == (WeightCategories)Drone.Weight).First();
                //item.ToList().Remove(lastW.droneToList);
                //item.Append(lastW.droneToList);
                //int index = lastW.droneToLists.ToList().FindIndex(i => i.Key.Status == (DroneStatuses)Drone.Status
                //&& i.Key.Weight == (WeightCategories)Drone.Weight);
                //lastW.droneToLists[index] = item;


                lastW.checkComboBoxesDrone();
                //lastW.DronesListView.Items.Refresh();
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
            {
                parcelGrid.Visibility = Visibility.Hidden;
                senderGrid.Visibility = Visibility.Hidden;
                receiverGrid.Visibility = Visibility.Hidden;
            }

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

        private void AutoButton_Click(object sender, RoutedEventArgs e)
        {
            if (AutoManual == false)// if it's on state of manual
            {
                AutoManual = true;
                AutoRun = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                AutoRun.DoWork += AutoRun_DoWork;
                AutoRun.ProgressChanged += AutoRun_ProgressChanged;
                AutoRun.RunWorkerCompleted += AutoRun_RunWorkerCompleted;
                AutoRun.RunWorkerAsync(Drone.Id);
            }
            else
            {
                AutoManual = false;
                AutoRun?.CancelAsync();
            }
            //    Auto = true;
            //    AutoRun = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            //    AutoRun.DoWork += AutoRun_DoWork;
            //    AutoRun.ProgressChanged += AutoRun_ProgressChanged;
            //    AutoRun.RunWorkerCompleted += AutoRun_RunWorkerCompleted;
            //    AutoRun.RunWorkerAsync(Drone.Id);
            //}
            //private void ManualButton_Click(object seder, RoutedEventArgs e) => AutoRun?.CancelAsync();
        }
            private void AutoRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                throw new NotImplementedException();
            }

            private void AutoRun_ProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                Drone = bl.GetDrone(Drone.Id);
                DataContext = Drone;
            }

            private void AutoRun_DoWork(object sender, DoWorkEventArgs e)
            {
                bl.simulation(Drone.Id, chekEnd, UpdatedTask);
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
