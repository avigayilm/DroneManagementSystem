using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL
    {
        int BatteryUsage(double distance, int pwrIndex)
        {
            return (int)(idal1.DronePwrUsg()[pwrIndex] * distance);
        }
        //public bool EnoughBattery(double distance, double battery, WeightCategories weight)
        //{

        //}

        void UpdateBattery()
        {

        }
    }
}
