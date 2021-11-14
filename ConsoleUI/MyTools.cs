using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;
using DAL;

namespace BL
{
    static class MyTools
    {
        /// <summary>
        /// function converts a drone object from idal to a Bl drone
        /// </summary>
        /// <param name="dro"></param>
        /// <returns></returns>


        public static Drone ToBLDrone(this IDAL.DO.Drone dro )
        {
            Drone BLDrone = new()
            {
                Id = dro.id,
                Model = dro.model,
                Weight = (WeightCategories)dro.maxWeight,
            };
            return BLDrone;
        }
        /// <summary>
        /// function converts IDAL droneList to BL droneList
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static IEnumerable<Drone> ToBLDroneList(this List<IDAL.DO.Drone> dr  )
        {
            List<Drone> BLdrones = new();
            foreach(IDAL.DO.Drone dro in dr )
            {
                BLdrones.Add(dro.ToBLDrone());//adds to the list all drones from idal list converted
            }
            return BLdrones;
        }
    }
}
