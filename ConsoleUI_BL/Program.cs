﻿using System;
using IBL.BO;
using IBL;

namespace ConsoleUI_BL
{
    class Program
    {
        //enums to determine updating, adding etc. choices
        enum MenuOptions { Exit, Add, Update, Show_One, Show_List, Show_Distance }
        enum EntityOptions { Exit, Station, Drone, Customer, Parcel }
        enum UpdateOptions { Exit, Drone, Parcel, Customer, Station,Assignment, Pickedup, Delivery, Recharge, ReleasefromCharge }
        enum ListOptions { Exit, Stations, Drones, Customers, Parcels, UnAssignmentParcels, AvailableChargingStations, DroneCharge }

        //menu function wil go thorough all options for user
        private static void ShowMenu()
        {

            Ibl ibl1 = new BL.BL();

            EntityOptions entityOption;
            MenuOptions menuOption;
            ListOptions listOption;
            UpdateOptions updateOption;
            Console.WriteLine("Welcome to our Parcel service, please make your choice out of the following  ");
            do
            {
                Console.WriteLine("options:\n 1-Add, \n 2 Update,\n,3 Show Item,\n 4-show List\n, 5-Show distance between a point and a station or a customer\n 0-Exit");
                menuOption = (MenuOptions)int.Parse(Console.ReadLine());
                switch (menuOption)
                {
                    case MenuOptions.Add:
                        {
                            Console.WriteLine("what do you want to add?\n 1:Station,\n 2:Drone,\n 3:Customer\n,4:Parcel\n");
                            entityOption = (EntityOptions)int.Parse(Console.ReadLine());
                            switch (entityOption)//what entity to add
                            {
                                case EntityOptions.Station://add station
                                    {
                                        Console.WriteLine("Enter the ID, name, latitude, longitude, and chargeslots\n ");
                                        int.TryParse(Console.ReadLine(), out int stationID);
                                        string inputname = Console.ReadLine();
                                        double.TryParse(Console.ReadLine(), out double latitudeInput);
                                        double.TryParse(Console.ReadLine(), out double longitudeInput);
                                        int.TryParse(Console.ReadLine(), out int slots);
                                        Station tempStat = new()// adds a new station
                                        {
                                            Id = stationID,
                                            Name = inputname,
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

                                        Console.WriteLine("Enter the Id,stationId,weightcategory(0=light,1=medium,2=heavy),stationId for first charging");
                                        int.TryParse(Console.ReadLine(), out int droneID);
                                        int.TryParse(Console.ReadLine(), out int stationID);
                                        WeightCategories maxim = (WeightCategories)int.Parse(Console.ReadLine());
                                        Drone newDrone = new()
                                        {
                                            Id = droneID,
                                            Weight = maxim,
                                        };

                                        ibl1.AddDrone(newDrone,stationID);// add drone to dronelist
                                        break;
                                    }
                                case EntityOptions.Parcel://adds a prcel
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
                            break;
                        }
                    case MenuOptions.Update://update existing entities
                        {
                            Console.WriteLine("what do you want to update?\n 1-Drone\n 2-Parcel\n 3-Customer\n 4-Station\n 5-Assignment,\n 6-Pickedup\n 7-Delivery,\n 8-Recharge\n 9-ReleasefromCharge\n ");
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
                                        int availableChargeSlots= int.Parse(Console.ReadLine());
                                        ibl1.UpdateStation(stationId, name, availableChargeSlots);
                                        break;
                                    }
                                case UpdateOptions.Parcel:
                                    {
                                        break;
                                    }
                                case UpdateOptions.Customer:
                                    {
                                        break;
                                    }
                                case UpdateOptions.Assignment://assign drone to parcel
                                    {
                                        Console.WriteLine("Enter the parcelid for the assignment of the parcel and the droneid.\n");
                                        int parcelId = int.Parse(Console.ReadLine());
                                        int droneId = int.Parse(Console.ReadLine());
                                        dal.ParcelDrone(parcelId, droneId);
                                        dal.ChangeDroneStatus(droneId, DroneStatuses.Delivery);
                                        break;
                                    }
                                case UpdateOptions.Delivery://adds a delivery of parcel by drone
                                    {
                                        Console.WriteLine("Enter the parcelid and the Datetime\n");
                                        int.TryParse(Console.ReadLine(), out int ID);
                                        DateTime.TryParse(Console.ReadLine(), out DateTime time);
                                        dal.ParcelDelivered(ID, time);
                                        break;
                                    }
                                case UpdateOptions.Pickedup://changes drone and parcel accordingly
                                    {
                                        Console.WriteLine("Enter the parcelid and the Datetime\n");
                                        int id = int.Parse(Console.ReadLine());
                                        DateTime.TryParse(Console.ReadLine(), out DateTime time);
                                        dal.ParcelPickedUp(id);
                                        break;
                                    }
                                case UpdateOptions.Recharge://charges a drone using dronecharge entity
                                    {
                                        Console.WriteLine("Enter the droneId and the stationId\n");
                                        int droneId = int.Parse(Console.ReadLine());
                                        int stationId = int.Parse(Console.ReadLine());
                                        dal.SendToCharge(droneId, stationId);
                                        dal.ChangeDroneStatus(droneId, DroneStatuses.Maintenance);
                                        dal.ChangeChargeSlots(stationId, -1);
                                        break;
                                    }

                                case UpdateOptions.ReleasefromCharge:
                                    {
                                        Console.WriteLine("Enter the droneId and the stationId");
                                        int droneId, stationId;
                                        int.TryParse(Console.ReadLine(), out droneId);// gets the droneId and stationId from the user
                                        int.TryParse(Console.ReadLine(), out stationId);
                                        dal.BatteryCharged(droneId, stationId);// sets the battery to 100
                                        dal.ChangeDroneStatus(droneId, DroneStatuses.Available);// changes the dronestatus to available
                                        dal.ChangeChargeSlots(stationId, 1);// one extra chargeslot is free.'
                                        break;
                                    }
                                case UpdateOptions.Exit:
                                    {
                                        break;
                                    }
                            }
                            break;

                        }
                    case MenuOptions.Show_List://displays the lists
                        {
                            Console.WriteLine("what List do you want to print?\n 1-Stations\n, 2-Drones\n, 3-Customers\n, 4-Parcels\n, 5-UnAssignmentParcels\n, 6-AvailableChargingStations\n, 7-DroneCharge");
                            listOption = (ListOptions)int.Parse(Console.ReadLine());
                            switch (listOption)
                            {
                                case ListOptions.Stations://display stations list
                                    {
                                        List<Station> stationListTemp = dal.GetAllStations();
                                        stationListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                        break;
                                    }
                                case ListOptions.Parcels://display parcels list
                                    {
                                        List<Parcel> parcelListTemp = dal.GetParcelList();
                                        parcelListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                        break;
                                    }
                                case ListOptions.Drones://display drones list
                                    {
                                        List<Drone> dronesListTemp = dal.GetAllDrones();
                                        dronesListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                        break;
                                    }
                                case ListOptions.Customers://display customers list
                                    {
                                        List<Customer> customerListTemp = dal.GetAllCustomers();
                                        customerListTemp.ForEach(p => Console.WriteLine(p));
                                        break;
                                    }
                                case ListOptions.UnAssignmentParcels://display unassigned parcels
                                    {
                                        List<Parcel> UnAssignmentListTemp = dal.GetvacantParcel();
                                        UnAssignmentListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                        break;
                                    }
                                case ListOptions.AvailableChargingStations://diplay stations with available charging slots
                                    dal.GetStationWithCharging().ForEach(p => Console.WriteLine(p));
                                    break;

                                case ListOptions.DroneCharge://display drone charge list
                                    {

                                        List<DroneCharge> stationDroneChargeListTemp = dal.GetDroneChargeList();
                                        stationDroneChargeListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                        break;
                                    }
                                case ListOptions.Exit:
                                    {
                                        break;
                                    }
                            }
                            break;
                        }

                    case MenuOptions.Show_One://displays a single entity
                        {
                            Console.WriteLine("what do you want to print?\n 1-Station,\n 2-Drone,\n 3-Customer,\n 4-Parcel\n");
                            entityOption = (EntityOptions)int.Parse(Console.ReadLine());
                            switch (entityOption)
                            {
                                case EntityOptions.Station:
                                    {
                                        Console.WriteLine("Enter the ID of the station you want to print\n");
                                        int.TryParse(Console.ReadLine(), out int id);
                                        Console.WriteLine(dal.GetStation(id));
                                        break;
                                    }
                                case EntityOptions.Parcel:
                                    {
                                        Console.WriteLine("Enter the ID of the parcel you want to print\n");
                                        int.TryParse(Console.ReadLine(), out int id);
                                        Console.WriteLine(dal.GetParcel(id));
                                        break;
                                    }
                                case EntityOptions.Drone:
                                    {
                                        Console.WriteLine("Enter the ID of the drone you want to print\n");
                                        int.TryParse(Console.ReadLine(), out int id);
                                        Console.WriteLine(dal.GetDrone(id));
                                        break;
                                    }
                                case EntityOptions.Customer:
                                    {
                                        Console.WriteLine("Enter the ID of the Customer you want to print\n");
                                        string id = Console.ReadLine();
                                        Console.WriteLine(dal.GetCustomer(id));
                                        break;
                                    }
                                case EntityOptions.Exit:
                                    {
                                        break;
                                    }

                            }
                            break;
                        }
                    case MenuOptions.Show_Distance://to show distance between a point and a station/cusomer
                        {
                            Console.WriteLine("Enter lattitude for required point");
                            double.TryParse(Console.ReadLine(), out double latP);
                            Console.WriteLine("Enter longitude for required point");
                            double.TryParse(Console.ReadLine(), out double lonP);
                            Console.WriteLine("Enter ID, for station 4 digits , for customer 9 ");
                            int ID = int.Parse(Console.ReadLine());
                            dynamic custOrStat = ID > 9999 ? dal.GetCustomer(string.Format($"{ID}")) : dal.GetStation(ID);//determines whether  a station or customer was entered
                            // calls the distance function to determine distance btween the points    
                            Console.WriteLine("The distance is: " + global::Bonus.Haversine(custOrStat.longitude, custOrStat.latitude, lonP, latP) + "KM");//calls haversine function from bonus library

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


            ShowMenu();

        }
    }
}

