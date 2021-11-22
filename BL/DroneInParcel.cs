using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneInParcel
        {
            public int Id { get; set; }
            public double Battery { get; set; }
            public Location Loc { get; set; }

            public override string ToString()
            {
                return " "
                    + $" drone id: {Id} \n"
                    + $"drone battery: {Battery} \n"
                    + $"drone location: {Loc} \n"
                    ;
            }
        }
    }
}
