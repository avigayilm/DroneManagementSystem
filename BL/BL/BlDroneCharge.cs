using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;
using DO;
using DalApi;

namespace BL
{
    public partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int SendingDroneToCharge(int droneId)
        {
            DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
            int stationId = 0;
            if (tempDron.Status == DroneStatuses.Available)
            {
                DO.Station tempStation = FindClosestStation(tempDron);// returns a station that the drone can fly to.
                bool hasEnoughBattery = CanReachstation(tempDron, tempStation);
                if (hasEnoughBattery)//if it's available and there is enough battery
                {
                    //update the drone
                    stationId = tempStation.Id;
                    double distance = Bonus.Haversine(tempDron.Loc.Longitude, tempDron.Loc.Latitude, tempStation.Longitude, tempStation.Latitude);
                    tempDron.Loc.Longitude = tempStation.Longitude;
                    tempDron.Loc.Latitude = tempStation.Latitude;
                    int batteryusage = BatteryUsage(distance, 0);// when sending the drone to charge the drone is empty
                    tempDron.Battery -= batteryusage;
                    if (tempDron.Battery < 0)
                        tempDron.Battery = 0;
                    tempDron.Status = DroneStatuses.Maintenance;
                    lock (idal1)
                    {
                        //update the station
                        idal1.SendToCharge(droneId, tempStation.Id);
                    }

                }
            }
            else
                throw new DroneChargeException("Can't send the drone to charge");// throw approptate acception
            return stationId;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ReleasingDroneFromCharge(int droneId)
        {
            try
            {
                DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
                if (tempDron.Status == DroneStatuses.Maintenance)
                {
                    IEnumerable<DO.DroneCharge> tempDroneChargeList = idal1.GetDroneChargeList();
                    DO.DroneCharge droneCharge = tempDroneChargeList.First(dc => dc.DroneId == droneId);
                    int stationId = droneCharge.StationId;
                    tempDron.Status = DroneStatuses.Available;
                    // update drone
                    lock (idal1)
                    {
                        TimeSpan timeInCharging = DateTime.Now - droneCharge.ChargingTime;
                        int batteryCharge = (int)(timeInCharging.TotalHours * idal1.DronePwrUsg()[4]);
                        tempDron.Battery += batteryCharge;
                        if (tempDron.Battery > 100)
                            tempDron.Battery = 100;
                        //update station
                        idal1.ChangeChargeSlots(stationId, 1);
                        //update dronecharge
                        idal1.BatteryCharged(droneId, stationId);
                    }
                    return stationId;
                }
                else
                    throw new DroneChargeException("Couldn't release the drone from charge");// throw approptate acception
                return 0;
            }
            catch (DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Drone.", ex);
            }
        }

        internal DroneInCharge getDroneInCharge(int droneId)
        {
            DroneInCharge droneInCharge = new DroneInCharge();
            droneInCharge.Battery = GetDrone(droneId).Battery;
            droneInCharge.Id = droneId;
            return droneInCharge;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public (IEnumerable<DroneInCharge>,int) getAllDroneInCharge(int stationId)
        {
            IEnumerable<DroneInCharge> listDronecharge = new List<DroneInCharge>();
            lock (idal1)
            {
                listDronecharge = from droneC in idal1.DronesChargingAtStation(stationId)
                                   select (getDroneInCharge(droneC.Id));
                return (listDronecharge, listDronecharge.Count());
            }
           
        }
    }
}
