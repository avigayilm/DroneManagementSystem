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
            public int id { get; set; }
            public Customer sender { get; set; }
            public Customer receiver { get; set; }
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }
            public Drone dr {get; set;}
            public DateTime? requested { get; set; }
            public DateTime? scheduled{get; set;}
            public DateTime? pickedUp{get; set;}
            public DateTime? delivered{get; set;}

        }
    }

}
