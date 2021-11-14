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
            public string NameSender { get; set; }
            public string NameReceiver { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            // package status

        }
    }

}
