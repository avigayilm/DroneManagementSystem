using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        public double DistanceBetweenCustomers(IDAL.DO.Customer cus1, IDAL.DO.Customer cus2)
        {
            return Bonus.Haversine(cus1.Longitude, cus1.Latitude, cus2.Longitude, cus2.Latitude);
        }
        
        /// <summary>
        /// returns the distance between a drone and a parcel
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public double DroneDistanceFromParcel(IBL.BO.DroneToList dr, IDAL.DO.Parcel par)
        {
            double distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, idal1.GetCustomer(par.Sender).Longitude, idal1.GetCustomer(par.Sender).Latitude);
            return distance;
        }
        /// <summary>
        /// measures distance between a customer and a station
        /// </summary>
        /// <param name="cus"></param>
        /// <param name="st"></param>
        /// <returns>double</returns>
        public double StationDistanceFromCustomer(IDAL.DO.Customer cus, IDAL.DO.Station st)
        {
            return Bonus.Haversine(cus.Longitude, cus.Latitude, st.Longitude, st.Latitude);
        }
        /// <summary>
        /// returns the distance between a station and a customer
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public double DroneDistanceFromStation(IBL.BO.DroneToList dr, IDAL.DO.Station st)
        {
            return Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, st.Longitude, st.Latitude);
        }
    }
}
