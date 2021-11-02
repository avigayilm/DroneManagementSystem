using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DAL
{
    public class DataSource// the datasource class just initializes all the classes
    {
        internal class Config
        {
            internal static int LastParcelNumber = 1000;
        }

        internal static List<Drone> dronesList = new();
        internal static List<Station> stationList = new();
        internal static List<Customer> customerList = new();
        internal static List<Parcel> parcelList = new();
        internal static List<DroneCharge> chargeList = new();       
        internal static Random rand = new Random();
        public static void Initialize()
        {
            CreateDrone();
            CreateStation();
            CreateCustomer();
            CreateParcel();

        }
        public static void CreateDrone()
        {   //loop for updete 5 drone
            for (int i = 0; i < 5; i++)
            {
                //dronesList[i] = new Drone();
                dronesList.Add(new Drone()
                {
                    id = rand.Next(1000, 9999),
                    model = ("A" + rand.Next(0, 10)) + rand.Next(100, 1000).ToString(),
                    maxWeight = (WeightCategories)rand.Next(3),
                    status = DroneStatuses.Available,
                    battery = GetRandomNumber(20, 100)
                });
                // Config.droneIndex++;
            }
        }

        static void CreateCustomer()
        {
            string[] customerNames = { "Frodo", "Sam", "Gloin", "Oin", "Sauron", "Saruman", "Elrond", "Galadriel", "Legolas", "Aragorn" };
            //lop for 10 customer
            for (int i = 0; i < 10; i++)
            {
                //customerList[i] = new Customer();
                customerList.Add(new()
                {
                    id = $"0{rand.Next(100000000, 999999999)}",
                    name = customerNames[i],
                    phone = $"0{rand.Next(50, 58)}-{rand.Next(1000000, 10000000)}",
                    //Lat-long coorditates for cities in Israel are in range: Latitude from 29.55805 to 33.20733 and longitude from 34.57149 to 35.57212.
                    latitude = GetRandomNumber(29.55805, 33.20733),
                    longitude = GetRandomNumber(34.57149, 35.57212)
                });
            }
        }

        public static void CreateParcel()
        {
            DateTime startDate = new DateTime(2021, 1, 1);
            for (int i = 0; i < 20; i++)
            {
                Parcel temp = new()
                {
                    id = ++DataSource.Config.LastParcelNumber,
                    senderid = customerList[rand.Next(customerList.Count)].id,// gets a random number of one of the customers
                    targetid = customerList[rand.Next(customerList.Count)].id,
                    weight = (WeightCategories)rand.Next(3),
                    priority = (Priorities)rand.Next(3),
                    requested = startDate.AddDays(rand.Next(200)).AddMinutes(rand.Next(24 * 60)),
                    droneId = 0
                };

                int statusStats = rand.Next(100);
                if (statusStats >= 10) // scheduled
                {
                    temp.scheduled = temp.requested.AddMinutes(180);

                    if (statusStats >= 20) // picked up
                    {
                        temp.pickedUp = temp.scheduled.AddMinutes(60);
                        if (statusStats >= 30) // delivered
                        {
                            temp.delivered = temp.pickedUp.AddMinutes(60);
                            temp.droneId = dronesList[rand.Next(dronesList.Count)].id;
                        }
                    }
                    if (temp.droneId == 0)
                    {
                        int dIndex = 0;
                        dIndex = dronesList.FindIndex(d => d.status == DroneStatuses.Available
                                                          && d.maxWeight <= temp.weight
                                                          && d.battery >= 30);
                        var drone = dronesList[dIndex];
                        temp.droneId = drone.id;
                        drone.status = DroneStatuses.Delivery;
                        dronesList[dIndex] = drone;
                    }
                }

                parcelList.Add(temp);
                DataSource.Config.LastParcelNumber++;
            }
        }
        public static void CreateStation()// maybe just one
        {
            for (int i = 0; i < 2; i++)
            {
                stationList.Add(new()
                {
                    id = rand.Next(1000, 9999),
                    name = $"Station-{(char)('A' + rand.Next(26)) + rand.Next(10)}",
                    chargeSlots = rand.Next(2, 10),
                    latitude = GetRandomNumber(29.55805, 33.20733),// values of Jerusalem
                    longitude = GetRandomNumber(34.57149, 35.57212)
                });
            }
        }
        public static double GetRandomNumber(double minimum, double maximum)// gets a random decimal number between the 2 numbers
        {
            return rand.NextDouble() * (maximum - minimum) + minimum;
        }

    }
}



