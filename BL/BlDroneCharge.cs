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
                if (droneBL[index].Status == DroneStatuses.Available)
                {
                    IDAL.DO.Station tempStation = FindClosestPossibleStation(droneBL[index]);// returns a station that the drone can fly to.
                    DroneToList tempDrone = droneBL[index];
                    if (tempStation.Id != 0)//if it's available and there is enough battery
                    {
                        //update the drone
                        tempDrone.Loc.Longitude = tempStation.Longitude;
                        tempDrone.Loc.Latitude = tempStation.Latitude;
                        double distance = Bonus.Haversine(tempDrone.Loc.Longitude, tempDrone.Loc.Latitude, tempDrone.Loc.Longitude, tempDrone.Loc.Latitude);
                        double batteryusage = BatteryUsage(distance, 0);// when sending the drone to charge the drone is empty
                        tempDrone.Battery += batteryusage;
                        tempDrone.Status = DroneStatuses.Maintenance;
                        //update the station
                        idal1.ChangeChargeSlots(tempStation.Id, -1);
                        //update dronecharge
                        idal1.SendToCharge(droneId, tempStation.Id);

                    }
                }
                else
                    throw new DroneChargeException("Can't send the drone to charge\n");// throw approptate acception
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get drone\n", ex);
            }
            catch(BatteryIssueException)
            {
                throw new DroneChargeException("Can't send the drone to charge")
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
                int index = idal1.CheckExistingDrone(droneId);
                if (droneBL[index].Status == DroneStatuses.Maintenance)
                {
                    List<IDAL.DO.DroneCharge> tempDroneChargeList = (List<IDAL.DO.DroneCharge>)idal1.GetDroneChargeList();
                    int droneChargeIndex = droneBL.FindIndex(d => d.Id == droneId);// finding the index of drone to get the station id
                    int stationId = tempDroneChargeList[droneChargeIndex].StationId;
                    DroneToList tempDrone = droneBL[index];
                    // update drone
                    double batteryFilled = (chargingTime/60) * idal1.DronePwrUsg()[4];
                    tempDrone.Battery += batteryFilled;
                    tempDrone.Status = DroneStatuses.Available;
                    //update station
                    idal1.ChangeChargeSlots(stationId, 1);
                    //update dronecharge
                    idal1.BatteryCharged(droneId, stationId);
                    droneBL[index] = tempDrone;
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
