using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;


namespace BL
{
    public partial class BL
    {
        /// <summary>
        /// returns the distance between 2 customers
        /// </summary>
        /// <param name="cus1"></param>
        /// <param name="cus2"></param>
        /// <returns></returns>
        internal double DistanceBetweenCustomers(DO.Customer cus1, DO.Customer cus2)
        {
            return Bonus.Haversine(cus1.Longitude, cus1.Latitude, cus2.Longitude, cus2.Latitude);
        }

       /// <summary>
       /// computes distance of drone from a cusotmer
       /// </summary>
       /// <param name="drone"></param>
       /// <param name="cus"></param>
       /// <returns></returns>
        internal double DronedistanceFromCustomer(BO.DroneToList drone, BO.Customer cus)
        {
            return Bonus.Haversine(cus.Loc.Longitude, cus.Loc.Latitude, drone.Loc.Longitude, drone.Loc.Latitude);
        }

        /// <summary>
        /// returns the distance between a drone and a parcel
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        internal double DroneDistanceFromParcel(BO.DroneToList dr, DO.Parcel par)
        {
            double distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, idal1.GetCustomer(par.SenderId).Longitude, idal1.GetCustomer(par.SenderId).Latitude);
            return distance;
        }
        /// <summary>
        /// measures distance between a customer and a station
        /// </summary>
        /// <param name="cus"></param>
        /// <param name="st"></param>
        /// <returns>double</returns>
        internal double StationDistanceFromCustomer(DO.Customer cus, DO.Station st)
        {
            return Bonus.Haversine(cus.Longitude, cus.Latitude, st.Longitude, st.Latitude);
        }

        /// <summary>
        /// returns the distance between a station and a customer
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        internal double DroneDistanceFromStation(BO.DroneToList dr, DO.Station? st1 = null, BO.Station st2 = null )
        {
            return Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, st1 != null ? ((DO.Station)st1).Longitude : ((BO.Station)st2).Loc.Longitude, st1 != null ? ((DO.Station)st1).Latitude : st2.Loc.Latitude);
        }
    }
}
