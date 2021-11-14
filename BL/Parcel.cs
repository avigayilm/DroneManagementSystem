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
            public int Id;
            public string SenderId;
            public string ReceiverId;
            public WeightCategories Weight;
            public Priorities Priority;
            public Drone? Dr;
            public DateTime? Requested;
            public DateTime? Scheduled;
            public DateTime? PickedUp;
            public DateTime? Delivered;

        }
    }

}
