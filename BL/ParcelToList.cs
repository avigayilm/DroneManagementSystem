using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace BO
    {
        public class ParcelToList
        {
            public int Id { get; set; }
            public string SenderId { get; set; }
            public string ReceiverId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatuses parcelStatus { get; set; }


            public override string ToString()
            {
                return " "
                    + $"Parcel id: {Id}\n"
                    + $"Parcel Sender:{SenderId}\n"
                    + $"Parcel Receiver:{ReceiverId}\n"
                    + $"Parcel Weight:{Weight}\n "
                    + $"Parcel priority:{Priority}\n"
                    ;
            }
        }
    }

