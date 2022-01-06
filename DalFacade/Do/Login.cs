using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct Login
    {
        public string UserName { get; set; }
        public string Password  { get; set; }
        public bool StaffOrUser  { get; set; }
        public string profileSource { get; set; }
        public string emailAddress { get; set; }
    }
}
