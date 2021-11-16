using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;


namespace DAL
{
    public partial class DalObject
    {
        /// <summary>
        /// sending a drone to charge in a station, adding the drone to the dronechargelist
        /// </summary>
        /// <param name="DroneId"></param>
        /// <param name="StationId"></param>
        public void SendToCharge(int droneId, int stationId)
        {
            int droneIndex = DataSource.dronesList.FindIndex(d => d.Id == droneId);
            int stationIndex = DataSource.stationList.FindIndex(s => s.Id == stationId);
            if (stationIndex == -1)
                throw new MissingIdException(" No such station in list\n");
            if (droneIndex == -1)
                throw new MissingIdException("No  such drone in list \n");
            else
            {
                //// making a new Dronecharge
                DroneCharge DC = new DroneCharge();
                DC.DroneId = droneId;
                DC.StationId = stationId;
                DataSource.chargeList.Add(DC);
            }

        }

        /// <summary>
        /// Once the drone is charged release the drone from the station, update the chargeslots, and remove the drone from the dronechargelist.
        /// </summary>
        /// <param name="Buzzer"></param>
        public void BatteryCharged(int droneId, int stationId)
        {
            int droneIndex = DataSource.chargeList.FindIndex(d => d.DroneId == droneId);// find the index of the dronecharge according to teh droneIndex
            if (droneIndex == -1)
                throw new MissingIdException("No such drone \n");
            else
            {
                DataSource.chargeList.Remove(DataSource.chargeList[droneIndex]);// removing the drone from the chargelist
                var temp = DataSource.dronesList[droneIndex];
                ///temp.battery = 100.00;// battery is full
                DataSource.dronesList[droneIndex] = temp;
            }

        }

        /// <summary>
        /// returns a list of Dronecharge
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDroneChargeList()// Display all the parcels in the array
        {
            List<DroneCharge> list = new();
            DataSource.chargeList.ForEach(dc => list.Add(dc));
            return (IEnumerable<DroneCharge>)list;
        }
    }
}