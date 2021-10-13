using System;

namespace IDAL
{
    namespace DO
    {
        enum Hello { WeightCategories, Priorities };
        public struct Customer
    {
       
        public int ID { get; set; }// so no one can changeID
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

                public DateTime Requestion { get; set; }

                public int Droneld { get; set; }
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

            public struct Station
            {
                public int ID { get; set; }
                public int Name { get; set; }
                public double Longitude { get; set; }
                public double Latitude { get; set; }
                public int ChargeSlots { get; set; }

            }

            public struct DroneCharge
            {
                public int DroneId { get; set; }
                public int StationId { get; set; }
            }

        }
     }

}

namespace DalObject
{
    internal class DataSource
    {
        IDAL.DO.Drone[] DronesArr = new IDAL.DO.Drone[10];
        IDAL.DO.Station[] StationArr = new IDAL.DO.Station[5];
        IDAL.DO.Customer[] CustomerArr = new IDAL.DO.Customer[100];
        IDAL.DO.Parcel[] ParcelArr = new IDAL.DO.Parcel[10];


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
            
        }
    }


}
}
