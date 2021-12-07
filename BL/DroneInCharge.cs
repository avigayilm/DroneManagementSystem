using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneInCharge
        {
            public int Id { get; set; }
            public int Battery { get; set; }

            public override string ToString()
            {
                return " "
                    + $" drone id: {Id}\n"
                    + $"drone battery: {Battery}\n"
                    ;
            }
        }
    }
}
