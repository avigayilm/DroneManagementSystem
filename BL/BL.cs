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
            try
            {
                //DAL.DalObject dal2 = new();

                double[] tempArray = idal1.DronePwrUsg();
                double pwrUsgEmpty = tempArray[0];
                double pwrUsgLight = tempArray[1];
                double pwrUsgMedium = tempArray[2];
                double pwrUsgHeavy = tempArray[3];
                double chargePH = tempArray[4];

                //List<IDAL.DO.Drone> tempDroneList = (List<IDAL.DO.Drone>)idal1.GetAllDrones();
                (idal1.GetAllDrones().ToList()).CopyPropertyListtoIBLList(droneBL);// converts the dronelist to IBL
                List<IDAL.DO.Parcel> undeliveredParcel = idal1.GetAllParcels(p => p.Delivered == null).ToList();

                foreach (IDAL.DO.Parcel p in undeliveredParcel)
                {
                    DroneToList tempDro = droneBL.FirstOrDefault(d => d.Id == p.DroneId);
                    if (tempDro != default)// if there is a drone assigned to the parcel
                    {
                        tempDro.Status = DroneStatuses.Delivery;
                        tempDro.Battery = rand.Next(40, 100);// random battery level so that the drone can still fly
                        tempDro.Loc = DroneLocation(p, tempDro);//location of drone
                    }
                }
                foreach (DroneToList dr in droneBL)
                {
                    if (dr.Status != DroneStatuses.Delivery)
                    {
                        dr.Status = (DroneStatuses)rand.Next(1);
                        if (dr.Status == DroneStatuses.Available)
                        {
                            //List<IDAL.DO.Customer> cus = idal1.CustomersDeliverdTo();
                            //List<IDAL.DO.Parcel> tempList = idal1.GetAllParcels(p => p.Delivered != null).ToList();
                            List<IDAL.DO.Customer> cusDeliveredTo = (idal1.GetAllCustomers(c => idal1.GetAllParcels(p => p.Delivered != null).ToList().Any(p => c.Id == p.ReceiverId))).ToList();//returns a  list of all the customers that have received a parcel
                            IDAL.DO.Customer tempCus = cusDeliveredTo[rand.Next(cusDeliveredTo.Count())];
                            dr.Loc = new() { Longitude = tempCus.Longitude, Latitude = tempCus.Latitude };
                            // int minBat = BatteryUsage(FindClosestPossibleStation1(dr).Item2, 0
                            int minBat = BatteryUsage(DroneDistanceFromStation(dr, FindClosestStation(dr)), 0);//calculates battery usage of flying to closest station to drone
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
            catch(BatteryIssueException ex)
            {
                throw new ConstructorIssueException("Problem in the constructor\n", ex);
            }

        }
    }
}



