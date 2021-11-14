using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class CustomerToList
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public int NumPacksReceived { get; set; }
            public int NumPackSentDel { get; set; }
            public int NumPackSentUnDel { get; set; }
            public int NumPackExp { get; set; }
        }
    }
}
