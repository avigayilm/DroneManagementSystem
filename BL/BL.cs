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
    partial class BL : IBL.IBL
    {
        internal static Random rand = new Random();
        internal List<IBL.BO.Drone> droneBL = new();
        IDal idal1 = new DAL.DalObject();
        internal Location DroneLocation(IDAL.DO.Parcel p, Drone tempBl)
        {
            Location locTemp = new();
            if (p.requested != null && p.pickedUp == null)//if assigned but not yet collected
            {
                //location of deone will be in  station closed to the sender
                var stationTemp = idal1.smallestDistanceStation(p.senderid);
                locTemp.longitude = stationTemp.longitude;
                locTemp.latitude = stationTemp.latitude;
            }
            if (p.requested != null && p.pickedUp != null)// if pakage is assigned and picked up
            {
                //location wIll be at the sender
                List<IDAL.DO.Customer> tempCustomerList = (List<IDAL.DO.Customer>)idal1.GetAllCustomers();
                int customerIndex = tempCustomerList.FindIndex(c => c.id == p.senderid);
                if (customerIndex != -1)
                {
                    locTemp.longitude = tempCustomerList[customerIndex].longitude;
                    locTemp.latitude = tempCustomerList[customerIndex].latitude;
                }
                
            }
            return locTemp;
        }
        BL()
        {
            //DAL.DalObject dal2 = new();
            
            double[] tempArray = idal1.DronePwrUsg();
            double pwrUsgEmpty = tempArray[0];
            double pwrUsgLight = tempArray[1];
            double pwrUsgMedium = tempArray[2];
            double pwrUsgHeavy = tempArray[3];
            double chargePH = tempArray[4];

            List<IDAL.DO.Drone> tempDroneList = (List<IDAL.DO.Drone>)idal1.GetAllDrones();
            droneBL = (List<IBL.BO.Drone>)tempDroneList.ToBLDroneList();// converts the dronelist to IBL
            List<IDAL.DO.Parcel> undeliveredParcel = (List<IDAL.DO.Parcel>)idal1.UndeliveredParcels();

            foreach (IDAL.DO.Parcel p in undeliveredParcel)
            {
                int DroneId = p.droneId;
                int droneIndex = droneBL.FindIndex(d => d.id == DroneId);
                if (droneIndex > 0)// if there is a drone assigned to the parcel
                {
                    var tempBl = droneBL[droneIndex];
                    tempBl.status = DroneStatuses.Delivery;
                    tempBl.battery = rand.Next(40, 100);// random battery level so that the drone can still fly
                    tempBl.loc = DroneLocation(p, tempBl);//location of drone
                }
            }
                foreach(Drone dr in droneBL)
                {
                    if(dr.status != DroneStatuses.Delivery)
                    {
                    Drone tempDr = dr;
                        tempDr.status = (DroneStatuses)rand.Next(1);
                        if(dr.status==DroneStatuses.Available)
                        {
                            
                        }
                        else
                        {
                            tempBl.battery = rand.Next(20);// random battery level so that the drone can still fly
                        }
                    }    
                }
            
        }
                

    }

    //functions used in the main menu



}
}
