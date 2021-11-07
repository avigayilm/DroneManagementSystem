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
            public string id;
            public string name;
            public string phoneNumber;
            public Location ImHere;
            public List<Parcel> receivedParcels = new();
            public List<Parcel> sentParcels = new();
        }
    }
    
}
