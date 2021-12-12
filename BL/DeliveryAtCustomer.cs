using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return " "
                + $"ID is {Id}\n"
                + $"Priority is {Prioirty}\n"
                + $"Customer in Delivery {Sender}\n"
                + $"Customer in Delivery {Receiver}\n";
            }

        }
    }
