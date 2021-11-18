using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelInTransfer
        {
            public int Id { get; set; }
            public Priorities Priority { get; set; }
            public bool status { get; set; }// 1=not picked up or on it's way
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Receiver { get; set; }
            public WeightCategories Weight { get; set; }
            public Location PickedUp { get; set; }
            public Location DeliverdTo { get; set; }
            public double Distance { get; set; }
        }
    }
}
