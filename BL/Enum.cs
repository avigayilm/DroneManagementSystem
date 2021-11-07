using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
            public enum DroneStatuses
            {
                Available, Maintenance, Delivery
            }
            public enum WeightCategories
            {
                light = 0, medium, heavy
            }

            public enum Priorities
            {
                normal = 0, fast, emergency
            }
    }
}
