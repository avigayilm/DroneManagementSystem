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
        public class Drone
        {
            public int id;
            public string model;
            public WeightCategories weight;
            public double battery;
            //status
            DeliveryInTransfer delintrans;
            Location loc;
        }
    }
 
}
