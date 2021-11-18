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
            public int Id { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int AvailableChargeSlots { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"Name is {Name}, \n";
                result += $"Latitude is {(Bonus.DecimalToSexagesimal(Latitude, 't'))}, \n";
                result += $"longitude is {(Bonus.DecimalToSexagesimal(Longitude, 'n'))}, \n";
                result += $"There are {AvailableChargeSlots} Chargeslots\n";
                return result;
            }
        }

    }
}
