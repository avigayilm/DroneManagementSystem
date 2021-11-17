using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class PackageToList
        {
            public int Id { get; set; }
            public string Sender { get; set; }
            public string Target { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            // package status

        }
    }

}
