using System;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public string Sender { get; set; }
            public string Target { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }

            public DateTime? Requested { get; set; }

            public int DroneId { get; set; }
            public DateTime? Scheduled { get; set; }
            public DateTime? PickedUp { get; set; }
            public DateTime? Delivered { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is #{Id}, \n";
                result += $"Senderid is {Sender}, \n";
                result += $"Targetid is {Target}, \n";
                result += $"Weight is {Weight},\n";
                result += $"Priority is {Priority}\n";
                result += $"The parcel was requested at {Requested},\n";
                result += $"Droneid is {DroneId}, \n";
                result += $" The Parcel is scheduled for {Scheduled},\n";
                result += $"The Parcel was picked up at{PickedUp},\n";
                result += $"The parcel was delivered at { Delivered},\n";
                return result;
            }
        }
    }
}
