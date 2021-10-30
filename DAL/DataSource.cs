using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace IDAL
{
    namespace DO
    {
        namespace DalObject
        {
            public class DataSource// the datasource class just initializes all the classes
            {
                internal static List<Drone> dronesList = new List<Drone>(10);
                internal static List<Station> stationList = new List<Station>(5);
                internal static List<Customer> customerList = new List<Customer>(100);
                internal static List<Parcel> parcelList = new List<Parcel>(1000);
                internal static List<DroneCharge> chargeList = new List<DroneCharge>(10);
                public static int PackageCounter = 1000;
                static Random rand = new Random();
                public static void Initialize()
                {
                    createDrone();
                    createStation();
                    createCustomer();
                    createParcel();

                }
                public static void createDrone()
                {   //loop for updete 5 drone
                    for (int i = 0; i < 5; i++)
                    {
                        dronesList[i] = new Drone()
                        {
                            ID = rand.Next(1000,9999),
                            Model = ("A" + rand.Next(0, 10)) + rand.Next(100, 1000).ToString(),
                            MaxWeight = (WeightCategories)rand.Next(3),
                            Status = DroneStatuses.available,
                            Battery = 100.0
                        };
                       // Config.droneIndex++;
                    }
                }

                public static void createCustomer()
                {
                    string[] customerNames = { "Frodo", "Sam", "Gloin", "Oin", "Sauron", "Saruman", "Elrond", "Galadriel", "Legolas", "Aragorn" };
                    //lop for 10 customer
                    for (int i = 0; i < 10; i++)
                    {
                        customerList[i] = new Customer
                        {
                            ID = $"0{rand.Next(100000000, 999999999)}",
                            name = customerNames[i],
                            phone = $"0{rand.Next(50, 58)}-{rand.Next(1000000, 10000000)}",
                            //Lat-long coorditates for cities in Israel are in range: Latitude from 29.55805 to 33.20733 and longitude from 34.57149 to 35.57212.
                            latitude = GetRandomNumber(29.55805, 33.20733),
                            longitude = GetRandomNumber(34.57149, 35.57212)
                        };
                    }
                }
                public static void createParcel()
                {
                    DateTime temp = DateTime.Now;
                    for (int i = 0; i < 10; i++)
                    {
                        parcelList[i] = new Parcel()
                        {
                            ID = DataSource.PackageCounter++,
                            Senderid = customerList[rand.Next(customerList.Count)].ID,// gets a random number of one of the customers
                            Targetid = customerList[rand.Next(customerList.Count)].ID,
                            Weight = (WeightCategories)rand.Next(3),
                            Priority = (Priorities)rand.Next(3),
                            requested = temp.AddDays(Math.Pow(i, 2))//random date
                        };
                        PackageCounter++;
                    }
                }
                public static void createStation()// maybe just one
                {
                    for (int i = 0; i < 2; i++)
                    {
                        stationList[i] = new Station
                        {
                            ID = rand.Next(1000, 9999),
                            Name = $"Station {'A' + rand.Next(10)}",
                            ChargeSlots = rand.Next(10),
                            Latitude = GetRandomNumber(29.55805, 33.20733),
                            Longitude = GetRandomNumber(34.57149, 35.57212)
                        };
                    }
                }
                public static double GetRandomNumber(double minimum, double maximum)
                {
                    return rand.NextDouble() * (maximum - minimum) + minimum;
                }

            }
        }
    }
}



