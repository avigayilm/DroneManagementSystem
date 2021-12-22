using System;
using System.Collections.Generic;
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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class Check : ValidationRule, IDataErrorInfo, INotifyPropertyChanged// I notifypropertychanged is an interface so that we can make varaibles update on events. meaning we make them dependency property
    {
        IBL.Ibl bl;
        DroneToList droneToList = new();
        public int MinimumCharacters { get; set; }

        // this is just to gove red border
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)//value is the string written in he textbox
        {
            string charString = value as string; //casting to string
            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $" no{MinimumCharacters}characters.");// there was an error give tis message as error
            // we can add here a lot of iff statement
            return new ValidationResult(true, null);
        }
        public string UserName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public int Id2
        {
        get{return droneToList.Id;}
        set
            {
                Id2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));// invoke show that something changed in the dictionary
            }
        }
        public string Error => throw new NotImplementedException();
        public string this[string columnName] => throw new NotImplementedException();
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


    }
    public enum UpdateOptions
    {
        updateModel =0,SendingToCharge, ReleaseFromCharge,Assign,CollectingAParcel,DeliveringAParcel
    }
    public partial class DroneWindow//: CustomWindow
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

                lastW.droneToLists.Remove(lastW.droneToList);
                lastW.droneToLists.Add(bl.getDroneToList(Drone.Id));
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
            if(Drone.ParcelInTrans==null)
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
