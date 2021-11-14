using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class StationToList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int AvailableSlots { get; set; }
            public int OccupiedSlots { get; set; }
        }
    }

}
