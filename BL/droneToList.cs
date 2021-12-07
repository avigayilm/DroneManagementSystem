using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneToList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories Weight { get; set; }
            public int Battery { get; set; }
            public DroneStatuses Status { get; set; }
            public Location Loc { get; set; }
            public int ParcelId { get; set; }

            public override string ToString()
            {
                return " "
                    + $"Drone id: {Id}\n"
                    + $"Drone model: {Model}\n"
                    + $"Drone Weight: {Weight}\n"
                    + $"Drone battery level: {Battery}%\n"
                    + $"Drone status: {Status}\n"
                    + $"Drone location: {Loc}\n"
                    + $"Drone parcel being delivered id: {ParcelId}\n"
                    ;
            }
        }
    }
}
