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
            public CustomerInDelivery Sender { get; set; }
            public CustomerInDelivery Reiceiver { get; set; }


        }
    }
}
