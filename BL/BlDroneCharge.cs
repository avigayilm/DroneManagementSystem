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
            try
            {
                int index = idal1.CheckExistingDrone(droneId);
                //List<IDAL.DO.Station> tempStWithCharging = (List<IDAL.DO.Station>)idal1.GetStationWithCharging();
                IDAL.DO.Station tempStation = FindClosestPossibleStation(droneBL[index]);// returns a station that the drone can fly to. if id=-1 there is no such station
                DroneToList tempDrone = droneBL[index];
                if (droneBL[index].Status == DroneStatuses.Available && tempStation.Id != 0)//if it's available and there is enough battery
                {
                    //update the drone
                    tempDrone.Loc.Longitude = tempStation.Longitude;
                    tempDrone.Loc.Latitude = tempStation.Latitude;
                    UpdateBattery();
                    tempDrone.Status = DroneStatuses.Maintenance;
                    //update the station
                    idal1.ChangeChargeSlots(tempStation.Id, -1);
                    //update dronecharge
                    idal1.SendToCharge(droneId, tempStation.Id);

                }
                else
                    throw new DroneChargeException("Can't send the drone to charge\n");// throw approptate acception
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get drone\n", ex);
            }



        }

        /// <summary>
        /// release teh drone from charge, updates the battery according to the charging time
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="chargingTime"></param>
        public void ReleasingDroneFromCharge(int droneId, double chargingTime)
        {
            try
            {
                int index = GetDroneIndex(droneId);
                if (droneBL[index].Status == DroneStatuses.Maintenance)
                {
                    List<IDAL.DO.DroneCharge> tempStWithCharging = (List<IDAL.DO.DroneCharge>)idal1.GetDroneChargeList();
                    int droneChargeIndex = droneBL.FindIndex(d => d.Id == droneId);// finding the index of drone to get the station id
                    int stationId = tempStWithCharging[droneChargeIndex].StationId;
                    DroneToList tempDrone = droneBL[index];
                    // update drone
                    UpdateBattery();
                    tempDrone.Status = DroneStatuses.Available;
                    //update station
                    idal1.ChangeChargeSlots(stationId, 1);
                    //update dronecharge
                    idal1.BatteryCharged(droneId, stationId);
                }
                else
                    throw new DroneChargeException("Couldn't release the drone from charge\n");// throw approptate acception
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Drone.\n,", ex);
            }

        }
    }
}
