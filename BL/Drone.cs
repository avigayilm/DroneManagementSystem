using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Drone
        {
            public int id;
            public string model;
            public WeightCategories maxwWeight;
            public double battery;
            public DroneStatuses status;
            public DeliveryInTransfer delintrans;
            public Location loc;
        }
    }
 
}
