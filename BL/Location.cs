using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Location
        {
            public double Longitude { get; set; }//we can't chage location, set it only when we initilize
            public double Latitude { get; set; }

            public override string ToString()
            {
                return " "
                    + $"logitude: {Bonus.DecimalToSexagesimal(Longitude, 'n')}"
                    + $"latitude: {Bonus.DecimalToSexagesimal(Latitude, 't')}\n"
                    ;
            }
        }
    }
}
