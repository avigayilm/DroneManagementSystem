using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace BO
    {
        public class Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int AvailableChargeSlots { get; set; }
            public Location Loc { get; set; }
            public IEnumerable<DroneInCharge> Charging { get; set; }

            public override string ToString()
            {
                return String.Join(" ", Charging)
                    + $"Station id: {Id}\n"
                    + $"Station Name:{Name}\n"
                    + $"Stations available Slots:{AvailableChargeSlots}\n"
                    + $"Station location:{Loc}\n"
                    ;
            }

        }
    }

