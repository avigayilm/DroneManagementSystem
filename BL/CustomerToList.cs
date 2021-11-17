using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerToList
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public int NumPacksReceived { get; set; }
            public int NumPackSentDel { get; set; }     
            public int NumPackSentUnDel { get; set; }
            public int NumPackExp { get; set; }
            public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"Name is {Name}, \n";
                result += $"Telephone is {PhoneNumber.Substring(0, 3) + '-' + PhoneNumber.Substring(3)}, \n";
                result += $"number of Packages received{NumPacksReceived}, \n";
                result += $"number of Packages sent delivered{NumPackSentDel}, \n";
                result += $"number of packages sent undelivered{NumPackSentUnDel}, \n";
                result += $"number of Packages expected{NumPackExp}, \n";
                return result;
            }
        }
    }
}
