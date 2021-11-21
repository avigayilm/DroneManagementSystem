using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL
    {
        double BatteryUsage(double distance, int pwrIndex)
        {
            return idal1.DronePwrUsg()[pwrIndex] * distance;
        }
        //public bool EnoughBattery(double distance, double battery, WeightCategories weight)
        //{

        //}

        void UpdateBattery()
        {

        }
    }
}
