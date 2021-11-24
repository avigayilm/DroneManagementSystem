using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DeliveryInTransfer
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public bool Stat { get; set; }
            public Location Collecting { get; set; }
            public Location Delivering { get; set; }
            double DeliveryDistance { get; set; }

            public override string ToString()
            {
                return " "
                + $"ID is: {Id}\n"
                + $"Priority is: {Priority}\n"
                + $"stat is: {Stat}\n"
                + $"Collecting location: {Collecting}\n"
                + $"Delivering location: {Delivering}\n"
                + $"Delivery distance: {DeliveryDistance}";
            }



        }
    }

}
