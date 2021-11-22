using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelToList
        {
            public int Id { get; set; }
            public string Sender { get; set; }
            public string Receiver { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            // package status


            public override string ToString()
            {
                return " "
                    + $"Parcel id: {Id}\n"
                    + $"Parcel Sender:{Sender}\n"
                    + $"Parcel Receiver:{Receiver}\n"
                    + $"Parcel Weight:{Weight}\n "
                    + $"Parcel priority:{Priority}\n"
                    ;
            }
        }
    }
}
