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
            public Priorities Priority { get; set; 
            public bool stat { get; set; }
            public Location collecting { get; set; }
            public Location delivering { get; set; }
            double deliveryDistance { get; set; }




        }
    }
   
}
