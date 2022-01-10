using System;

    namespace DO
    {
        public struct Customer
        {

            public string Id { get; set; }// so no one can change ID
            public String Name { get; set; }
            public String PhoneNumber { get; set; }// we make it string so it can have 0 at beginning
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public bool Deleted { get; set; }
        public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"Name is {Name}, \n";
                result += $"Telephone is {PhoneNumber.Substring(0, 3) + '-' + PhoneNumber.Substring(3)}, \n";
                result += $"Latitude is {Bonus.DecimalToSexagesimal(Latitude, 't')}, \n";
                result += $"longitude is{Bonus.DecimalToSexagesimal(Longitude, 'n')}, \n";
                return result;
            }
        }
    }
