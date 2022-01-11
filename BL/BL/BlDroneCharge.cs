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
                        //idal1.ChangeChargeSlots(tempStation.Id, -1);
                        //update dronecharge
                        idal1.SendToCharge(droneId, tempStation.Id);
                        if (chargeSlotsToAdd.ContainsKey(tempStation.Id))
                            chargeSlotsToAdd[tempStation.Id]++;
                        else
                            chargeSlotsToAdd.Add(tempStation.Id, 1);
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
                    List<DO.DroneCharge> tempDroneChargeList = (List<DO.DroneCharge>)idal1.GetDroneChargeList();
                    int droneChargeIndex = tempDroneChargeList.FindIndex(dc => dc.DroneId == droneId);// finding the index of drone to get the station id
                    int stationId = tempDroneChargeList[droneChargeIndex].StationId;
                    tempDron.Status = DroneStatuses.Available;
                    // update drone
                    lock (idal1)
                    {
                        TimeSpan timeInCharging = DateTime.Now - idal1.GetDroneChargeList(x => x.DroneId == tempDron.Id).First().ChargingTime;
                        //double batteryFilled = (chargingTime / 60) * idal1.DronePwrUsg()[4];
                        int batteryCharge = (int)(timeInCharging.TotalHours * idal1.DronePwrUsg()[4]);
                        tempDron.Battery += batteryCharge;
                        if (tempDron.Battery > 100)
                            tempDron.Battery = 100;
                        //update station
                        idal1.ChangeChargeSlots(stationId, 1);
                        //update dronecharge
                        idal1.BatteryCharged(droneId, stationId);
                        chargeSlotsToAdd.Add(stationId, 1);
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
                //var chargingListIdal = idal1.DronesChargingAtStation(stationId);
                //foreach (DO.Drone d in chargingListIdal)
                //{
                //    listDronecharge.Add(getDroneInCharge(d.Id));
                //}
                return (listDronecharge, listDronecharge.Count());
            }
           
        }
    }
}
