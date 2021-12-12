using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;

namespace Dal
{

     internal sealed partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// constructor to initialize the dalobject
        /// </summary>

        private static readonly Lazy<DalObject> instance = new Lazy<DalObject>(()=>new DalObject());
        
        private DalObject() { DataSource.Initialize(); }
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






