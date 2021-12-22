using System;

    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public string SenderId { get; set; }
            public string ReceiverId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public int DroneId { get; set; }
            public DateTime? Created { get; set; }
            public DateTime? Assigned { get; set; }
            public DateTime? PickedUpTime { get; set; }
            public DateTime? Delivered { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is #{Id}, \n";
                result += $"Senderid is {SenderId}, \n";
                result += $"Targetid is {ReceiverId}, \n";
                result += $"Weight is {Weight},\n";
                result += $"Priority is {Priority}\n";
                result += $"The parcel was requested at {Created},\n";
                result += $"Droneid is {DroneId}, \n";
                result += $" The Parcel is scheduled for {Assigned},\n";
                result += $"The Parcel was picked up at{PickedUpTime},\n";
                result += $"The parcel was delivered at { Delivered},\n";
                return result;
            }
        }
    }

