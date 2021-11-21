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
    public partial class BL : IBL.Ibl
    {
        internal static Random rand = new Random();
        internal List<IBL.BO.DroneToList> droneBL = new();
        IDal idal1 = new DAL.DalObject();
        public BL()
        {
            //DAL.DalObject dal2 = new();

            double[] tempArray = idal1.DronePwrUsg();
            double pwrUsgEmpty = tempArray[0];
            double pwrUsgLight = tempArray[1];
            double pwrUsgMedium = tempArray[2];
            double pwrUsgHeavy = tempArray[3];
            double chargePH = tempArray[4];

            //List<IDAL.DO.Drone> tempDroneList = (List<IDAL.DO.Drone>)idal1.GetAllDrones();
            ((List<IDAL.DO.Drone>)idal1.GetAllDrones()).CopyPropertyListtoIBLList(droneBL);// converts the dronelist to IBL
            List<IDAL.DO.Parcel> undeliveredParcel = (List<IDAL.DO.Parcel>)idal1.UndeliveredParcels();

            foreach (IDAL.DO.Parcel p in undeliveredParcel)
            {
                int DroneId = p.DroneId;
                int droneIndex = droneBL.FindIndex(d => d.Id == DroneId);
                if (droneIndex > 0)// if there is a drone assigned to the parcel
                {
                    var tempBl = droneBL[droneIndex];
                    tempBl.Status = DroneStatuses.Delivery;
                    tempBl.Battery = rand.Next(40, 100);// random battery level so that the drone can still fly
                    tempBl.Loc = DroneLocation(p, tempBl);//location of drone
                }
            }
            foreach (DroneToList dr in droneBL)
            {
                if (dr.Status != DroneStatuses.Delivery)
                {
                    dr.Status = (DroneStatuses)rand.Next(1);
                    if (dr.Status == DroneStatuses.Available)
                    {
                        List<IDAL.DO.Customer> cus = idal1.CustomersDeliverdTo();
                       IDAL.DO.Customer tempCus = cus[rand.Next(cus.Count())];
                        dr.Loc.Longitude = tempCus.Longitude;
                        dr.Loc.Longitude = tempCus.Latitude;
                        int minBat = BatteryUsage(DroneDistanceFromStation(dr, FindClosestPossibleStation(dr)), 0);//calculates battery usage of flying to closest station to drone
                        dr.Battery = rand.Next(minBat, 100);
                    }
                    else//it is in maintenance
                    {
                        dr.Battery = rand.Next(20);// random battery level so that the drone can still fly
                        List<IDAL.DO.Station> tempList = (List<IDAL.DO.Station>)idal1.GetAllStations();
                        IDAL.DO.Station tempSt = tempList[rand.Next(tempList.Count())];
                        dr.Loc.Latitude = tempSt.Latitude;
                        dr.Loc.Longitude = tempSt.Longitude;
                    }
                }
            }

        }
        //public double DroneDistanceFromParcel(IBL.BO.DroneToList dr, IDAL.DO.Parcel par)
        //{
        //    double distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, idal1.GetCustomer(par.Sender).Longitude, idal1.GetCustomer(par.Sender).Latitude);
        //    return distance;
        //}
        ///// <summary>
        ///// measures distance between a customer and a sttion
        ///// </summary>
        ///// <param name="cus"></param>
        ///// <param name="st"></param>
        ///// <returns>double</returns>
        //public double StationDistanceFromCustomer(IDAL.DO.Customer cus, IDAL.DO.Station st)
        //{
        //    return Bonus.Haversine(cus.Longitude, cus.Latitude, st.Longitude, st.Latitude);
        //}
        //public double DroneDistanceFromStation(DroneToList dr, IDAL.DO.Station st)
        //{
        //    return Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, st.Longitude, st.Latitude);
        //}
    }
}



