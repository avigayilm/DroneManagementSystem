using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public enum DroneStatuses
        {
            available, maintenance, delivery
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
