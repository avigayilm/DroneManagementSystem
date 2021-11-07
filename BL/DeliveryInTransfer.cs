using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IBL
{
    namespace BO
    {
        public class DeliveryInTransfer
        {
            public int id;
            public WeightCategories weight;
            public Priorities priority;
            //status Boolean
            public Location collecting;
            public Location delivering;
            double deliveryDistance;




        }
    }
   
}
