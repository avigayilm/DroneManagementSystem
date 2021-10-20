using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int ID { get; set; }
            public int name { get; set; }
            public double longitude { get; set; }
            public double latitude { get; set; }
            public int chargeSlots { get; set; }
        }

    }
}
