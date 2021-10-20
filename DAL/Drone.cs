using System;
namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int ID { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery { get; set;    }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is {ID}, \n";
                result += $"Model is {Model}, \n";
                result += $"Maxweight{MaxWeight}";
                result += $"Battery is {Battery}%, \n";// how many percent it is charged
                return result;
            }


        }

    }
}
