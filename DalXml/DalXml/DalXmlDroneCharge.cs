using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using System.Xml.Linq;
using DO;

namespace Dal
{
    internal sealed partial class DalXml
    {

        //public void SendToCharge(int droneId, int stationId)
        //{
        //    throw new NotImplementedException();
        //}

        //public void BatteryCharged(int droneId, int stationId)
        //{
        //    throw new NotImplementedException();
        //}


        //public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate = null)
        //{
        //    throw new NotImplementedException();
        //}
        //public IEnumerable<Drone> DronesChargingAtStation(int stationId)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// sending a drone to charge in a station, adding the drone to the dronechargelist
        /// </summary>
        /// <param name="DroneId"></param>
        /// <param name="StationId"></param>
        public void SendToCharge(int droneId, int stationId)
        {
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);

            int droneIndex = CheckExistingDrone(droneId);
               // DataSource.dronesList.FindIndex(d => d.Id == droneId);
            int stationIndex = CheckExistingStation(stationId);
            //    //DataSource.stationList.FindIndex(s => s.Id == stationId);
            //if (stationIndex == -1)
            //    throw new MissingIdException(" No such station in list\n");
            //if (droneIndex == -1)
            //    throw new MissingIdException("No  such drone in list \n");
            //// making a new Dronecharge
            ChangeChargeSlots(stationId, -1);
            DroneCharge DC = new DroneCharge();
            DC.DroneId = droneId;
            DC.StationId = stationId;
            DC.ChargingTime = DateTime.Now;
            droneCharges.Append(DC);
            droneChargeRoot.Add(DC);
            XMLTools.SaveListToXMLElement(droneChargeRoot, DroneChargeXml);
        }

        /// <summary>
        /// Once the drone is charged release the drone from the station, update the chargeslots, and remove the drone from the dronechargelist.
        /// </summary>
        /// <param name="Buzzer"></param>
        public void BatteryCharged(int droneId, int stationId)
        {
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
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
                XMLTools.SaveListToXMLElement(droneChargeRoot, DroneChargeXml);
            }
        }

        


        public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate = null)// Display all the parcels in the array
        {
            //return DataSource.chargeList.FindAll(c => predicate == null ? true : predicate(c));
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
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
            return droneCharges.Where(c => predicate == null ? true : predicate(c));
        }
    }
}
