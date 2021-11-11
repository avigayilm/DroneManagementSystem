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
            public int id;
            public Customer sender;
            public Customer receiver;
            public WeightCategories weight;
            public Priorities priority;
            public Drone dr;
            public DateTime? requested;
            public DateTime? scheduled;
            public DateTime? pickedUp;
            public DateTime? delivered;

        }
    }

}
