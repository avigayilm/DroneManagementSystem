using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerInDelivery
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"Name is {Name}, \n";
                return result;
            }
        }
    }
}
