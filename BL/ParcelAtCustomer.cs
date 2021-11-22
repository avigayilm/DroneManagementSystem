using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelAtCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses ParcelStatus { get; set; }
        public CustomerInParcel CustomerInP { get; set; }


        public override string ToString()
        {
            return " "
                + $"Parcel id: {Id}\n"
                + $"Parcel Weight:{Weight}\n "
                + $"Parcel priority:{Priority}\n"
                + $"customer in parcel :{CustomerInP}\n"
                + $"parcel status is: {ParcelStatus}, \n"
                ;
        }
    }
}
