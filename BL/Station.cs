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
            public int id;
            public string name;
            public Location loc;
            public int chargeslots;
            public List<Drone> charging;
        }
    }
}
