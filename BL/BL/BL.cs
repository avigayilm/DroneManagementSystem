using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using BO;
using DO;
using DalApi;
using System.Threading;

namespace BL
{
    public sealed partial class BL : BlApi.Ibl
    {
        internal static Random rand = new Random();
        internal List<BO.DroneToList> droneBL = new List<DroneToList>();
        internal DalApi.Idal idal1 = DalFactory.GetDal();
        private static readonly Lazy<BL> instance = new Lazy<BL>(() => new BL());
        public static BL Instance
        {
            get
            {
                return instance.Value;
            }
        }
        private BL()
        {

            //DAL.DalObject dal2 = new();
          
            int[] tempArray = idal1.DronePwrUsg();
            double pwrUsgEmpty = tempArray[0];
            double pwrUsgLight = tempArray[1];
            double pwrUsgMedium = tempArray[2];
            double pwrUsgHeavy = tempArray[3];
            double chargePH = tempArray[4];
            int count = 0;
            //List<DO.Drone> tempDroneList = (List<DO.Drone>)idal1.GetAllDrones();
            idal1.GetAllDrones().ToList().CopyPropertyListtoIBLList(droneBL);// converts the dronelist to IBL
            List<DO.Parcel> undeliveredParcel = idal1.GetAllParcels(p => p.Delivered == null).ToList();

            foreach (DO.Parcel p in undeliveredParcel)
            {
                DroneToList tempDro = droneBL.FirstOrDefault(d => d.Id == p.DroneId);
                if (tempDro != default)// if there is a drone assigned to the parcel
                {
                    tempDro.Status = DroneStatuses.Delivery;// the drone is in delivery
                    tempDro.Battery = rand.Next(40, 100);// random battery level so that the drone can still fly
                    tempDro.Loc = DroneLocation(p, tempDro);//location of drone
                    tempDro.ParcelId = p.Id;
                }
            }
            foreach (DroneToList dr in droneBL)
            {
                //Thread.Sleep(19999);
                if (dr.Status != DroneStatuses.Delivery)
                {
                    //dr.Status = (DroneStatuses)rand.Next(2);
                   if(idal1.GetDroneChargeList(d => d.DroneId == dr.Id).Any()) //it is in maintenance
                    {
                        dr.Status = DroneStatuses.Maintenance;
                        dr.Battery = rand.Next(20, 50);// random battery level so that the drone can still fly
                        List<DO.Station> tempList = (List<DO.Station>)idal1.GetAllStations();
                        DO.Station tempSt = tempList[rand.Next(tempList.Count())];
                        dr.Loc = new Location() { Latitude = tempSt.Latitude, Longitude = tempSt.Longitude };
                        idal1.SendToCharge(dr.Id, tempSt.Id);
                        count++;
                    }
                    else //the drone is available
                    {
                        dr.Status = DroneStatuses.Available;
                        List<DO.Customer> cusDeliveredTo = (idal1.GetAllCustomers(c => idal1.GetAllParcels(p => p.Delivered != null).ToList().Any(p => c.Id == p.ReceiverId))).ToList();//returns a  list of all the customers that have received a parcel
                        DO.Customer tempCus = cusDeliveredTo[rand.Next(cusDeliveredTo.Count())];
                        dr.Loc = new Location() { Longitude = tempCus.Longitude, Latitude = tempCus.Latitude };
                        // calculates battery usage of flying to closest station to drone
                        int minBat = BatteryUsage(DroneDistanceFromStation(dr, FindClosestStation(dr)), 0);
                        //dr.Battery = rand.Next(minBat, 100);
                        dr.Battery = rand.Next(40, 100);
                    }

                }
            }
        }

        public void simulation(int droneId, Func<bool> func, Action reportProgress)
        {
            new Simulation(this, droneId, func, reportProgress);
        }
    }
}



