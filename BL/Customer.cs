using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Customer
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public Location Loc { get; set; }
            public List<Parcel> ReceivedParcels { get; set; }
            public List<Parcel> SentParcels { get; set; }
            public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"Name is {Name}, \n";
                result += $"Telephone is {PhoneNumber.Substring(0, 3) + '-' + PhoneNumber.Substring(3)}, \n";
                result += $"Latitude is {Bonus.DecimalToSexagesimal(Loc.Latitude, 't')}, \n";
                result += $"longitude is{Bonus.DecimalToSexagesimal(Loc.Longitude, 'n')}, \n";
                return result;
            }
        }
    }
    
}
