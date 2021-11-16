using System;
using IDAL.DO;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {

            public string Id { get; set; }// so no one can change ID
            public String Name { get; set; }
            public String Phone { get; set; }// we make it string so it can have 0 at beginning
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"Name is {Name}, \n";
                result += $"Telephone is {Phone.Substring(0, 3) + '-' + Phone.Substring(3)}, \n";
                result += $"Latitude is {Bonus.DecimalToSexagesimal(Latitude, 't')}, \n";
                result += $"longitude is{Bonus.DecimalToSexagesimal(Longitude, 'n')}, \n";
                return result;
            }
        }
    }
}