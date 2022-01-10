﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BL
{

    public partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal int BatteryUsage(double distance, int pwrIndex)
        {
            lock (idal1)
            {
                return (int)(idal1.DronePwrUsg()[pwrIndex] * distance);
            }
           
        }
    }
}
