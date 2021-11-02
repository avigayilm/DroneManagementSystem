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
            public int id { get; set; }
            public string name { get; set; }
            public double longitude { get; set; }
            public double latitude { get; set; }
            public int chargeSlots { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is {id}, \n";
                result += $"Name is {name}, \n";
                result += $"Latitude is {(Bonus.DecimalToSexagesimal(latitude, 't'))}, \n";
                result += $"longitude is {(Bonus.DecimalToSexagesimal(longitude, 'n'))}, \n";
                result += $"There are {chargeSlots} Chargeslots\n";
                return result;
            }
        }

    }
}
