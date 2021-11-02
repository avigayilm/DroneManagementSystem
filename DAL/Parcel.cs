using System;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int id { get; set; }
            public string senderid { get; set; }
            public String targetid { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }

            public DateTime requested { get; set; }

            public int droneId { get; set; }
            public DateTime scheduled { get; set; }
            public DateTime pickedUp { get; set; }
            public DateTime Delivered { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is #{ID}, \n";
                result += $"Senderid is {senderid}, \n";
                result += $"Targetid is {targetid}, \n";
                result += $"Weight is {Weight},\n";
                result += $"Priority is {Priority}\n";
                result += $"The parcel was requested at {requested},\n";
                result += $"Droneid is {droneId}, \n";
                result += $" The Parcel is scheduled for {scheduled},\n";
                result += $"The Parcel was picked up at{pickedUp},\n";
                result += $"The parcel was delivered at { Delivered},\n";
                return result;
            }
        }
    }
}
