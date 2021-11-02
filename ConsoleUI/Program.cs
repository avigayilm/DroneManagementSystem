using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

// trying to see if it works :)



namespace ConsoleUI
{
    class Program
    {
        enum MenuOptions { Exit, Add, Update, Show_One, Show_List, Show_Distance }
        enum EntityOptions { Exit, Station, Drone, Customer, Parcel }
        enum UpdateOptions { Exit, Assignment, Pickedup, Delivery, Recharge }
        enum ListOptions { Exit, Stations, Drones, Customers, Parcels, UnAssignmentParcels, AvailableChargingStations, DroneCharge }

        private static void ShowMenu()
        {
            DAL.DalObject dal = new();
            //static public DalObject obj=new DalObject
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
                            switch (entityOption)
                            {
                                case EntityOptions.Station:
                                    {
                                        Console.WriteLine("Enter the ID, name, latitude, longitude, and chargeslots\n ");


                                        int slots, stationID;
                                        int.TryParse(Console.ReadLine(), out stationID);
                                        string inputname = Console.ReadLine();
                                        double longitudeInput, latitudeInput;
                                        double.TryParse(Console.ReadLine(), out latitudeInput);
                                        double.TryParse(Console.ReadLine(), out longitudeInput);
                                        int.TryParse(Console.ReadLine(), out slots);
                                        Station Victoria = new Station()
                                        {
                                            ID = stationID,
                                            Name = inputname,
                                            ChargeSlots = slots,
                                            Longitude = longitudeInput,
                                            Latitude = latitudeInput,
                                        };
                                        //IDAL.DO.DalObject.DalObject.AddStation(Victoria);
                                        dal.AddStation(Victoria);
                                        break;
                                    }
                                case EntityOptions.Customer:
                                    {

                                        Console.WriteLine("Enter the ID, name, phone, latitude,longtitude\n ");
                                        string inputId = Console.ReadLine();
                                        string inputname = Console.ReadLine();
                                        string inputphone = Console.ReadLine();
                                        double longitudeInput, latitudeInput;
                                        double.TryParse(Console.ReadLine(), out longitudeInput);
                                        double.TryParse(Console.ReadLine(), out latitudeInput);
                                        Customer newCustomer = new()
                                        {
                                            ID = inputId,
                                            name = inputname,
                                            phone = inputphone,
                                            longitude = longitudeInput,
                                            latitude = latitudeInput,


                                        };
                                        //IDAL.DO.DalObject.DalObject.AddCustomer(newCustomer);
                                        dal.AddCustomer(newCustomer);
                                        break;
                                    }
                                case EntityOptions.Drone:
                                    {

                                        Console.WriteLine("Enter the Id, model, weightcategory(0-2),status(0-2), battery percentage\n ");
                                        int droneID;
                                        int.TryParse(Console.ReadLine(), out droneID);
                                        string inputmodel = Console.ReadLine();
                                        WeightCategories maxim = (WeightCategories)int.Parse(Console.ReadLine());
                                        DroneStatuses stat = (DroneStatuses)int.Parse(Console.ReadLine());
                                        double batt;
                                        double.TryParse(Console.ReadLine(), out batt);
                                        Drone newDrone = new Drone()
                                        {
                                            ID = droneID,
                                            Model = inputmodel,
                                            MaxWeight = maxim,
                                            Status = stat,
                                            Battery = batt
                                        };
                                        //IDAL.DO.DalObject.DalObject.AddDrone(newDrone);
                                        dal.AddDrone(newDrone);
                                        break;
                                    }
                                case EntityOptions.Parcel:
                                    {

                                        Console.WriteLine("Enter the senderId, the targetId, weightcatergory(0-2), priority(0,2), date requested, scheduled time\n ");
                                        string inputSenderId = Console.ReadLine();
                                        string inputTargetId = Console.ReadLine();
                                        WeightCategories maxim = (WeightCategories)int.Parse(Console.ReadLine());
                                        Priorities prio = (Priorities)int.Parse(Console.ReadLine());
                                        DateTime req;
                                        DateTime sched;
                                        DateTime.TryParse(Console.ReadLine(), out req);
                                        DateTime.TryParse(Console.ReadLine(), out sched);
                                        Parcel newParcel = new Parcel()
                                        {
                                            senderid = inputSenderId,
                                            targetid = inputTargetId,
                                            weight = maxim,
                                            priority = prio,
                                            requested = req,
                                            scheduled = sched
                                        };
                                        //IDAL.DO.DalObject.DalObject.AddParcel(newParcel);
                                        dal.AddParcel(newParcel);
                                        break;
                                    }


                            }
                            break;
                        }
                    case MenuOptions.Update:
                        {
                            Console.WriteLine("what do you want to update?\n  1-Assignment,\n 2-Pickedup\n 3-Delivery,\n 4-Recharge\n ");
                            updateOption = (UpdateOptions)int.Parse(Console.ReadLine());
                            switch (updateOption)
                            {
                                case UpdateOptions.Assignment:
                                    {
                                        Console.WriteLine("Enter the parcelid for the assignment of the parcel and the droneid.\n");
                                        int parcelId, droneId;
                                        int.TryParse(Console.ReadLine(), out parcelId);
                                        int.TryParse(Console.ReadLine(), out droneId);
                                        //IDAL.DO.DalObject.DalObject.ParcelDrone(parcelId,droneId);
                                        dal.ParcelDrone(parcelId, droneId);
                                        dal.ChangeDroneStatus(droneId, DroneStatuses.Delivery);
                                        break;
                                    }
                                case UpdateOptions.Delivery:
                                    {
                                        Console.WriteLine("Enter the parcelid and the Datetime\n");
                                        int ID;
                                        int.TryParse(Console.ReadLine(), out ID);
                                        DateTime time;
                                        DateTime.TryParse(Console.ReadLine(), out time);
                                        //IDAL.DO.DalObject.DalObject.ParcelDelivered(ID, time);
                                        dal.ParcelDelivered(ID, time);
                                        break;
                                    }
                                case UpdateOptions.Pickedup:
                                    {
                                        Console.WriteLine("Enter the parcelid and the Datetime\n");
                                        int ID;
                                        int.TryParse(Console.ReadLine(), out ID);
                                        DateTime time;
                                        DateTime.TryParse(Console.ReadLine(), out time);
                                        //IDAL.DO.DalObject.DalObject.ParcelPickedUp(ID, time);
                                        dal.ParcelPickedUp(ID);
                                        break;
                                    }
                                case UpdateOptions.Recharge:
                                    {
                                        Console.WriteLine("Enter the parcelid and the Datetime\n");
                                        int droneId, stationId;
                                        int.TryParse(Console.ReadLine(), out droneId);
                                        int.TryParse(Console.ReadLine(), out stationId);
                                        //IDAL.DO.DalObject.DalObject.SendToCharge(droneId, stationId);
                                        dal.SendToCharge(droneId, stationId);
                                        break;
                                    }
                                case UpdateOptions.Exit:
                                    {
                                        break;
                                    }
                            }
                            break;

                        }
                    case MenuOptions.Show_List:
                        {
                            Console.WriteLine("what List do you want to print?\n 1-Stations\n, 2-Drones\n, 3-Customers\n, 4-Parcels\n, 5-UnAssignmentParcels\n, 6-AvailableChargingStations\n, 7-DroneCharge");
                            listOption = (ListOptions)int.Parse(Console.ReadLine());
                            switch (listOption)
                            {
                                case ListOptions.Stations:
                                    {
                                        // List<Station> stationListTemp = IDAL.DO.DalObject.DalObject.DisplayStationList();
                                        List<Station> stationListTemp = dal.DisplayStationList();
                                        stationListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                        break;
                                    }
                                case ListOptions.Parcels:
                                    {
                                        //List<Parcel> parcelListTemp = IDAL.DO.DalObject.DalObject.DisplayParcelList();
                                        List<Parcel> parcelListTemp = dal.DisplayParcelList();
                                        parcelListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                        break;
                                    }
                                case ListOptions.Drones:
                                    {
                                        // List<Drone> dronesListTemp= IDAL.DO.DalObject.DalObject.DisplayDroneList();
                                        List<Drone> dronesListTemp = dal.DisplayDroneList();
                                        dronesListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                        break;
                                    }
                                case ListOptions.Customers:
                                    {
                                        //List<Customer> customerListTemp = IDAL.DO.DalObject.DalObject.DisplayCustomerList();
                                        List<Customer> customerListTemp = dal.GetAllCustomers();
                                        customerListTemp.ForEach(p => Console.WriteLine(p));
                                        break;
                                    }
                                case ListOptions.UnAssignmentParcels:
                                    {
                                        // List<Parcel> UnAssignmentListTemp = IDAL.DO.DalObject.DalObject.DisplayvacantParcel();
                                        List<Parcel> UnAssignmentListTemp = dal.DisplayvacantParcel();
                                        UnAssignmentListTemp.ForEach(p => Console.WriteLine(p.ToString()));
                                        break;
                                    }
                                case ListOptions.AvailableChargingStations:
                                    dal.DisplayStationWithCharging().ForEach(p => Console.WriteLine(p));
                                    break;

                                case ListOptions.DroneCharge:
                                    {
                                        //List<DroneCharge> stationDroneChargeListTemp = IDAL.DO.DalObject.DalObject.DisplayDroneChargeList();
                                        List<DroneCharge> stationDroneChargeListTemp = dal.DisplayDroneChargeList();
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

                    case MenuOptions.Show_One:
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
                                        int ID;
                                        int.TryParse(Console.ReadLine(), out ID);
                                        //Console.WriteLine(IDAL.DO.DalObject.DalObject.DisplayParcel(ID));
                                        Console.WriteLine(dal.GetParcel(ID));
                                        break;
                                    }
                                case EntityOptions.Drone:
                                    {
                                        Console.WriteLine("Enter the ID of the drone you want to print\n");
                                        int ID;
                                        int.TryParse(Console.ReadLine(), out ID);
                                        //Console.WriteLine(IDAL.DO.DalObject.DalObject.DisplayDrone(ID));
                                        Console.WriteLine(dal.GetDrone(ID));

                                        break;
                                    }
                                case EntityOptions.Customer:
                                    {
                                        Console.WriteLine("Enter the ID of the Customer you want to print\n");
                                        string ID = Console.ReadLine();
                                        //Console.WriteLine(IDAL.DO.DalObject.DalObject.DisplayCustomer(ID));
                                        Console.WriteLine(dal.GetCustomer(ID));
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
                            double latP, lonP;
                            Console.WriteLine("Enter lattitude for required point");
                            double.TryParse(Console.ReadLine(), out latP);
                            Console.WriteLine("Enter longitude for required point");
                            double.TryParse(Console.ReadLine(), out lonP);
                            Console.WriteLine("Enter ID, for station 4 digits , for customer 9 ");
                            int ID = int.Parse(Console.ReadLine());
                            // Console.WriteLine("The distance is: " + IDAL.DO.DalObject.DalObject.Distance(ID,lonP, latP) + "KM");// calls the distance function to determine distance btween the points
                            dynamic custOrStat = ID > 9999 ? dal.GetCustomer(string.Format($"{ID}")) : dal.GetStation(ID);
                            // calls the distance function to determine distance btween the points    
                            Console.WriteLine("The distance is: " + global::Bonus.Haversine(custOrStat.longitude, custOrStat.latitude, lonP, latP) + "KM");

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
