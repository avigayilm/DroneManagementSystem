using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int droneId { get; set; }
            public int stationId { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"droneId is {droneId}, \n";
                result += $"stationId is {stationId}, \n";
                return result;
            }
        }
    }
};
