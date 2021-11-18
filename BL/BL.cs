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
        internal Location DroneLocation(IDAL.DO.Parcel p, DroneToList tempBl)
        {
            Location locTemp = new();
            if (p.Created != null && p.PickedUp == null)//if assigned but not yet collected
            {
                //location of deone will be in  station closed to the sender
                var stationTemp = idal1.SmallestDistanceStation(p.Sender);
                locTemp.Longitude = stationTemp.Longitude;
                locTemp.Latitude = stationTemp.Latitude;
            }
            if (p.Created != null && p.PickedUp != null)// if pakage is assigned and picked up
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

                    }
                    else
                    {
                        tempBl.battery = rand.Next(20);// random battery level so that the drone can still fly
                    }
                }
            }

        }









    




        double BatteryUsage(double distance, int pwrIndex )
        {
            return idal1.DronePwrUsg()[pwrIndex] * distance;
        }
        //public bool EnoughBattery(double distance, double battery, WeightCategories weight)
        //{

        //}

         void UpdateBattery()
        {

        }

        /// <summary>
        /// returns a station which is has charging and the drone has enough battery to fly to
        /// </summary>
        /// <param name="withCharging"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public IDAL.DO.Station FindPossibleStation(List<IDAL.DO.Station> withCharging, DroneToList dr)
        {
            double minDistance = double.MaxValue;IDAL.DO.Station temp = new();
            foreach (IDAL.DO.Station st in withCharging)
            {
                double distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, st.Longitude, st.Latitude);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    temp = st;
                }                     
            }
            if (BatteryUsage(minDistance, 0) < dr.Battery) return temp;
            throw new catgettochargeException;
        }

        /// <summary>
        /// release teh drone from charge, updates the battery according to the charging time
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="chargingTime"></param>
        public void ReleasingDroneFromCharge(int droneId, double chargingTime)
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
                    DroneToList tempDrone = droneBL[index];
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

        public double DroneDistanceFromParcel(IBL.BO.DroneToList dr, IDAL.DO.Parcel par)
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
            List<IBL.BO.DroneToList> tempDroneList = droneBL;
            List<IDAL.DO.Parcel> tempParcelList = (List<IDAL.DO.Parcel>)idal1.GetAllParcels();//data layer parcel list
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
                       if((int)pack.Priority >= maxPri && (int)pack.Weight >= maxW && DroneDistanceFromParcel(droneBL[index], pack) < minDis )//pack is only eligible if its weight , distance and priority are bes
                       {
                            
                            double toCus = DroneDistanceFromParcel(droneBL[index], pack);//distance from drones current location to sending customer
                            double toStat = StationDistanceFromCustomer(idal1.GetCustomer(pack.Sender), idal1.SmallestDistanceStation(pack.Sender));//distance from receiver of package to closest charging station
                            double betweenCus = DistanceBetweenCustomers(idal1.GetCustomer(pack.Sender), idal1.GetCustomer(pack.Receiver));//distance between sender to receiver
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
            DroneToList tempDro = droneBL[index];
            IDAL.DO.Parcel tempPack = idal1.GetParcel(tempDro.ParcelId);
            if (!(droneBL[index].Status == DroneStatuses.Delivery && tempPack.PickedUp == null))
                throw new DeliveryIssueException("Parcel cannot be picked up by drone\n");
            tempDro.Battery -= BatteryUsage(DroneDistanceFromParcel(tempDro, tempPack), 0);
            IDAL.DO.Customer tempCus = idal1.GetCustomer(tempPack.Sender);
            Location tempLoc = new()
            {
                Longitude = tempCus.Longitude,
                Latitude =  tempCus.Latitude
            };
            tempDro.Loc.Longitude = tempCus.Longitude;
            idal1.ParcelPickedUp(tempPack.Id);//update parcel to picked up in idal
            droneBL[index] = tempDro;//update drone in list to picked up


        }

        public void DeliverParcelByDrone(int droneId)
        {
            int index = GetDroneIndex(droneId);
            DroneToList tempDro = droneBL[index];
            IDAL.DO.Parcel tempPack = idal1.GetParcel(tempDro.ParcelId);
            if (!(droneBL[index].Status == DroneStatuses.Delivery && tempPack.Delivered == null))
                throw new DeliveryIssueException("Parcel cannot be delivered by drone\n");
            IDAL.DO.Customer tempCus = idal1.GetCustomer(tempPack.Receiver);
            tempDro.Battery -= BatteryUsage(DistanceBetweenCustomers(idal1.GetCustomer(tempPack.Sender), tempCus),(int)tempPack.Weight + 1);//calculates the battery usage in delivery according to the weight of the package
            tempDro.Loc.Longitude = tempCus.Longitude;
            tempDro.Loc.Latitude = tempCus.Latitude;
            tempDro.Status = DroneStatuses.Available;
            idal1.ParcelDelivered(tempPack.Id);
        }

        public DroneToList getDroneToList(int droneId)
        {
            int index = droneBL.FindIndex(d => d.Id == droneId);
            if (index == -1)
                throw new MissingIdException("No such drone\n");
            else
                return droneBL[index];
        }

        public ParcelInTransfer GetParcelInTransfer(int parcelId)
        {
            Parcel parcel = new();
            ParcelInTransfer parcelInTrans = new();
            parcel = GetParcel(parcelId);
            parcel.CopyPropertiestoIBL(parcelInTrans);
            parcelInTrans.DeliverdTo= GetCustomer(parcel.Receiver.Id).Loc;
            parcelInTrans.PickedUp = GetCustomer(parcel.Sender.Id).Loc;
            parcelInTrans.Distance = Bonus.Haversine(parcelInTrans.DeliverdTo.Longitude, parcelInTrans.DeliverdTo.Latitude, parcelInTrans.PickedUp.Longitude, parcelInTrans.PickedUp.Latitude);
            if (parcel.PickedUp == null)
                parcelInTrans.status = true;
            else
                parcelInTrans.status = false;
            return parcelInTrans;
        }

        public ParcelAtCustomer GetParcelAtCustomer(int parcelId)
        {
            Parcel parcel = new();
            ParcelAtCustomer parcelAtCustomer = new();
            parcel = GetParcel(parcelId);
            parcel.CopyPropertiestoIBL(parcelAtCustomer);
            parcelAtCustomer.ParcelStatus = GetParcelStatus(parcel);
            if (parcel.Dr.Loc == GetCustomer(parcel.Sender.Id).Loc)// if the location is same as sender
                parcelAtCustomer.CustomerInP = parcel.Receiver;
            else
                parcelAtCustomer.CustomerInP = parcel.Sender;
            return parcelAtCustomer;
        }

        public ParcelStatuses GetParcelStatus(Parcel parcel)
        {
            if (parcel.Delivered != null)
                 return ParcelStatuses.Delivered;
            if (parcel.PickedUp != null)
                return ParcelStatuses.PickedUp;
            if (parcel.Assigned != null)
                return ParcelStatuses.Assigned;
            else
                return ParcelStatuses.Created;

        }

        public Drone getDrone(int droneId)
        {
            DroneToList droneToList = getDroneToList(droneId);
            Drone drone = new();
            droneToList.CopyPropertiestoIBL(drone);
            if (droneToList.ParcelId == 0)// if the drone doesn't hold a parcel
                drone.parcelInTrans = null;
           else
               drone.parcelInTrans = GetParcelInTransfer(droneToList.ParcelId);
            return drone;

        }

        public Customer GetCustomer(string customerId)
        {
            Customer customer = new();
            idal1.GetCustomer(customerId).CopyPropertiestoIBL(customer);
            List<IDAL.DO.Parcel>ReceivedParcelListDal= (List< IDAL.DO.Parcel >)idal1.GetCustomerReceivedParcels(customerId);
            List<IDAL.DO.Parcel>SentParcelListDal= (List<IDAL.DO.Parcel>)idal1.GetCustomerSentParcels(customerId);
            ReceivedParcelListDal.ForEach(p => { customer.ReceivedParcels.Add(GetParcelAtCustomer(p.Id)); });// changes the list to a ParcelAtCustomerList
            SentParcelListDal.ForEach(p => { customer.SentParcels.Add(GetParcelAtCustomer(p.Id)); });
            return customer;
        }

        public Parcel GetParcel(int parcelId)
        {
            Parcel parcel = new();
            IDAL.DO.Parcel parcelDal = idal1.GetParcel(parcelId);
            parcelDal.CopyPropertiestoIBL(parcel);
            idal1.GetCustomer(parcelDal.Sender).CopyPropertiestoIBL(parcel.Sender);// converts to CustomerInParcel
            idal1.GetCustomer(parcelDal.Receiver).CopyPropertiestoIBL(parcel.Receiver);
            getDrone(parcelDal.DroneId).CopyPropertiestoIBL(parcel.Dr);
            return parcel;
        }

        public Station GetStation(int stationId)
        {
            Station station = new();
            IDAL.DO.Station stationDal = idal1.GetStation(stationId);
            stationDal.CopyPropertiestoIBL(station);
            List<IDAL.DO.Station> chargingListIdal=(List<IDAL.DO.Station>)idal1.DronesChargingAtStation(stationId);
            chargingListIdal.CopyPropertyListtoIBLList(station.Charging);// converts the list to a DroneInChargeLists
            return station;
        }

        public IEnumerable<StationToList> GetAllStation()
        {
            List<StationToList> tempList = new();
            int[] slots;
            idal1.GetAllStations().CopyPropertyListtoIBLList(tempList);
            foreach(StationToList temp in tempList)
            {
                slots = idal1.AvailableAndEmptySlots(temp.Id);
                temp.OccupiedSlots = slots[0];
                temp.AvailableSlots = slots[1];
            }
            return tempList;
        }

        public IEnumerable<DroneToList> GetAllDrones()
        {
            return droneBL;
        }

        public IEnumerable<CustomerToList> GetAllCustomers()
        {
            List<CustomerToList> tempList = new();
            idal1.GetAllCustomers().CopyPropertyListtoIBLList(tempList);
            foreach (CustomerToList cus in tempList)
            {
                cus.NumPacksReceived = idal1.DeliveredParcels().Where(p => p.Receiver == cus.Id).Count();
                cus.NumPackSentDel = idal1.DeliveredParcels().Where(p => p.Sender == cus.Id).Count();
                cus.NumPackExp = idal1.UndeliveredParcels().Where(p => p.Receiver == cus.Id).Count();
                cus.NumPackSentDel = idal1.UndeliveredParcels().Where(p => p.Sender == cus.Id).Count();
            }
            return tempList;
        }
        /// <summary>
        /// returns all parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PackageToList> GetAllParcels()
        {
            List<PackageToList> tempList = new();
            idal1.GetAllParcels().CopyPropertyListtoIBLList(tempList);
            return tempList;
        }
        /// <summary>
        /// returns all unassigned parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PackageToList> GetAllUnassignedParcels()
        {
            List<PackageToList> tempList = new();
            idal1.UnAssignedParcels().CopyPropertyListtoIBLList(tempList);
            return tempList;
        }
        /// <summary>
        /// returns all stations with available charging slots
        /// </summary>
        public IEnumerable<StationToList> GetAllStationsWithCharging()
        {
            List<StationToList> tempList = new();
            idal1.GetStationWithCharging().CopyPropertyListtoIBLList(tempList);
            return tempList;
        }
    }
}

