using System;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int ID { get; set; }
            public int senderId { get; set; }
            public int targetId { get; set; }
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }

            public DateTime requested { get; set; }

            public int droneId { get; set; }
            public DateTime acheduled { get; set; }
            public DateTime pickedUp { get; set; }
            public DateTime delivered { get; set; }
        }
    }
}
