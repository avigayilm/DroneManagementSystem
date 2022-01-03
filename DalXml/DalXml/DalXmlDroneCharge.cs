using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal
{
    internal sealed partial class DalXml
    {

        public void SendToCharge(int droneId, int stationId)
        {
            throw new NotImplementedException();
        }

        public void BatteryCharged(int droneId, int stationId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate = null)
        {
            throw new NotImplementedException();
        }
        //public IEnumerable<Drone> DronesChargingAtStation(int stationId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
