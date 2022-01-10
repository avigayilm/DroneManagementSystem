using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using DO;

namespace Dal
{
    internal sealed partial class DalXml
    {


        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SendToCharge(int droneId, int stationId)
        {
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            droneCharges = GetDroneChargeList().ToList();
            int droneIndex = CheckExistingDrone(droneId);
               //DataSource.dronesList.FindIndex(d => d.Id == droneId);
            int stationIndex = CheckExistingStation(stationId);
            ChangeChargeSlots(stationId, -1);
            DroneCharge DC = new DroneCharge() { StationId = stationId, DroneId = droneId, ChargingTime = DateTime.Now};
            // here we check to make sure the drone charge doesnt already exist
            XElement x = (from d in droneChargeRoot.Elements()
                          where d.Element("DroneId").Value == droneId.ToString() && d.Element("StationId").Value == stationId.ToString()
                          select d).FirstOrDefault();
            if(x== null) // is it does not already exist
            {
                droneCharges.Add(DC);
                droneChargeRoot.Add(new XElement("DroneCharge", new XElement("DroneId", droneId), new XElement("StationId", stationId), new XElement("ChargingTime", DateTime.Now)));
                XMLTools.SaveListToXMLElement(droneChargeRoot, DroneChargeXml);
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void BatteryCharged(int droneId, int stationId)
        {
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            droneCharges = GetDroneChargeList();
            int dronecIndex = droneCharges.FindIndex(d => d.DroneId == droneId);// find the index of the dronecharge according to teh droneIndex
            if (dronecIndex == -1)
                throw new MissingIdException("No such drone Charge \n");
            //int droneIndex = CheckExistingDrone(droneId);
            else
            {
                droneCharges.Remove(droneCharges[dronecIndex]);// removing the drone from the chargelist
                                                               //var temp = DataSource.dronesList[dronecIndex];
                                                               /////temp.battery = 100.00;// battery is full
                                                               //DataSource.dronesList[dronecIndex] = temp;
                (from d in droneChargeRoot.Elements()
                 where Int32.Parse(d.Element("DroneId").Value) == droneId
                 select d)
                    .FirstOrDefault().Remove();
                XMLTools.SaveListToXMLElement(droneChargeRoot, DroneChargeXml);
            }
        }



        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate = null)// Display all the parcels in the array
        {
            //return DataSource.chargeList.FindAll(c => predicate == null ? true : predicate(c));
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            if(droneCharges == null)
            {
                try
                {

                    droneCharges = (from dc in droneChargeRoot.Elements()
                                    select new DroneCharge()
                                    {
                                        DroneId = Convert.ToInt32(dc.Element("DroneId").Value),
                                        StationId = Convert.ToInt32(dc.Element("StationId").Value),
                                        ChargingTime = Convert.ToDateTime(dc.Element("ChargingTime").Value)
                                    }).ToList();
                }
                catch
                {
                    droneCharges = null;
                }
            }
           
            return droneCharges.Where(c => predicate == null ? true : predicate(c)).ToList();
        }
    }
}
