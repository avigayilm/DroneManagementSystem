using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal
{

    internal sealed partial class DalXml:Idal
    {
        private static string DroneXml = @"DroneXml.xml";
        private static string ParcelXml = @"ParcelXml.xml";
        private static string StationXml = @"StationXml.xml";
        private static string DroneChargeXml = @"DroneChargeXml.xml";
        private static string CustomerXml = @"CustomerXml.xml";
        private static string LoginXml = @"LoginXml.xml";

        private List<Station> stations;
        private List<Parcel> parcels;
        private List<Drone> drones;
        List<Customer> customers;
        List<Login> logins;
        private void loadingToList<T>(ref List<T> tempList,string fileName)
        {
            if(tempList == null)
            {
                tempList = XMLTools.LoadListFromXMLSerializer<T>(fileName);
            }
        }

        /// <summary>
        /// constructor to initialize the dalobject
        /// </summary>
        internal static DalXml instance { get { return Instance.Value; } }
        private static Lazy<DalXml> Instance = new Lazy<DalXml>(() => new ());
        static DalXml() { XMLTools.SaveListToXMLSerializer(new List<int> { 0 }, "runNumXml.xml"); }
        private DalXml() { }
        public double[] DronePwrUsg()
        {
            double[] pwrUsg = { DataSource.Config.pwrUsgEmpty, DataSource.Config.pwrUsgLight, DataSource.Config.pwrUsgMedium, DataSource.Config.pwrUsgHeavy, DataSource.Config.chargePH };
            return pwrUsg;
        }
        public (double, double, double, double, double) DronePwrUsg1()
        {
            return (DataSource.Config.pwrUsgEmpty, DataSource.Config.pwrUsgLight, DataSource.Config.pwrUsgMedium, DataSource.Config.pwrUsgHeavy, DataSource.Config.chargePH);
        }
    }
}
