using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace DAL
{

     public partial class DalObject :IDal
    {
        /// <summary>
        /// constructor to initialize the dalobject
        /// </summary>
        public DalObject() => DataSource.Initialize();

        public double[] DronePwrUsg()
        {
            double[] pwrUsg = { DataSource.Config.pwrUsgEmpty, DataSource.Config.pwrUsgLight,DataSource.Config.pwrUsgMedium, DataSource.Config.pwrUsgHeavy, DataSource.Config.chargePH };
            return pwrUsg;
        }
        public ( double, double, double, double, double) DronePwrUsg1()
        {
            return (DataSource.Config.pwrUsgEmpty, DataSource.Config.pwrUsgLight, DataSource.Config.pwrUsgMedium, DataSource.Config.pwrUsgHeavy, DataSource.Config.chargePH);
          
        }
    }


}






