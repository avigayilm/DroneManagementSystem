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
            public int ID;
            public Priorities priority;
            public CustomerInDelivery sender;
            public CustomerInDelivery receiver;
        }
    }
}
