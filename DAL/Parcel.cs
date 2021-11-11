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
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }

            public DateTime? requested { get; set; }

            public int droneId { get; set; }
            public DateTime? scheduled { get; set; }
            public DateTime? pickedUp { get; set; }
            public DateTime? delivered { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is #{id}, \n";
                result += $"Senderid is {senderid}, \n";
                result += $"Targetid is {targetid}, \n";
                result += $"Weight is {weight},\n";
                result += $"Priority is {priority}\n";
                result += $"The parcel was requested at {requested},\n";
                result += $"Droneid is {droneId}, \n";
                result += $" The Parcel is scheduled for {scheduled},\n";
                result += $"The Parcel was picked up at{pickedUp},\n";
                result += $"The parcel was delivered at { delivered},\n";
                return result;
            }
        }
    }
}
