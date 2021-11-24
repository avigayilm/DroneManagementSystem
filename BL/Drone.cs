using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories Weight { get; set; }
            public double Battery { get; set; }
            public DroneStatuses Status { get; set; }
            public ParcelInTransfer ParcelInTrans { get; set; }
            public Location Loc { get; set; }

            public override string ToString()
            {
                return " "
                + $"ID is {Id}\n"
                + $"Model is {Model}\n"
                + $"Maxweight is {Weight}\n"
                + $"Battery is {Battery}%\n"
                + $"drone status is {Status}\n "
                + $"parcel in transfer is {ParcelInTrans}\n"
                + $"drone location {Loc}\n"
                ;
            }
        }
    }

}
