using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
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
        private List<DroneCharge> droneCharges;
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
        static DalXml() {
           // DataSource.Initialize();
            //XMLTools.SaveListToXMLElement(new XElement("DroneChargeXml.xml"), DroneChargeXml);
            //XElement rootElem = new XElement("configLists",new XElement("runNum", 1020), 
            //    new XElement("pwrUsgEmpty", pwrUsgEmpty),
            //    new XElement("pwrUsgLight", pwrUsgLight),
            //    new XElement("pwrUsgMedium", pwrUsgMedium),
            //    new XElement("pwrUsgHeavy", pwrUsgHeavy),
            //    new XElement("chargePH", chargePH));

            ////rootElem.Add(new int[] { , pwrUsgLight, pwrUsgMedium, pwrUsgHeavy, chargePH });
            //XMLTools.SaveListToXMLElement(rootElem, "config.xml");
        }
        private DalXml()
        {
           
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int[] DronePwrUsg()
        {
            XElement tempPwr = XMLTools.LoadListFromXMLElement(@"config.xml");
            int[] pwrUsg = { int.Parse(tempPwr.Element("pwrUsgEmpty").Value), int.Parse(tempPwr.Element("pwrUsgLight").Value), int.Parse(tempPwr.Element("pwrUsgMedium").Value), int.Parse(tempPwr.Element("pwrUsgHeavy").Value), int.Parse(tempPwr.Element("chargePH").Value) };
            return pwrUsg;
        }

       
    }
}
