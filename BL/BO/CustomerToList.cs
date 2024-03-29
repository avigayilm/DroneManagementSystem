﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return " "
                + $"ID is {Id}\n"
                + $"Name is {Name}\n"
                + $"Telephone is {PhoneNumber.Substring(0, 3) + '-' + PhoneNumber.Substring(3)}\n"
                + $"number of Packages received {NumPacksReceived}\n"
                + $"number of Packages sent delivered {NumPackSentDel}\n"
                + $"number of packages sent undelivered {NumPackSentUnDel}\n"
                + $"number of Packages expected {NumPackExp}\n";
            }
        }
    }

