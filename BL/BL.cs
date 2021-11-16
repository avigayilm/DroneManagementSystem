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
            if (p.Requested != null && p.PickedUp == null)//if assigned but not yet collected
            {
                //location of deone will be in  station closed to the sender
                var stationTemp = idal1.SmallestDistanceStation(p.Sender);
                locTemp.Longitude = stationTemp.Longitude;
                locTemp.Latitude = stationTemp.Latitude;
            }
            if (p.Requested != null && p.PickedUp != null)// if pakage is assigned and picked up
            {
                //location wIll be at the sender
                List<IDAL.DO.Customer> tempCustomerList = (List<IDAL.DO.Customer>)idal1.GetAllCustomers();
                int customerIndex = tempCustomerList.FindIndex(c => c.Id == p.Sender);
                if (customerIndex != -1)
                {
                    locTemp.Longitude = tempCustomerList[customerIndex].Longitude;
                    locTemp.Latitude = tempCustomerList[customerIndex].Latitude;
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
            int index = tempStat.FindIndex(d => d.Id == stationId);
            newDrone.Loc = new() { Longitude = tempStat[index].Longitude, Latitude = tempStat[index].Latitude };
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
                if (droneBL[index].Status == DroneStatuses.Available && tempStation.Id != 0)//if it's available and there is enough battery
                {
                    //update the drone
                    tempDrone.Loc.Longitude = tempStation.Longitude;
                    tempDrone.Loc.Latitude = tempStation.Latitude;
                    UpdateBattery();
                    tempDrone.Status = DroneStatuses.Maintenance;
                    //update the station
                    idal1.ChangeChargeSlots(tempStation.Id, -1);
                    //update dronecharge
                    idal1.SendToCharge(droneId, tempStation.Id);

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

        /// <summary>
        /// returns a station which is has charging and the drone has enough battery to fly to
        /// </summary>
        /// <param name="withCharging"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public IDAL.DO.Station FindPossibleStation(List<IDAL.DO.Station> withCharging, droneToList dr)
        {
            foreach (IDAL.DO.Station st in withCharging)
            {
                double distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, st.Longitude, st.Latitude);
                if (EnoughBattery(distance, dr.Battery))
                    return st;
            }
            IDAL.DO.Station temp = new() { Id = 0 };
            return temp;

        }

        /// <summary>
        /// release teh drone from charge, updates the battery according to the charging time
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="chargingTime"></param>
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
                    int stationId = tempStWithCharging[droneChargeIndex].StationId;
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

        /// <summary>
        /// assign a parcel to a drone
        /// </summary>
        /// <param name="droneId"></param>
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

                    Priorities maxPriIndex = 0;
                    int maxWeightIndex=0, nearest=10000;
                    IDAL.DO.Parcel tempParcel;
                    List<IDAL.DO.Parcel> tempParcelList = (List<IDAL.DO.Parcel>)idal1.getAllParcels();
                    foreach(IDAL.DO.Parcel p in tempParcelList)
                    {
                        if(maxPriIndex<p.Priority)
                        {

                        }
                    }
                    for(int i=0;i<tempParcelList.Count;i++)
                    {

                    }

                    //update data layer
                    idal1.ParcelDrone(parcelId, droneId);
                }
                
            }

            

        }
        public void CollectingParcelByDrone(int droneId)
        {

        }

        public void DeliverParcelByDrone(int droneId)
        {

        }

        /// <summary>
        /// returning a station from BL gettting it from dalObject
        /// </summary>
        /// <param name="stationId"></param>
        public Station GetStation(int stationId)
        {
            Station station = new();
            idal1.GetStation(stationId).CopyPropertiestoIDAL(station);
            return station;
            //Station st=station.
        }

        public Drone getDrone(int droneId)
        {
            int index = droneBL.FindIndex(d => d.Id == droneId);
            if (index == -1)
                throw new MissingIdException("No such drone\n");
            else
                return droneBL[index];


        }

        public Customer getCustomer(string customerId)
        {
            Customer customer = new();
            idal1.GetCustomer(customerId).CopyPropertiestoIDAL(customer);
            customer.ReceivedParcels = new();
            customer.SentParcels = new();
            customer.
            return customer;
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

        public void getAllStationsWithCharging()
        {

        }
    }
}

