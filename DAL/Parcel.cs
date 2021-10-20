using System;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int ID { get; set; }
            public string Senderid { get; set; }
            public String Targetid { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }

            public DateTime requested { get; set; }

            public int DroneId { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is #{ID}, \n";
                result += $"Senderid is {Senderid}, \n";
                result += $"Targetid is {Targetid}, \n";
                result += $"Weight is {Weight},\n";
                result += $"Priority is {Priority}\n";
                result += $"The parcel was requested at {requested},\n";
                result += $"Droneid is {DroneId}, \n";
                result += $" The Parcel is scheduled for {Scheduled},\n";
                result += $"The Parcel was picked up at{PickedUp},\n";
                result += $"The parcel was delivered at { Delivered},\n";
                return result;
            }
        }
    }
}
