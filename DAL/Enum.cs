using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  enum DroneStatuses
    {
        available=0,maintenance,delivery
    }

    public enum WeightCategories
    {
        light=0, medium, heavy
    }

    public enum Priotities
    {
        empty=0,full
    }
}
