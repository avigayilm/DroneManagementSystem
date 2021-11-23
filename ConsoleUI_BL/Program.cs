using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IBL;

// need to finish empty input by update
//update battery for the droneCharge functions
//check the adding with all the exceptions

namespace ConsoleUI_BL
{
    class Program
    {
        //enums to determine updating, adding etc. choices
        enum MenuOptions { Exit, Add, Update, Show_One, Show_List, Show_Distance }
        enum EntityOptions { Exit, Station, Drone, Customer, Parcel }
        enum UpdateOptions { Exit, Drone, Customer, Station,Assignment, Pickedup, Delivery, Recharge, ReleasefromCharge }
        enum ListOptions { Exit, Stations, Drones, Customers, Parcels, UnAssignmentParcels, AvailableChargingStations, }

        //menu function wil go thorough all options for user
        private static void ShowMenuBL()
        {

            Ibl ibl1 = new BL.BL();

            EntityOptions entityOption;
            MenuOptions menuOption;
            ListOptions listOption;
            UpdateOptions updateOption;
            Console.WriteLine("Welcome to our Parcel service!\nPlease make your choice out of the following");
            do
            {
                Console.WriteLine("options:\n 1-Add, \n 2 Update,\n,3 Show Item,\n 4-show List,\n 0-Exit");
                menuOption = (MenuOptions)int.Parse(Console.ReadLine());
                switch (menuOption)
                {
                    case MenuOptions.Add:
                        {
                            try
                            {
                                Console.WriteLine("What do you want to add?\n 1:Station,\n 2:Drone,\n 3:Customer\n4:Parcel\n 0:return to menu\n");
                                entityOption = (EntityOptions)int.Parse(Console.ReadLine());
                                switch (entityOption)//what entity to add
                                {
                                    case EntityOptions.Station://add station
                                        {
                                            Console.WriteLine("Enter the ID, name, latitude(range +-90), longitude(range +-180), and chargeslots\n ");
                                            int.TryParse(Console.ReadLine(), out int stationID);
                                            string inputName = Console.ReadLine();
                                            double latitudeInput, longitudeInput;
                                            double.TryParse(Console.ReadLine(), out  latitudeInput);
                                            //latitudeInput = Convert.ToDouble(Console.ReadLine());
                                            double.TryParse(Console.ReadLine(), out  longitudeInput);
                                            int.TryParse(Console.ReadLine(), out int slots);
                                            Station tempStat = new()// adds a new station
                                            {
                                                Id = stationID,
                                                Name = inputName,
                                                AvailableChargeSlots = slots,
                                                Loc = new() { Longitude = longitudeInput, Latitude = latitudeInput },
                                                Charging = new()
                                            };
                                            ibl1.AddStation(tempStat);
                                            break;
                                        };
                                    case EntityOptions.Customer:// adds a new customer
                                        {

                                            Console.WriteLine("Enter the ID, name, phone.\n ");
                                            string inputId = Console.ReadLine();
                                            string inputname = Console.ReadLine();
                                            string inputphone = Console.ReadLine();
                                            Customer newCustomer = new()
                                            {
                                                Id = inputId,
                                                Name = inputname,
                                                PhoneNumber = inputphone,

                                            };
                                            ibl1.AddCustomer(newCustomer);
                                            break;
                                        }
                                    case EntityOptions.Drone://adds drone
                                        {

                                            Console.WriteLine("Enter the Id,weightcategory(0=light,1=medium,2=heavy),stationId for first charging");
                                            int.TryParse(Console.ReadLine(), out int droneID);
                                            WeightCategories maxim = (WeightCategories)int.Parse(Console.ReadLine());
                                            int.TryParse(Console.ReadLine(), out int stationID);
                                            Drone newDrone = new()
                                            {
                                                Id = droneID,
                                                Weight = maxim,
                                            };

                                            ibl1.AddDrone(newDrone, stationID);// add drone to dronelist
                                            break;
                                        }
                                    case EntityOptions.Parcel://adds a parcel
                                        {

                                            Console.WriteLine("Enter the senderId, the targetId, weightcategory(0=light,1=medium,2=heavy), priority(0=normal,1=fast,2=emergency),\n ");// date requested, scheduled time\n ");
                                            string inputSenderId = Console.ReadLine();
                                            string inputTargetId = Console.ReadLine();
                                            WeightCategories maxim = (WeightCategories)int.Parse(Console.ReadLine());
                                            Priorities prio = (Priorities)int.Parse(Console.ReadLine());
                                            Parcel newParcel = new()
                                            {
                                                Sender = new() { Id = inputSenderId },
                                                Receiver = new() { Id = inputTargetId },
                                                Weight = maxim,
                                                Priority = prio,
                                            };
                                            ibl1.AddParcel(newParcel);
                                            break;
                                        }


                                }
                                
                            }
                            catch(AddingException ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                            catch(InvalidInputException ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                            break;
                        }
                    case MenuOptions.Update://update existing entities
                        {
                            try
                            {
                                Console.WriteLine("what do you want to update?\n 1-Drone\n 2-Customer\n 3-Station\n 4-Assignment,\n 5-Pickedup\n 6-Delivery,\n 7-Recharge\n 8-ReleasefromCharge\n 0:return to menu\n");
                                updateOption = (UpdateOptions)int.Parse(Console.ReadLine());
                                switch (updateOption)
                                {
                                    case UpdateOptions.Drone:
                                        {
                                            Console.WriteLine("Enter the ID and the model\n");
                                            int droneId = int.Parse(Console.ReadLine());
                                            string model = Console.ReadLine();
                                            ibl1.UpdateDrone(droneId, model);
                                            break;
                                        }
                                    case UpdateOptions.Station:
                                        {
                                            Console.WriteLine("Enter the ID and the name or\and chargeslots\n");
                                            int stationId = int.Parse(Console.ReadLine());
                                            string name = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(name))// checks if there was a string entered
                                                name = null;
                                            int availableChargeSlots;
                                            bool result = int.TryParse(Console.ReadLine(), out availableChargeSlots);
                                            if (!result)
                                                availableChargeSlots = -1;
                                            ibl1.UpdateStation(stationId, name, availableChargeSlots);
                                            break;
                                        }
                                    case UpdateOptions.Customer:
                                        {
                                            Console.WriteLine("Enter the ID name and Phone\n");
                                            string customerId = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(customerId)) ;
                                            string name = Console.ReadLine();
                                            string phone = Console.ReadLine();
                                            if (!string.IsNullOrEmpty(name))
                                                name = null;
                                            if (!string.IsNullOrEmpty(phone))
                                                phone = null;
                                            ibl1.UpdateCustomer(customerId, name, phone);
                                            break;
                                        }
                                    case UpdateOptions.Recharge:
                                        {
                                            Console.WriteLine("Enter the droneId\n");
                                            int droneId = int.Parse(Console.ReadLine());
                                            ibl1.SendingDroneToCharge(droneId);
                                            break;
                                        }
                                    case UpdateOptions.ReleasefromCharge:
                                        {
                                            Console.WriteLine("Enter the droneId\n and the charging time");
                                            int droneId = int.Parse(Console.ReadLine());
                                            double chargeTime = double.Parse(Console.ReadLine());
                                            ibl1.ReleasingDroneFromCharge(droneId, chargeTime);
                                            break;
                                        }
                                    case UpdateOptions.Assignment://assign drone to parcel
                                        {
                                            Console.WriteLine("Enter the droneId.\n");
                                            int droneId = int.Parse(Console.ReadLine());
                                            ibl1.AssignParcelToDrone(droneId);
                                            break;
                                        }
                                    case UpdateOptions.Delivery://adds a delivery of parcel by drone
                                        {
                                            Console.WriteLine("Enter the droneId\n");
                                            int.TryParse(Console.ReadLine(), out int droneId);
                                            ibl1.DeliverParcelByDrone(droneId);
                                            break;
                                        }
                                    case UpdateOptions.Pickedup://changes drone and parcel accordingly
                                        {
                                            Console.WriteLine("Enter the droneId\n");
                                            int.TryParse(Console.ReadLine(), out int droneId);
                                            ibl1.CollectingParcelByDrone(droneId);
                                            break;
                                        }
                                    case UpdateOptions.Exit:
                                        {
                                            break;
                                        }
                                }
                            }
                            catch(UpdateIssueException ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                            catch(DroneChargeException ex)
                            {
                                Console.WriteLine((ex.Message));
                            }
                            catch(RetrievalException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;

                        }
                    case MenuOptions.Show_List://displays the lists
                        {

                                Console.WriteLine("what List do you want to print?\n 1-Stations\n, 2-Drones\n, 3-Customers\n, 4-Parcels\n, 5-UnAssignmentParcels\n, 6-AvailableChargingStations\n, 7-DroneCharge\n0:return to menu\n");
                                listOption = (ListOptions)int.Parse(Console.ReadLine());
                                switch (listOption)
                                {
                                    case ListOptions.Stations://display stations list
                                        {
                                            List<StationToList> stationListTemp = (List<StationToList>)ibl1.GetAllStation();
                                            stationListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                            break;
                                        }
                                    case ListOptions.Parcels://display parcels list
                                        {
                                            List<ParcelToList> parcelListTemp = (List<ParcelToList>)ibl1.GetAllParcels();
                                            Console.WriteLine("----------DISPLAY PARCELLIST----------\n");
                                            parcelListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                            break;
                                        }
                                    case ListOptions.Drones://display drones list
                                        {
                                            List<DroneToList> dronesListTemp = (List<DroneToList>)ibl1.GetAllDrones();
                                            Console.WriteLine("----------DISPLAY DRONELIST----------\n");
                                            dronesListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                            break;
                                        }
                                    case ListOptions.Customers://display customers list
                                        {
                                            List<CustomerToList> customerListTemp = (List<CustomerToList>)ibl1.GetAllCustomers();
                                            Console.WriteLine("----------DISPLAY CUSTOMERLIST----------\n");
                                            customerListTemp.ForEach(p => Console.WriteLine(p));
                                            break;
                                        }
                                    case ListOptions.UnAssignmentParcels://display unassigned parcels
                                        {
                                            Console.WriteLine("----------DISPLAY UNASSIGNMENTPARCELLIST----------\n");
                                            ((List<ParcelToList>)ibl1.GetAllUnassignedParcels()).ForEach(p => Console.WriteLine(p.ToString()));
                                            break;
                                        }
                                    case ListOptions.AvailableChargingStations://diplay stations with available charging slots
                                        Console.WriteLine("----------DISPLAY AVAILABLECHARGINGSTATION----------\n");
                                        ((List<StationToList>)ibl1.GetAllStationsWithCharging()).ForEach(p => Console.WriteLine(p));
                                        break;
                                    case ListOptions.Exit:
                                        {
                                            break;
                                        }
                                
                            }
                            break;
                        }

                    case MenuOptions.Show_One://displays a single entity
                        {
                            try
                            {
                                Console.WriteLine("what do you want to print?\n 1-Station,\n 2-Drone,\n 3-Customer,\n 4-Parcel\n 0:return to menu\n");
                                entityOption = (EntityOptions)int.Parse(Console.ReadLine());
                                switch (entityOption)
                                {
                                    case EntityOptions.Station:
                                        {
                                            Console.WriteLine("Enter the ID of the station you want to display\n");
                                            int.TryParse(Console.ReadLine(), out int id);
                                            Console.WriteLine("----------DISPLAY STATION----------\n");
                                            Console.WriteLine(ibl1.GetStation(id));
                                            break;
                                        }
                                    case EntityOptions.Parcel:
                                        {
                                            Console.WriteLine("Enter the ID of the parcel you want to print\n");
                                            int.TryParse(Console.ReadLine(), out int id);
                                            Console.WriteLine("----------DISPLAY PARCEL----------\n");
                                            Console.WriteLine(ibl1.GetParcel(id));
                                            break;
                                        }
                                    case EntityOptions.Drone:
                                        {
                                            Console.WriteLine("Enter the ID of the drone you want to print\n");
                                            int.TryParse(Console.ReadLine(), out int id);
                                            Console.WriteLine("----------DISPLAY DRONE----------\n");
                                            Console.WriteLine(ibl1.GetDrone(id));
                                            break;
                                        }
                                    case EntityOptions.Customer:
                                        {
                                            Console.WriteLine("Enter the ID of the Customer you want to print\n");
                                            string id = Console.ReadLine();
                                            Console.WriteLine("----------DISPLAY CUSTOMER----------\n");
                                            Console.WriteLine(ibl1.GetCustomer(id));
                                            break;
                                        }
                                    case EntityOptions.Exit:
                                        {
                                            break;
                                        }

                                }
                            }
                            catch(RetrievalException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case MenuOptions.Exit:
                        {
                            break;
                        }
                }
            }
            while (menuOption != MenuOptions.Exit);




        }


       static void Main(string[] args)
        {
            ShowMenuBL();
        }
    }
}

