﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IBL
{
    namespace BO
    {
        public class package
        {
            public int id;
            public Customer sender;
            public Customer receiver;
            public WeightCategories weight;
            public Priorities priority;
            public Drone dr;
            public DateTime creation;
            public DateTime assigning;
            public DateTime collecting;
            public DateTime delivering;

        }
    }

}
