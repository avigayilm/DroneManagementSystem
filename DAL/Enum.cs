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
            light, medium, heavy
        }

        public enum Priorities
        {
            empty = 0, full
        }
    }
    
}
