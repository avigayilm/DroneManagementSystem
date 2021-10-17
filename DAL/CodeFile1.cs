using System;
using IDAL.DO;
//using DalObject;

namespace IDAL
{
    namespace DO
    {
        enum Hello { WeightCategories, Priorities };
        public struct Customer
    {
       
        public int ID { get; set; }// so no one can change ID
        public String Name { get; set; }
        public String Phone { get; set; }// we make it string so it can have 0 at beginning
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            String result = " ";
            result += $"ID is {ID}, \n";
            result += $"Name is {Name}, \n";
            result += $"Telephone is {Phone.Substring(0, 3) + '-' + Phone.Substring(3)}, \n";
            result += $"Latitude is {Latitude}, \n";
            result += $"longitude is {Longitude}, \n";
            return result;
        }
    }

            public struct Parcel
            {
                public int ID { get; set; }
                public int Senderid { get; set; }
                public int Targetid { get; set; }
                public WeightCategories Weight { get; set; }
                public Priorites Priority { get; set; }

                public DateTime Requested { get; set; }

                public int DroneId { get; set; }
                public DateTime Scheduled { get; set; }
                public DateTime PickedUp { get; set; }
                public DateTime Delivered { get; set; }
            }

            public struct Drone
            {
                public int ID { get; set; }
                public string Model { get; set; }
                public WeightCategories MaxWeight { get; set; }
                public DroneStatuses Status { get; set; }
                public double Battery { get; set; }


            }

        }
     }


namespace DalObject
{
    internal class DataSource
    {
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
        public static void Initialize()
        {
            // initializing the Drone array
            DronesArr[0].ID = 5;
            DronesArr[0].Model = "hello";
            DronesArr[0].Battery = 5.6;

            //initializing the Station array
            StationArr[0].ID = 23;
            StationArr[0].Latitude = 3435;
            StationArr[0].Longitude = 387434;
            StationArr[0].Name = 3;

            //intialzing the Custommer array
            CustomerArr[0].ID = 34;
            CustomerArr[0].Latitude = 34;
            CustomerArr[0].Phone = "0986";
            CustomerArr[0].Longitude = 6764;

            //initializing the Parcel array
            ParcelArr[0].ID = 876;
            CustomerArr[0].Latitude = 987;
            CustomerArr[0].Longitude = 876;
            CustomerArr[0].Phone = "34533";


        }
    }
    public class DalObject
    {
        public void addStation(Station Victoria)
        {
            if (DataSource.Config.stationIndex <= 4)// there are max 5 places in the array
            {
                DataSource.StationArr[DataSource.Config.stationIndex] = Victoria;
                DataSource.Config.stationIndex++;
            }
            else
                Console.WriteLine("Error!overflow in array");

        }

        public void addDrone(Drone Flyboy)
        {
            if (DataSource.Config.droneIndex <= 9)// there are max 5 places in the array
            {
                DataSource.DronesArr[DataSource.Config.droneIndex] = Flyboy;
                DataSource.Config.droneIndex++;
            }
            else
                Console.WriteLine("Error!overflow in array");

        }

        public void addCustomer(Customer me)
        {
            if (DataSource.Config.customerIndex <= 99)// there are max 5 places in the array
            {
                DataSource.CustomerArr[DataSource.Config.customerIndex] = me;
                DataSource.Config.customerIndex++;
            }
            else
                Console.WriteLine("Error!overflow in array");

        }

        public void addParcel(Parcel Fedex)
        {
            if (DataSource.Config.parcelIndex <= 9)// there are max 5 places in the array
            {
                DataSource.ParcelArr[DataSource.Config.parcelIndex] = Fedex;
                DataSource.Config.parcelIndex++;
            }
            else
                Console.WriteLine("Error!overflow in array");
            //adding the parcels in a sorted wat so that it easy to find the parcels
        }

        public void ParcelDrone(int ParcelId,int DroneId)
        {
            int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
            int index = -1;
            for(int i=0;i<=size;i++)
            {
                if (DataSource.ParcelArr[i].ID == ParcelId)// if the parcel was found
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)// if the value is not found
                Console.WriteLine("Parcel not found");
            //update the parcel values
            else
            {
                DataSource.ParcelArr[index].DroneId = DroneId;//updating the DroneId of hte parcel
            }


        }

        public void ParcelPickedUp(int parcelId, DateTime day )
        {
            int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
            int index = -1;
            for (int i = 0; i <= size; i++)
            {
                if (DataSource.ParcelArr[i].ID == parcelId)// if the parcel was found
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)// if the value is not found
                Console.WriteLine("Parcel not found");
            //update the parcel values
            else
            {
                DataSource.ParcelArr[index].PickedUp = day;//updating the DroneId of hte parcel
            }
        }

        public void ParcelDelivered(int parcelId, DateTime day)
        {
            int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
            int index = -1;
            for (int i = 0; i <= size; i++)
            {
                if (DataSource.ParcelArr[i].ID == parcelId)// if the parcel was found
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)// if the value is not found
                Console.WriteLine("Parcel not found");
            //update the parcel values
            else
            {
                DataSource.ParcelArr[index].Delivered = day;//updating the DroneId of hte parcel
            }
        }

        public DroneCharge SendToCharge(int DroneId, int StationId)
        {
            int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
            int index = -1;
            DroneCharge DC = new DroneCharge();// making a new Dronecharge
            for (int i = 0; i <= size; i++)
            {
                if (DataSource.DronesArr[i].ID == DroneId)// if the parcel was found
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)// if the value is not found
            {
                Console.WriteLine("Parcel not found");// return a dronecharge
                return DC;
            }

            //update the parcel values
            else
            {
                //   DataSource.DronesArr[index].Status= ;//updating the DroneId of hte parcel

            }
            
            DC.DroneId = DroneId;
            DC.StationId = StationId;
            return DC;

        }

        public void BatteryCharged (DroneCharge Buzzer)
        {
            int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
            int index = -1;
            for (int i = 0; i <= size; i++)
            {
                if (DataSource.DronesArr[i].ID == Buzzer.DroneId)// if the parcel was found
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)// if the value is not found
                Console.WriteLine("Parcel not found");
            //update the parcel values
            else
            {
                //   DataSource.DronesArr[index].Status= ;//updating the DroneId of hte parcel

            }
        }


        





    }


}

