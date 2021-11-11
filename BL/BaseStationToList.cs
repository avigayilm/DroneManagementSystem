using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class BaseStationToList
        {
            public int id { get; set; }
            public string name;
            public int availableSlots;
            public int occupiedSlots;
        }
    }

}
