﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;

namespace Dal
{ 
    public static class DataSource// the datasource class just initializes all the classes
    {
        //indexer for parcel list
        internal class Config
        {
            internal static int LastParcelNumber = 1000;
            /// <summary>
            /// battery usage per km of flying- battery life is 20 hours of flying empty and takes 3+ hours to charge
            /// </summary>
            internal static double pwrUsgEmpty = 3;
            internal static double pwrUsgLight= 5;
            internal static double pwrUsgMedium = 10;
            internal static double pwrUsgHeavy = 15;
            internal static double chargePH = 150;

        }

        internal static List<Drone> dronesList = new();
        internal static List<Station> stationList = new ();
        internal static List<Customer> customerList = new ();
        internal static List<Parcel> parcelList = new ();
        internal static List<DroneCharge> chargeList = new ();
        internal static List<Login> loginList = new();
        internal static Random rand = new Random();

        public static void Initialize()
        {
            CreateDrone();
            CreateStation();
            CreateCustomer();
            CreateParcel();
            createCharges();
            CreateLogin();
            XMLTools.SaveListToXMLSerializer(dronesList, DroneXml);
            XMLTools.SaveListToXMLSerializer(stationList, StationXml);
            XMLTools.SaveListToXMLSerializer(customerList, CustomerXml);
            XMLTools.SaveListToXMLSerializer(parcelList, ParcelXml);
            XMLTools.SaveListToXMLSerializer(chargeList, DroneChargeXml);
            XMLTools.SaveListToXMLSerializer(loginList, LoginXml);
        }

        private static string DroneXml = @"DroneXml.xml";
        private static string ParcelXml = @"ParcelXml.xml";
        private static string StationXml = @"StationXml.xml";
        private static string DroneChargeXml = @"DroneChargeXml.xml";
        private static string CustomerXml = @"CustomerXml.xml";
        private static string LoginXml = @"LoginXml.xml";
        //initializes five drones
        public static void CreateDrone()
        {   //loop for updete 5 drone
            for (int i = 0; i < 5; i++)
            {
                int id;// checking if the random Id exists already
                do
                {
                    id = rand.Next(1000, 9999);
                }
                while (dronesList.Exists(d => d.Id == id));

                dronesList.Add(new Drone()
                {
                    Id = id,
                    Model = ("A" + rand.Next(0, 10)) + rand.Next(100, 1000).ToString(),
                    Weight = (WeightCategories)rand.Next(3),
                });
            }
        }

        //initializes 10 customers
        static void CreateCustomer()
        {
            string[] customerNames = { "Frodo", "Sam", "Gloin", "Oin", "Sauron", "Saruman", "Elrond", "Galadriel", "Legolas", "Aragorn" };
            //loop for 10 customer
            for (int i = 0; i < 10; i++)
            {
                string id,phone;// checking if the random Id exists
                do
                {
                    id = "" + $"0{rand.Next(100000000, 999999999)}";
                }
                while (customerList.Exists(d => d.Id == id));
                do
                {
                    phone = $"0{rand.Next(50, 58)}-{rand.Next(1000000, 10000000)}";
                }
                while (customerList.Exists(d => d.PhoneNumber==phone ));

                customerList.Add(new Customer()
                {
                    Id = id,
                    Name = customerNames[i],
                    PhoneNumber = $"0{rand.Next(50, 58)}-{rand.Next(1000000, 10000000)}",
                    //Lat-long coorditates for cities in Israel are in range: Latitude from 29.55805 to 33.20733 and longitude from 34.57149 to 35.57212.
                    Latitude = GetRandomNumber(29.55805, 33.20733),//jerusalem range
                    Longitude = GetRandomNumber(34.57149, 35.57212)
                });
            }
        }

        public static void CreateParcel()//initializes 20 parcels
        {
            int count = 0;
            DateTime startDate = new DateTime(2021, 1, 1);
            for (int i = 0; i < 20; i++)
            {

                Parcel temp = new Parcel()
                {
                    Id = DataSource.Config.LastParcelNumber,
                    SenderId = customerList[rand.Next((customerList.Count-1))].Id,// gets a random number of one of the customers
                    ReceiverId = customerList[rand.Next((customerList.Count-1))].Id,
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Created = startDate.AddDays(rand.Next(200)).AddMinutes(rand.Next(24 * 60)),
                   
                };
                if(count < 4)
                {
                    temp.DroneId = dronesList[count].Id;
                    temp.Assigned = ((DateTime)temp.Created).AddMinutes(180);
                    if (count < 2)
                        temp.PickedUp = ((DateTime)temp.Assigned).AddMinutes(60);
                    if (count < 1)
                        temp.Delivered = ((DateTime)temp.PickedUp).AddMinutes(500);
                    count++;

                }
                
    

                parcelList.Add(temp);
                DataSource.Config.LastParcelNumber++;
            }
        
        }
        public static void CreateStation()// initializes 2 stations
        {
            for (int i = 0; i < 4; i++)
            {
                int id;// checking if the random Id exists already
                do
                {
                    id = rand.Next(1000, 9999);
                }
                while (stationList.Exists(d => d.Id == id));
                stationList.Add(new Station()
                {
                    Id = id,
                    Name = $"Station-{(char)('A' + rand.Next(26)) + rand.Next(10)}",
                    AvailableChargeSlots = rand.Next(4, 10),
                    Latitude = GetRandomNumber(29.55805, 33.20733),// values of Jerusalem
                    Longitude = GetRandomNumber(34.57149, 35.57212)
                });
            }
        }
        public static void createCharges()
        {
           
            DroneCharge dc = new() { ChargingTime = DateTime.Now, DroneId = dronesList[4].Id, StationId = stationList[0].Id };
            chargeList.Add(dc);
        }

            public static void CreateLogin()
        {
            loginList.Add(new Login()
            {
                UserName = "Manager",
                Password = "YehuditAvigayil"
            });
            foreach (Customer cus in customerList)
            {
                loginList.Add(new Login()
                {
                    UserName = cus.Id,
                    Password = "miniProject",
                    profileSource = @"C:\Users\Hudis\source\repos\HudiF\DotNet5782_9033_6996\PL\Icons\bbchedva whatsapp.jpeg"

                });
            }
        }
        public static double GetRandomNumber(double minimum, double maximum)// gets a random decimal number between the 2 numbers
        {
            return rand.NextDouble() * (maximum - minimum) + minimum;
        }

    }
}



