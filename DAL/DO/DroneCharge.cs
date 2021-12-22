using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace DO
    {
        public struct DroneCharge
        {
            public int DroneId { get; set; }
            public int StationId { get; set; }
            public DateTime ChargingTime { get; set;}

            public override string ToString()
            {
                String result = " ";
                result += $"droneId is {DroneId}, \n";
                result += $"stationId is {StationId}, \n";
                return result;
            }
        }
    }
