using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace BO
    {
        public class CustomerInParcel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return " "
                + $"ID is: {Id}\n"
                + $"Name is: {Name}\n";

            }
        }
    }

