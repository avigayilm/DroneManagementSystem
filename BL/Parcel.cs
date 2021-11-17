﻿using System;
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
            public CustomerInDelivery Sender { get; set; }
            public CustomerInDelivery Receiver { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DroneInDelivery? Dr {get; set;}
            public DateTime? Requested { get; set; }
            public DateTime? Scheduled{get; set;}
            public DateTime? PickedUp{get; set;}
            public DateTime? Delivered{get; set;}

        }
    }

}
