using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace DAL
{
    /// <summary>
    /// 
    /// </summary>
     public partial class DalObject:IDal
    {
        /// <summary>
        /// 
        /// </summary>
        public DalObject() => DataSource.Initialize();

        public double[] DronePwrUsg()
        {
            double[] pwrUsg = { DataSource.Config.pwrUsgEmpty, DataSource.Config.pwrUsgLight, DataSource.Config.pwrUsgHeavy, DataSource.Config.chargePH };
            return pwrUsg;
        }
    }


}






