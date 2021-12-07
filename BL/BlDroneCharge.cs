using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DAL;
using IDAL;

namespace BL
{
    public partial class BL
    {
        public void SendingDroneToCharge(int droneId)
        {
            DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
            if (tempDron.Status == DroneStatuses.Available)
            {

                IDAL.DO.Station tempStation = FindClosestStation(tempDron);// returns a station that the drone can fly to.
                bool hasEnoughBattery = CanReachstation(tempDron, tempStation);
                if (hasEnoughBattery)//if it's available and there is enough battery
                {
                    //update the drone
                    double distance = Bonus.Haversine(tempDron.Loc.Longitude, tempDron.Loc.Latitude, tempStation.Longitude, tempStation.Latitude);
                    tempDron.Loc.Longitude = tempStation.Longitude;
                    tempDron.Loc.Latitude = tempStation.Latitude;
                    int batteryusage = BatteryUsage(distance, 0);// when sending the drone to charge the drone is empty
                    tempDron.Battery -= batteryusage;
                    if (tempDron.Battery < 0)
                        tempDron.Battery = 0;
                    tempDron.Status = DroneStatuses.Maintenance;
                    //update the station
                    idal1.ChangeChargeSlots(tempStation.Id, -1);
                    //update dronecharge
                    idal1.SendToCharge(droneId, tempStation.Id);

                }
            }
            else
                throw new DroneChargeException("Can't send the drone to charge");// throw approptate acception

        }


        /// <summary>
        /// release teh drone from charge, updates the battery according to the charging time
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="chargingTime"></param>
        public void ReleasingDroneFromCharge(int droneId)
        {
            try
            {
                DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
                if (tempDron.Status == DroneStatuses.Maintenance)
                {
                    List<IDAL.DO.DroneCharge> tempDroneChargeList = (List<IDAL.DO.DroneCharge>)idal1.GetDroneChargeList();
                    int droneChargeIndex = tempDroneChargeList.FindIndex(dc => dc.DroneId == droneId);// finding the index of drone to get the station id
                    int stationId = tempDroneChargeList[droneChargeIndex].StationId;
                    // update drone
                    TimeSpan timeInCharging = DateTime.Now - idal1.GetDroneChargeList().First(x => x.DroneId == tempDron.Id).ChargingTime;
                    //double batteryFilled = (chargingTime / 60) * idal1.DronePwrUsg()[4];
                    int batteryCharge =(int) (timeInCharging.TotalHours * idal1.DronePwrUsg()[4]);
                    tempDron.Battery += batteryCharge;
                    if (tempDron.Battery > 100)
                        tempDron.Battery = 100;
                    tempDron.Status = DroneStatuses.Available;
                    //update station
                    idal1.ChangeChargeSlots(stationId, 1);
                    //update dronecharge
                    idal1.BatteryCharged(droneId, stationId);

                }
                else
                    throw new DroneChargeException("Couldn't release the drone from charge");// throw approptate acception
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Drone.", ex);
            }
        }

        internal DroneInCharge getDroneInCharge(int droneId)
        {
            DroneInCharge droneInCharge = new();
            droneInCharge.Battery = GetDrone(droneId).Battery;
            droneInCharge.Id = droneId;
            return droneInCharge;
        }
        public (IEnumerable<DroneInCharge>,int) getAllDroneInCharge(int stationId)
        {
            List<DroneInCharge> listDronecharge = new();
            var chargingListIdal = idal1.DronesChargingAtStation(stationId);
            foreach (IDAL.DO.Drone d in chargingListIdal)
            {
                listDronecharge.Add(getDroneInCharge(d.Id));
            }
            return (listDronecharge, listDronecharge.Count);
        }
    }
}
