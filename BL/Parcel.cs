using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public Customer Sender { get; set; }
            public Customer Receiver { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public Drone Dr? {get; set;}
            public DateTime? Requested { get; set; }
            public DateTime? Scheduled{get; set;}
            public DateTime? PickedUp{get; set;}
            public DateTime? Delivered{get; set;}

        }
    }

}
