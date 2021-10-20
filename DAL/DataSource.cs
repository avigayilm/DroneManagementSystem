using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    internal class DataSource
    {
        //string[] prefices = new string[] {"058", "054", "053", "052"}
        
        public static Drone[] DronesArr = new Drone[10];
        public static Station[] StationArr = new IDAL.DO.Station[5];
        public static Customer[] CustomerArr = new Customer[100];
        public static Parcel[] ParcelArr = new Parcel[1000];


        internal class Config
        {
            internal static int droneIndex = 0;
            internal static int stationIndex = 0;
            internal static int customerIndex = 0;
            internal static int parcelIndex = 0;
            // public int runnerID;

        }

        static Random r = new Random();
        public static void Initialize()
        {
            creatDrone();
            createStation();
            createCustomer();
            createParcel();
        }    
                   public static void createDrone()
                   {   //loop for updete 5 drone
                        for (int i = Config.droneIndex; i < 5; i++)
			            {
                         DataSource.DronesArr[i].ID = Config.droneIndex ;
                         DataSource.DronesArr[i].Model = ("A" + r.Next(0, 10)) + string(r.Next(100, 1000));
                         DataSource.DronesArr[i].MaxWeight = r.Next(0, 3) ;
                         DataSource.DronesArr[i].Battery = r.Next(0.0, 100.0);;
                         DataSource.DronesArr[i].Status = r.Next(0, 3);
                         Config.parcelIndex++;
			            }
                   }

                    public static createStation()
                   { //lop for 2 station
                    for (int i = DataSource.Config.stationIndex, string str ='A' ; i < 2; i++,str++)
			            {
                        DataSource.StationArr[i].ID = Config.stationIndex;
                        DataSource.StationArr[i].Name = str;
                        DataSource.StationArr[i].Longitude = r.Next(-180, 180) ;
                        DataSource.StationArr[i].Lattitude = r.Next(-90, 90) ;
                        DataSource.StationArr[i].ChargeSlots = r.Next(1, 10);
			            }
                    string[] customerNames { "Frodo","Sam", "Gloin", "Oin", "Sauron", "Saruman", "Elrond", "Galadriel", "Legolas", "Aragorn" };
                    //lop for 10 customer
                    for (int i = customerIndex; i < 10; i++)
			        {
                        CustomerArr[i].longitude= r.Next(-180, 180);
                        CustomerArr[i].lattitude= r.Next(-90, 90);
                        CustomerArr[i].Phone = "05" + string(r.Next(00000000,99999999));
                        CustomerArr[i].ID = string(r.Next(0,5) + string(r.Next(00000000,99999999);
                        CustomerArr[i].Name = customerNames[i];
			        }
                     
                                                

                    for (int i = parcelIndex; i < 10; i++)
			        {
                        ParcelArr[i].id = Config.parcelIndex;
                        ParcelArr[i].senderid = num4;
                        ParcelArr[i].targetId = num4;
                        DateTime temp = DateTime.Now.AddDays(6);
                        //DateTime currentDate = new DateTime(Random, Random, Random);
                        //string printDate = currentDate.ToString("dd/MM/yyyy");
                        DataSource.ParcelArr[i].Requested = temp.AddDays(Math.Pow(i, 2));   
                        DataSource.ParcelArr[i].DroneId = 0;    
                        //DataSource.ParcellArr[i].Scheduled = temp.AddDays(Math.Pow(2, i));
                        //DataSource.ParcelArr[i].PickedUp = temp.AddDays(Math.Pow(2, i) + i);
                        //DataSource.ParcelArr[i].Delivered = temp.AddDays(Math.Pow(2, i) + (2*i));
                        DataSource.ParcelArr[i].Weight = r.Next(0, 3);
                        DataSource.ParcelArr[i].Priority = r.Next(0, 3);
                        Config.parcelIndex ++;
			        }
                }
    }
}