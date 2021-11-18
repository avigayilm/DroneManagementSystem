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
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Receiver { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DroneInParcel? Dr {get; set;}
            public DateTime? Created { get; set; }
            public DateTime? Assigned{get; set;}
            public DateTime? PickedUp{get; set;}
            public DateTime? Delivered{get; set;}

        }
    }

}
