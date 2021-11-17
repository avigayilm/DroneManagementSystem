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
            public int Id;
            public Priorities Priority;
            public bool status;// 1=not picked up or on it's way
            public CustomerInDelivery Sender;
            public CustomerInDelivery Receiver;
            public WeightCategories Weight;
            public Location PickedUp;
            public Location DeliverdTo;
            public double Distance;
        }
    }
}
