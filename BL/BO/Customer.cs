using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace BO
    {
        public class Customer
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public Location Loc { get; set; }
            public List<ParcelAtCustomer> ReceivedParcels { get; set; }
            public List<ParcelAtCustomer> SentParcels { get; set; }
            public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}\n";
                result += $"Name is {Name}\n";
                result += $"Telephone is {PhoneNumber.Substring(0, 3) + '-' + PhoneNumber.Substring(3)}\n";
                result += $"Latitude is {Bonus.DecimalToSexagesimal(Loc.Latitude, 't')}\n";
                result += $"Longitude is{Bonus.DecimalToSexagesimal(Loc.Longitude, 'n')}\n";
                return result;
            }
        }
    }


