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
        internal List<IBL.BO.droneToList> droneBL = new();
        IDal idal1 = new DAL.DalObject();
        internal Location DroneLocation(IDAL.DO.Parcel p, droneToList tempBl)
        {
            Location locTemp = new();
            if (p.requested != null && p.pickedUp == null)//if assigned but not yet collected
            {
                //location of deone will be in  station closed to the sender
                var stationTemp = idal1.smallestDistanceStation(p.senderid);
                locTemp.Longitude = stationTemp.longitude;
                locTemp.Latitude = stationTemp.latitude;
            }
            if (p.requested != null && p.pickedUp != null)// if pakage is assigned and picked up
            {
                //location wIll be at the sender
                List<IDAL.DO.Customer> tempCustomerList = (List<IDAL.DO.Customer>)idal1.GetAllCustomers();
                int customerIndex = tempCustomerList.FindIndex(c => c.id == p.senderid);
                if (customerIndex != -1)
                {
                    locTemp.Longitude = tempCustomerList[customerIndex].longitude;
                    locTemp.Latitude = tempCustomerList[customerIndex].latitude;
                }

            }
            return locTemp;
        }
        public BL()
        {
            //DAL.DalObject dal2 = new();

            double[] tempArray = idal1.DronePwrUsg();
            double pwrUsgEmpty = tempArray[0];
            double pwrUsgLight = tempArray[1];
            double pwrUsgMedium = tempArray[2];
            double pwrUsgHeavy = tempArray[3];
            double chargePH = tempArray[4];

            List<IDAL.DO.Drone> tempDroneList = (List<IDAL.DO.Drone>)idal1.GetAllDrones();
            droneBL = (List<IBL.BO.droneToList>)tempDroneList.ToBLDroneList();// converts the dronelist to IBL
            List<IDAL.DO.Parcel> undeliveredParcel = (List<IDAL.DO.Parcel>)idal1.UndeliveredParcels();

            foreach (IDAL.DO.Parcel p in undeliveredParcel)
            {
                int DroneId = p.droneId;
                int droneIndex = droneBL.FindIndex(d => d.Id == DroneId);
                if (droneIndex > 0)// if there is a drone assigned to the parcel
                {
                    var tempBl = droneBL[droneIndex];
                    tempBl.Status = DroneStatuses.Delivery;
                    tempBl.Battery = rand.Next(40, 100);// random battery level so that the drone can still fly
                    tempBl.Loc = DroneLocation(p, tempBl);//location of drone
                }
            }
            foreach (Drone dr in droneBL)
            {
                if (dr.Status != DroneStatuses.Delivery)
                {
                    Drone tempDr = dr;
                    tempDr.Status = (DroneStatuses)rand.Next(1);
                    if (dr.Status == DroneStatuses.Available)
                    {

                    }
                    else
                    {
                        tempBl.battery = rand.Next(20);// random battery level so that the drone can still fly
                    }
                }
            }

        }




        //functions used in the main menu
        public void AddStation(Station tempStat)
        {
            idal1.AddStation(tempStat);
            tempStat.Charging = new();

            //List<IDAL.DO.Station> stat = (List<IDAL.DO.Station>)idal1.GetAllCustomers();
            //int index = stat.FindIndex(c => c.id == tempStat.Id);
            //if (index != -1)
            //{
            //    throw new DuplicateIdException("Customer already exists\n");
            //}
            //else
            //{
            //    DataSource.customerList.Add(cus);
            //}
        }
        public void AddCustomer(Customer newCustomer)
        {
            idal1.AddCustomer(newCusomter)
        }
        public void AddDrone(Drone newDrone, int stationId)
        {
            newDrone.Battery = rand.Next(20, 40);
            newDrone.Status = DroneStatuses.Maintenance;
            //location of station id
            List<IDAL.DO.Station> tempStat = (List<IDAL.DO.Station>)idal1.GetAllStations();
            int index = tempStat.FindIndex(d => d.id == stationId);
            newDrone.Loc = new() { Longitude = tempStat[index].longitude, Latitude = tempStat[index].latitude };
            idal1.AddStation(tempStat);
        }
        public void AddParcel(Parcel newParcel)
        {
            newParcel.Delivered = null;
            newParcel.Scheduled = null;
            newParcel.PickedUp = null;
            newParcel.Dr = null;
            idal1.AddParcel(newParcel);
        }

        public void updateDrone(int droneId, string model)
        {
            idal1.UpdateDrone(droneId, model);
        }

        public void updateStation(int stationId, string name, int chargingSlots)
        {
            idal1.UpdateStation(stationId, name, chargingSlots);
        }

        public void updateCustomer(int customerId, string name, string phone)
        {
            updateCustomer(customerId, name, phone);
        }

        public void sendingDroneToCharge(int droneId)
        {
            int index = droneBL.FindIndex(d => d.Id == droneId);
            if (index == -1)
                throw new MissingIdException("No such drone\n");
            else
            {
                List<IDAL.DO.Station> tempStWithCharging = (List<IDAL.DO.Station>)idal1.GetStationWithCharging();
                IDAL.DO.Station tempStation = FindPossibleStation(tempStWithCharging, droneBL[index]);// returns a station that the drone can fly to. if id=-1 there is no such station
                droneToList tempDrone = droneBL[index];
                if (droneBL[index].Status == DroneStatuses.Available && tempStation.id != 0)//if it's available and there is enough battery
                {
                    //update the drone
                    tempDrone.Loc.Longitude = tempStation.longitude;
                    tempDrone.Loc.Latitude = tempStation.latitude;
                    UpdateBattery();
                    tempDrone.Status = DroneStatuses.Maintenance;
                    //update the station
                    idal1.ChangeChargeSlots(tempStation.id, -1);
                    //update dronecharge
                    idal1.SendToCharge(droneId, tempStation.id);

                }
                else
                    throw new MissingIdException("No such drone\n");// throw approptate acception
            }


        }

        public bool EnoughBattery(double distance, double battery)
        {

        }

        public void UpdateBattery()
        {

        }
        public IDAL.DO.Station FindPossibleStation(List<IDAL.DO.Station> withCharging, droneToList dr)
        {
            foreach (IDAL.DO.Station st in withCharging)
            {
                double distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, st.longitude, st.latitude);
                if (EnoughBattery(distance, dr.Battery))
                    return st;
            }
            IDAL.DO.Station temp = new() { id = 0 };
            return temp;

        }

        public void releasingDroneFromCharge(int droneId, double chargingTime)
        {
            int index = droneBL.FindIndex(d => d.Id == droneId);
            if (index == -1)
                throw new MissingIdException("No such drone\n");
            else
            {
                if (droneBL[index].Status == DroneStatuses.Maintenance)
                {
                    List<IDAL.DO.DroneCharge> tempStWithCharging = (List<IDAL.DO.DroneCharge>)idal1.GetDroneChargeList();
                    int droneChargeIndex = droneBL.FindIndex(d => d.Id == droneId);// finding the index of drone to get the station id
                    int stationId = tempStWithCharging[droneChargeIndex].stationId;
                    droneToList tempDrone = droneBL[index];
                    // update drone
                    UpdateBattery();
                    tempDrone.Status = DroneStatuses.Available;
                    //update station
                    idal1.ChangeChargeSlots(stationId, 1);
                    //update dronecharge
                    idal1.BatteryCharged(droneId, stationId);
                }
                else
                    throw new MissingIdException("No such drone\n");// throw approptate acception
            }
        }

        public void AssignParcelToDrone(int droneId)
        {
            List<IDAL.DO.Drone> tempDroneList =(List<IDAL.DO.Drone>) idal1.GetAllDrones();
            int index = tempDroneList.FindIndex(d => d.Id == droneId);
            if (index == -1)
                throw new MissingIdException("No such drone\n");
            else
            {
                if (droneBL[index].Status == DroneStatuses.Available)
                {

                    int maxPriIndex=0, maxWeightIndex=0, nearest=10000;
                    List<IDAL.DO.Parcel> tempParcelList = (List<IDAL.DO.Parcel>)idal1.getAllParcels();
                    foreach(IDAL.DO.Parcel p in tempParcelList)
                    {

                    }
                    for(int i=0;i<tempParcelList.Count;i++)
                    {

                    }

                    //update data layer
                    idal1.ParcelDrone(parcelId, droneId);
                }
                
            }

            

        }
        public void CollectingParcelByDrone()
        {

        }

        public void DeliverParcelByDrone()
        {

        }

        public void GetStation()
        {

        }

        public void getDrone()
        {

        }

        public void getCustomer()
        {

        }

        public void getParcel()
        {

        }

        public void getStation()
        {

        }

        public void getAllStation()
        {

        }

        public void getAllDrones()
        {

        }

        public void getAllCustomers()
        {

        }

        public void getAllParcels()
        {

        }

        public void getAllUnassignedParcels()
        {

        }
    }
}

