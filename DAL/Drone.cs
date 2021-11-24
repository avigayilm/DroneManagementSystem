using System;
namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories Weight { get; set; }
            //public DroneStatuses status { get; set; }
            //public double battery { get; set;    }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"Model is {Model}, \n";
                result += $"Maxweight is {Weight}\n";
                //result += $"Battery is {battery}%, \n";// how many percent it is charged
                return result;
            }


        }

    }
}
