using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DeliveryAtCustomer
        {
            public string Id { get; set; }
            public Priorities Prioirty { get; set; }
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Receiver { get; set; }
            public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"Priority is {Prioirty}, \n";
                result += $"Customer in Delivery {Sender.ToString()}, \n";
                result += $"Customer in Delivery {Receiver.ToString()}, \n";
                return result;
            }

        }
    }
}
