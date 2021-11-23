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
                DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
                if(tempDron == default)
                    throw new RetrievalException("Couldn't get drone\n");
                if (tempDron.Status == DroneStatuses.Available)
                {
                    
                    IDAL.DO.Station tempStation = FindClosestPossibleStation(tempDron);// returns a station that the drone can fly to.
                    if (tempStation.Id != 0)//if it's available and there is enough battery
                    {
                        //update the drone
                        tempDron.Loc.Longitude = tempStation.Longitude;
                        tempDron.Loc.Latitude = tempStation.Latitude;
                        double distance = Bonus.Haversine(tempDron.Loc.Longitude, tempDron.Loc.Latitude, tempDron.Loc.Longitude, tempDron.Loc.Latitude);
                        double batteryusage = BatteryUsage(distance, 0);// when sending the drone to charge the drone is empty
                        tempDron.Battery += batteryusage;
                        tempDron.Status = DroneStatuses.Maintenance;
                        //update the station
                        idal1.ChangeChargeSlots(tempStation.Id, -1);
                        //update dronecharge
                        idal1.SendToCharge(droneId, tempStation.Id);

                    }
                }
                else
                    throw new DroneChargeException("Can't send the drone to charge\n");// throw approptate acception
            }
            //catch (IDAL.DO.MissingIdException ex)
            //{
            //    throw new RetrievalException("Couldn't get drone\n", ex);
            //}
            catch(BatteryIssueException)
            {
                throw new DroneChargeException("Can't send the drone to charge");
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
                DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
                if (tempDron.Status == DroneStatuses.Maintenance)
                {
                    List<IDAL.DO.DroneCharge> tempDroneChargeList = (List<IDAL.DO.DroneCharge>)idal1.GetDroneChargeList();
                    int droneChargeIndex = tempDroneChargeList.FindIndex(dc => dc.DroneId == droneId);// finding the index of drone to get the station id
                    int stationId = tempDroneChargeList[droneChargeIndex].StationId;
                    // update drone
                    double batteryFilled = (chargingTime/60) * idal1.DronePwrUsg()[4];
                    tempDron.Battery += batteryFilled;
                    tempDron.Status = DroneStatuses.Available;
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
