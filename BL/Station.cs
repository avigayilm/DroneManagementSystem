using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        public class Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Chargeslots { get; set; }
            public Location Loc { get; set; }
            public List<Drone> Charging { get; set; }
        }
    }
}
