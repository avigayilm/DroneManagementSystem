﻿using System;
using IDAL.DO;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {

            public string id { get; set; }// so no one can change ID
            public String name { get; set; }
            public String phone { get; set; }// we make it string so it can have 0 at beginning
            public double latitude { get; set; }
            public double longitude { get; set; }

            public override string ToString()
            {
                String result = " ";
                result += $"ID is {id}, \n";
                result += $"Name is {name}, \n";
                result += $"Telephone is {phone.Substring(0, 3) + '-' + phone.Substring(3)}, \n";
                //result += $"Latitude is {(IDAL.DO.DalObject.DalObject.DecimalToSexagesimal(latitude, 't'))}, \n";
                result += $"Latitude is {Bonus.DecimalToSexagesimal(latitude, 't')}, \n";
                result += $"longitude is{Bonus.DecimalToSexagesimal(longitude, 'n')}, \n";
                return result;
            }
        }
    }
}