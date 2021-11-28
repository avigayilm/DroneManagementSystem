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
        enum UpdateOptions { Exit, Drone, Customer, Station, Assignment, Pickedup, Delivery, Recharge, ReleasefromCharge }
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
                Console.WriteLine("options:\n1-Add\n2 Update\n3-Show Item \n4-show List\n0-Exit");
                menuOption = (MenuOptions)int.Parse(Console.ReadLine());
                switch (menuOption)
                {
                    case MenuOptions.Add:
                        {
                            try
                            {
                                Console.WriteLine("What do you want to add?\n1:Station\n2:Drone\n3:Customer\n4:Parcel\n0:return to menu");
                                entityOption = (EntityOptions)int.Parse(Console.ReadLine());
                                switch (entityOption)//what entity to add
                                {
                                    case EntityOptions.Station://add station
                                        {
                                            Console.WriteLine("Enter the ID, name, latitude(range +-90), longitude(range +-180), and chargeslots");
                                            int.TryParse(Console.ReadLine(), out int stationID);
                                            string inputName = Console.ReadLine();
                                            double latitudeInput, longitudeInput;
                                            double.TryParse(Console.ReadLine(), out latitudeInput);
                                            double.TryParse(Console.ReadLine(), out longitudeInput);
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

                                            Console.WriteLine("Enter the ID, name, phone,latitude(range +-90), longitude(range +-180)");
                                            string inputId = Console.ReadLine();
                                            string inputname = Console.ReadLine();
                                            string inputphone = Console.ReadLine();
                                            double latitudeInput, longitudeInput;
                                            double.TryParse(Console.ReadLine(), out latitudeInput);
                                            double.TryParse(Console.ReadLine(), out longitudeInput);
                                            Customer newCustomer = new()
                                            {
                                                Id = inputId,
                                                Name = inputname,
                                                PhoneNumber = inputphone,
                                                Loc = new() { Longitude = longitudeInput, Latitude = latitudeInput },

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

                                            Console.WriteLine("Enter the senderId, the targetId, weightcategory(0=light,1=medium,2=heavy), priority(0=normal,1=fast,2=emergency)");// date requested, scheduled time\n ");
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
                            catch (AddingException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (InvalidInputException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case MenuOptions.Update://update existing entities
                        {
                            try
                            {
                                Console.WriteLine("what do you want to update?\n1-Drone\n2-Customer\n3-Station\n4-Assignment\n5-Pickedup\n6-Delivery\n7-Recharge\n8-ReleasefromCharge\n0:return to menu");
                                updateOption = (UpdateOptions)int.Parse(Console.ReadLine());
                                switch (updateOption)
                                {
                                    case UpdateOptions.Drone:
                                        {
                                            Console.WriteLine("Enter the ID and the model");
                                            int droneId = int.Parse(Console.ReadLine());
                                            string model = Console.ReadLine();
                                            ibl1.UpdateDrone(droneId, model);
                                            break;
                                        }
                                    case UpdateOptions.Station:
                                        {
                                            Console.WriteLine("Enter the ID and the name and chargeslots");
                                            int stationId = int.Parse(Console.ReadLine());
                                            string name = Console.ReadLine();
                                            if (string.IsNullOrEmpty(name))// checks if there was a string entered
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

                                            Console.WriteLine("Enter the ID name and Phone");
                                            string customerId = Console.ReadLine();
                                            string name = Console.ReadLine();
                                            string phone = Console.ReadLine();
                                            if (string.IsNullOrEmpty(name))
                                                name = null;
                                            if (string.IsNullOrEmpty(phone))
                                                phone = null;
                                            ibl1.UpdateCustomer(customerId, name, phone);
                                            break;
                                        }
                                    case UpdateOptions.Recharge:
                                        {
                                            Console.WriteLine("Enter the droneId");
                                            int droneId = int.Parse(Console.ReadLine());
                                            ibl1.SendingDroneToCharge(droneId);
                                            break;
                                        }
                                    case UpdateOptions.ReleasefromCharge:
                                        {
                                            Console.WriteLine("Enter the droneId and the charging time in minutes");
                                            int droneId = int.Parse(Console.ReadLine());
                                            double chargeTime = double.Parse(Console.ReadLine());
                                            ibl1.ReleasingDroneFromCharge(droneId, chargeTime);
                                            break;
                                        }
                                    case UpdateOptions.Assignment://assign drone to parcel
                                        {
                                            Console.WriteLine("Enter the droneId.");
                                            int droneId = int.Parse(Console.ReadLine());
                                            ibl1.AssignParcelToDrone(droneId);
                                            break;
                                        }
                                    case UpdateOptions.Delivery://adds a delivery of parcel by drone
                                        {
                                            Console.WriteLine("Enter the droneId.");
                                            int.TryParse(Console.ReadLine(), out int droneId);
                                            ibl1.DeliverParcelByDrone(droneId);
                                            break;
                                        }
                                    case UpdateOptions.Pickedup://changes drone and parcel accordingly
                                        {
                                            Console.WriteLine("Enter the droneId");
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
                            catch (UpdateIssueException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (DroneChargeException ex)
                            {
                                Console.WriteLine((ex.Message));
                            }
                            catch (RetrievalException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (BatteryIssueException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (DeliveryIssueException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;

                        }
                    case MenuOptions.Show_List://displays the lists
                        {
                            try
                            {
                                Console.WriteLine("what List do you want to print?\n1-Stations\n2-Drones\n3-Customers\n4-Parcels\n5-UnAssignmentParcels\n6-AvailableChargingStations\n7-DroneCharge\n0:return to menu");
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
                                            Console.WriteLine("----------DISPLAY PARCELS----------\n");
                                            parcelListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                            break;
                                        }
                                    case ListOptions.Drones://display drones list
                                        {
                                            List<DroneToList> dronesListTemp = (List<DroneToList>)ibl1.GetAllDrones();
                                            Console.WriteLine("----------DISPLAY DRONES----------\n");
                                            dronesListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                            break;
                                        }
                                    case ListOptions.Customers://display customers list
                                        {
                                            List<CustomerToList> customerListTemp = (List<CustomerToList>)ibl1.GetAllCustomers();
                                            Console.WriteLine("----------DISPLAY CUSTOMERS----------\n");
                                            customerListTemp.ForEach(p => Console.WriteLine(p));
                                            break;
                                        }
                                    case ListOptions.UnAssignmentParcels://display unassigned parcels
                                        {
                                            Console.WriteLine("----------DISPLAY UNASSIGNEDPARCELS----------\n");
                                            ((List<ParcelToList>)ibl1.GetAllUnassignedParcels()).ForEach(p => Console.WriteLine(p.ToString()));
                                            break;
                                        }
                                    case ListOptions.AvailableChargingStations://diplay stations with available charging slots
                                        Console.WriteLine("----------DISPLAY AVAILABLECHARGINGSTATION----------\n");
                                        ((List<StationToList>)ibl1.GetAllStation(s => s.AvailableChargeSlots > 0)).ForEach(p => Console.WriteLine(p));
                                        break;
                                    case ListOptions.Exit:
                                        {
                                            break;
                                        }

                                }
                            }
                            catch (RetrievalException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }
                    case MenuOptions.Show_One://displays a single entity
                        {
                            try
                            {
                                Console.WriteLine("what do you want to print?\n1-Station\n2-Drone\n3-Customer\n4-Parcel\n0:return to menu");
                                entityOption = (EntityOptions)int.Parse(Console.ReadLine());
                                switch (entityOption)
                                {
                                    case EntityOptions.Station:
                                        {
                                            Console.WriteLine("Enter the ID of the station you want to display");
                                            int.TryParse(Console.ReadLine(), out int id);
                                            Console.WriteLine("----------DISPLAY STATION----------\n");
                                            Console.WriteLine(ibl1.GetStation(id));
                                            break;
                                        }
                                    case EntityOptions.Parcel:
                                        {
                                            Console.WriteLine("Enter the ID of the parcel you want to print");
                                            int.TryParse(Console.ReadLine(), out int id);
                                            Console.WriteLine("----------DISPLAY PARCEL----------\n");
                                            Console.WriteLine(ibl1.GetParcel(id));
                                            break;
                                        }
                                    case EntityOptions.Drone:
                                        {
                                            Console.WriteLine("Enter the ID of the drone you want to print");
                                            int.TryParse(Console.ReadLine(), out int id);
                                            Console.WriteLine("----------DISPLAY DRONE----------\n");
                                            Console.WriteLine(ibl1.GetDrone(id));
                                            break;
                                        }
                                    case EntityOptions.Customer:
                                        {
                                            Console.WriteLine("Enter the ID of the Customer you want to print");
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
                            catch (RetrievalException ex)
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

