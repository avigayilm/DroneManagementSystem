using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class packageInTransfer
        {
            public int ID { get; set; }
            public Priorities Priority { get; set; }
            public CustomerInDelivery Sender { get; set; }
            public CustomerInDelivery Receiver { get; set; }
        }
    }
}
