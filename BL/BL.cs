﻿using System;
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

        /// <summary>
        /// adding a station to the list in the Datalayer
        /// </summary>
        /// <param name="station"></param>
        public void AddStation(Station station)
        {
            try
            {
                if (station.Chargeslots < 0)
                    throw new InvalidInputException("The number of charging slots is less than 0 \n");
                if (station.Id <= 0)// maybe I have to check that it is 3 digits
                    throw new InvalidInputException("The Id is less than zero \n");
                if (station.Loc.Latitude <= -90 || station.Loc.Latitude >= 90)// out of range of latitude
                    throw new InvalidInputException("The latitude is not in a existing range(between -90 and 90) \n");
                if (station.Loc.Longitude <= -180 || station.Loc.Longitude >= 180)// out of range of latitude
                    throw new InvalidInputException("The longitude is not in a existing range(betweeen -180 and 180)\n");
                IDAL.DO.Station st = new();
                station.CopyPropertiestoIDAL(st);
                idal1.AddStation(st);
                station.Charging = new();
            }
            catch(IDAL.DO.DuplicateIdException ex)
            {
                throw new DuplicateIdException("The Station already exists.\n,",ex);
            }
        }

        /// <summary>
        /// adding a customer in the list of the datalayer
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddCustomer(Customer newCustomer)
        {
            try
            {
                if (newCustomer.Id == "\n")
                    throw new InvalidInputException("invalid Id input");
                if (newCustomer.Loc.Latitude <= -90 || newCustomer.Loc.Latitude >= 90)// out of range of latitude
                    throw new InvalidInputException("The latitude is not in a existing range(between -90 and 90) \n");
                if (newCustomer.Loc.Longitude <= -180 || newCustomer.Loc.Longitude >= 180)// out of range of latitude
                    throw new InvalidInputException("The longitude is not in a existing range(betweeen -180 and 180)\n");
                if (newCustomer.PhoneNumber == "\n")
                    throw new InvalidInputException("invalid phonenumber");
                IDAL.DO.Customer customer = new();
                newCustomer.CopyPropertiestoIDAL(customer);
                idal1.AddCustomer(customer);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new DuplicateIdException("The Customer already exists.\n,", ex);
            }


        }

        /// <summary>
        /// adding a drone to the list of the datalayer
        /// </summary>
        /// <param name="newDrone"></param>
        /// <param name="stationId"></param>
        public void AddDrone(Drone newDrone, int stationId)
        {
            if (newDrone.Id < 0)
                throw new InvalidInputException("invalid Id input \n");
            if (newDrone.Loc.Latitude <= -90 || newDrone.Loc.Latitude >= 90)// out of range of latitude
                throw new InvalidInputException("The latitude is not in a existing range(between -90 and 90) \n");
            if (newDrone.Loc.Longitude <= -180 || newDrone.Loc.Longitude >= 180)// out of range of latitude
                throw new InvalidInputException("The longitude is not in a existing range(betweeen -180 and 180)\n");
            if (newDrone.Weight != WeightCategories.heavy && newDrone.Weight != WeightCategories.light && newDrone.Weight != WeightCategories.medium)
                throw new InvalidInputException("Invalid weightCategory \n");
            if (newDrone.Status != DroneStatuses.Available && newDrone.Status != DroneStatuses.Maintenance && newDrone.Status != DroneStatuses.Delivery) 
                throw new InvalidInputException("Invalid status \n");
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
        /// <summary>
        /// updates the drone's model
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        public void updateDrone(int droneId, string model)
        {
            idal1.UpdateDrone(droneId, model);
        }
        /// <summary>
        /// updates the name and amount of chargingslots of the station
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="name"></param>
        /// <param name="chargingSlots"></param>
        public void updateStation(int stationId, string name, int chargingSlots)
        {
            idal1.UpdateStation(stationId, name, chargingSlots);
        }

        /// <summary>
        /// updates the name and phone of the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
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

        public int GetDroneIndex(int ID)
        {
            int index = droneBL.FindIndex(d => d.Id == ID);
            if (index == -1)
            {
                throw new MissingIdException("No such drone\n");
            }
            else
            {
                return index;
            }

            // return DataSource.dronesList.Find(d => d.id == ID);
        }


        public double BatteryUsage(double distance, int pwrIndex )
        {
            return idal1.DronePwrUsg()[pwrIndex] * distance;
        }
        public bool EnoughBattery(double distance, double battery, WeightCategories weight)
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

        public double DroneDistanceFromParcel(IBL.BO.droneToList dr, IDAL.DO.Parcel par)
        {
            double distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, idal1.GetCustomer(par.Sender).Longitude, idal1.GetCustomer(par.Sender).Latitude);
            return distance;
        }
        /// <summary>
        /// measures distance between a customer and a sttion
        /// </summary>
        /// <param name="cus"></param>
        /// <param name="st"></param>
        /// <returns>double</returns>
        public double StationDistanceFromCustomer(IDAL.DO.Customer cus,IDAL.DO.Station st)
        {
            return Bonus.Haversine(cus.Longitude, cus.Latitude, st.Longitude, st.Latitude);
        }

        public double DistanceBetweenCustomers(IDAL.DO.Customer cus1, IDAL.DO.Customer cus2)
        {
            return Bonus.Haversine(cus1.Longitude, cus1.Latitude, cus2.Longitude, cus2.Latitude);
        }

        /// <summary>
        /// assigns a fitting parcel to given drone updating parcel in idal and drone list in bl
        /// </summary>
        /// <param name="droneId"></param>
        public void AssignParcelToDrone(int droneId)
        {
            List<IBL.BO.droneToList> tempDroneList = droneBL;
            List<IDAL.DO.Parcel> tempParcelList = (List<IDAL.DO.Parcel>)idal1.GetParcelList();//data layer parcel list
            int maxW = 0, maxPri = 0;
            double minDis = 0.0;
            int index = GetDroneIndex(droneId);
            if (index == -1)
                throw new MissingIdException("No such drone\n");

            else
            {
                if (droneBL[index].Status == DroneStatuses.Available)
                {
                   
                    IDAL.DO.Parcel? fittingPack = null;//new null temp parcel
                    foreach (IDAL.DO.Parcel pack in tempParcelList)
                       if((int)pack.priority >= maxPri && (int)pack.weight >= maxW && DroneDistanceFromParcel(droneBL[index], pack) < minDis )//pack is only eligible if its weight , distance and priority are bes
                       {
                            
                            double toCus = DroneDistanceFromParcel(droneBL[index], pack);//distance from drones current location to sending customer
                            double toStat = StationDistanceFromCustomer(idal1.GetCustomer(pack.Sender), idal1.SmallestDistanceStation(pack.Sender));//distance from receiver of package to closest charging station
                            double betweenCus = DistanceBetweenCustomers(idal1.GetCustomer(pack.Sender), idal1.GetCustomer(pack.Target));//distance between sender to receiver
                            maxPri = (int)pack.Priority;
                            maxW = (int)pack.Weight;
                            minDis = DroneDistanceFromParcel(droneBL[index], pack);
                            if (BatteryUsage(toCus, 0) + BatteryUsage(toStat, 0) + BatteryUsage(betweenCus, (int)pack.Weight + 1) < droneBL[index].Battery)//enough battery to make the trip
                                fittingPack = pack;//so far this is the most fitting pack for the drone
                                                   
                       }
                    if(fittingPack != null)//if indeed a fitting package has been found
                    {
                        droneBL[GetDroneIndex(droneId)].Status = DroneStatuses.Delivery;//update drone list in BL
                        idal1.ParcelDrone(fittingPack.GetValueOrDefault().Id, droneId);//update parcel in IDAL
                    }
                }
            }
        }
        /// <summary>
        /// pre assigned drone now collects parcel
        /// </summary>
        /// <param name="droneId"></param>
        public void CollectingParcelByDrone(int droneId)
        {
            int index = GetDroneIndex(droneId);
            droneToList tempDro = droneBL[index];
            IDAL.DO.Parcel tempPack = idal1.GetParcel(tempDro.PackageNumber);
            if (!(droneBL[index].Status == DroneStatuses.Delivery && tempPack.PickedUp == null))
                throw new DeliveryIssueException("Parcel cannot be picked up by drone\n");
            tempDro.Battery -= BatteryUsage(DroneDistanceFromParcel(tempDro, tempPack), 0);
            IDAL.DO.Customer tempCus = idal1.GetCustomer(tempPack.Sender);
            Location tempLoc = new()
            {
                Longitude = tempCus.Longitude,
                Latitude =  tempCus.Latitude
            };
            tempDro.Loc = tempLoc;
            idal1.ParcelPickedUp(tempPack.Id);//update parcel to picked up in idal
            droneBL[index] = tempDro;//update drone in list to picked up


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

