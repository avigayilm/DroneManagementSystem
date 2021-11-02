using System;
namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int id { get; set; }
            public string model { get; set; }
            public WeightCategories maxWeight { get; set; }
            public DroneStatuses status { get; set; }
            public double battery { get; set;    }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is {id}, \n";
                result += $"Model is {model}, \n";
                result += $"Maxweight is {maxWeight}\n";
                result += $"Battery is {battery}%, \n";// how many percent it is charged
                return result;
            }


        }

    }
}
