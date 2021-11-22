﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class StationToList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int AvailableSlots { get; set; }
            public int OccupiedSlots { get; set; }

            public override string ToString()
            {
                return " "
                    + $"Station id: {Id}\n"
                    + $"Station name: {Name}\n"
                    + $"Station number of available slots: {AvailableSlots}\n"
                    + $"Station number of occupied slots: {OccupiedSlots}\n"
                    ;

            }
        }
    }
}
