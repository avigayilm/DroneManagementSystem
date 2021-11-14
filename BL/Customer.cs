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
        }
    }
    
}
