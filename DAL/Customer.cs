using System;
namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {

            public string ID { get; set; }// so no one can change ID
            public String name { get; set; }
            public String phone { get; set; }// we make it string so it can have 0 at beginning
            public double latitude { get; set; }
            public double longitude { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is {id}, \n";
                result += $"Name is {name}, \n";
                result += $"Telephone is {Phone.Substring(0, 3) + '-' + phone.Substring(3)}, \n";
                result += $"Latitude is {(IDAL.DO.DalObject.DalObject.DecimalToSexagesimal(latitude, 't'))}, \n";
                result += $"longitude is{(IDAL.DO.DalObject.DalObject.DecimalToSexagesimal(longitude, 'n'))}, \n";
                return result;
            }
        }
    }
}