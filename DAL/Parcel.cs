using System;

namespace IDAL
{
    namespace DO
    {
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
    }
}
