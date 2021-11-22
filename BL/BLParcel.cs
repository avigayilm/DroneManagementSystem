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
    public partial class BL
    {
        /// <summary>
        /// adds a parcel to the parcelList
        /// </summary>
        /// <param name="newParcel"></param>
        public void AddParcel(Parcel newParcel)// do I have to check customer and receiver
        {
            try
            {
                if (newParcel.Id < 0)
                    throw new InvalidInputException("invalid Id input \n");
                if (newParcel.Priority != Priorities.Emergency && newParcel.Priority != Priorities.Fast && newParcel.Priority != Priorities.Normal)
                    throw new InvalidInputException("Invalid weightCategory \n");
                if (newParcel.Weight != WeightCategories.Heavy && newParcel.Weight != WeightCategories.Light && newParcel.Weight != WeightCategories.Medium)
                    throw new InvalidInputException("Invalid weightCategory \n");
                if (string.IsNullOrEmpty(newParcel.Sender.Id))
                    throw new InvalidInputException("invalid senderId \n");
                if (string.IsNullOrEmpty(newParcel.Receiver.Id))
                    throw new InvalidInputException("invalid receiverId \n");
                newParcel.Delivered = null;
                newParcel.Assigned = null;
                newParcel.PickedUp = null;
                newParcel.Dr = null;
                IDAL.DO.Parcel parcelTemp = new();
                newParcel.CopyPropertiestoIDAL(parcelTemp);
                idal1.AddParcel(parcelTemp);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new AddingException("Couldn't add the parcel.\n,", ex);
            }
        }

        /// <summary>
        /// assigns a fitting parcel to given drone updating parcel in idal and drone list in bl
        /// </summary>
        /// <param name="droneId"></param>
        public void AssignParcelToDrone(int droneId)
        {
            try
            {
                int index = idal1.CheckExistingDrone(droneId);
                List<IBL.BO.DroneToList> tempDroneList = droneBL;
                List<IDAL.DO.Parcel> tempParcelList = idal1.GetAllParcels(p => p.Assigned == null && (int)p.Weight <= (int)droneBL[index].Weight).ToList();//data layer parcel list
                int maxW = 0, maxPri = 0;
                double minDis = 0.0;

                if (droneBL[index].Status == DroneStatuses.Available)
                {
                    IDAL.DO.Parcel? fittingPack = null;//new null temp parcel
                    foreach (IDAL.DO.Parcel pack in tempParcelList)
                    {
                        double droneDisFromPack = DroneDistanceFromParcel(droneBL[index], pack);
                        if ((int)pack.Priority >= maxPri && (int)pack.Weight >= maxW && droneDisFromPack < minDis)//pack is only eligible if its weight , distance and priority are bes
                        {
                            
                            double toStat = StationDistanceFromCustomer(idal1.GetCustomer(pack.Sender), idal1.SmallestDistanceStation(pack.Sender));//distance from receiver of package to closest charging station
                            double betweenCus = DistanceBetweenCustomers(idal1.GetCustomer(pack.Sender), idal1.GetCustomer(pack.Receiver));//distance between sender to receiver
                            if (BatteryUsage(droneDisFromPack, 0) + BatteryUsage(toStat, 0) + BatteryUsage(betweenCus, (int)pack.Weight + 1) < droneBL[index].Battery)//enough battery to make the trip
                                fittingPack = pack;//so far this is the most fitting pack for the drone.
                            maxPri = (int)pack.Priority;
                            maxW = (int)pack.Weight;
                            minDis = droneDisFromPack;
                        }
                    }
                    if (fittingPack != null)//if indeed a fitting package has been found
                    {
                        droneBL[index].Status = DroneStatuses.Delivery;//update drone list in BL
                        idal1.ParcelDrone(fittingPack.GetValueOrDefault().Id, droneId);//update parcel in IDAL
                    }
                }
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new AssignIssueException("Couldn't assign the Parcel the parcel.\n,", ex);
            }
        }

        /// <summary>
        /// pre assigned drone now collects parcel
        /// </summary>
        /// <param name="droneId"></param>
        public void CollectingParcelByDrone(int droneId)
        {
            try
            {
                int index = idal1.CheckExistingDrone(droneId);
                DroneToList tempDro = droneBL[index];
                IDAL.DO.Parcel tempPack = idal1.GetParcel(tempDro.ParcelId);
                if (!(droneBL[index].Status == DroneStatuses.Delivery && tempPack.PickedUp == null))
                    throw new DeliveryIssueException("Parcel cannot be picked up by drone\n");
                tempDro.Battery -= BatteryUsage(DroneDistanceFromParcel(tempDro, tempPack), 0);
                IDAL.DO.Customer tempCus = idal1.GetCustomer(tempPack.Sender);
                Location tempLoc = new()
                {
                    Longitude = tempCus.Longitude,
                    Latitude = tempCus.Latitude
                };
                tempDro.Loc.Longitude = tempCus.Longitude;
                idal1.ParcelPickedUp(tempPack.Id);//update parcel to picked up in idal
                droneBL[index] = tempDro;//update drone in list to picked up
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Drone\n,", ex);
            }


        }
        /// <summary>
        /// delivering the parcel 
        /// </summary>
        /// <param name="droneId"></param>

        public void DeliverParcelByDrone(int droneId)
        {
            try
            {
                int index = idal1.CheckExistingDrone(droneId);
                DroneToList tempDro = droneBL[index];
                IDAL.DO.Parcel tempPack = idal1.GetParcel(tempDro.ParcelId);
                if (!(droneBL[index].Status == DroneStatuses.Delivery && tempPack.Delivered == null))
                    throw new DeliveryIssueException("Parcel cannot be delivered by drone\n");
                IDAL.DO.Customer tempCus = idal1.GetCustomer(tempPack.Receiver);
                tempDro.Battery -= BatteryUsage(DistanceBetweenCustomers(idal1.GetCustomer(tempPack.Sender), tempCus), (int)tempPack.Weight + 1);//calculates the battery usage in delivery according to the weight of the package
                tempDro.Loc.Longitude = tempCus.Longitude;
                tempDro.Loc.Latitude = tempCus.Latitude;
                tempDro.Status = DroneStatuses.Available;
                idal1.ParcelDelivered(tempPack.Id);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Drone.\n,", ex);
            }
        }
        /// <summary>
        /// returning a ParcelInTransfer
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public ParcelInTransfer GetParcelInTransfer(int parcelId)
        {
            Parcel parcel = new();
            ParcelInTransfer parcelInTrans = new();
            parcel = GetParcel(parcelId);
            parcel.CopyPropertiestoIBL(parcelInTrans);
            parcelInTrans.DeliverdTo = GetCustomer(parcel.Receiver.Id).Loc;
            parcelInTrans.PickedUp = GetCustomer(parcel.Sender.Id).Loc;
            parcelInTrans.Distance = Bonus.Haversine(parcelInTrans.DeliverdTo.Longitude, parcelInTrans.DeliverdTo.Latitude, parcelInTrans.PickedUp.Longitude, parcelInTrans.PickedUp.Latitude);
            if (parcel.PickedUp == null)
                parcelInTrans.Status = true;
            else
                parcelInTrans.Status = false;
            return parcelInTrans;
        }
        /// <summary>
        /// returns the status of the Parcel
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public ParcelStatuses GetParcelStatus(int parcelId)
        {
            Parcel parcel=GetParcel(parcelId);
            if (parcel.Delivered != null)
                return ParcelStatuses.Delivered;
            if (parcel.PickedUp != null)
                return ParcelStatuses.PickedUp;
            if (parcel.Assigned != null)
                return ParcelStatuses.Assigned;
            else
                return ParcelStatuses.Created;

        }
        /// <summary>
        /// returning a ParcelAtCustomer
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public ParcelAtCustomer GetParcelAtCustomer(int parcelId)
        {
            Parcel parcel = new();
            ParcelAtCustomer parcelAtCustomer = new();
            parcel = GetParcel(parcelId);
            parcel.CopyPropertiestoIBL(parcelAtCustomer);
            parcelAtCustomer.ParcelStatus = GetParcelStatus(parcelId);
            if (parcel.Dr.Loc == GetCustomer(parcel.Sender.Id).Loc)// if the location is same as sender
                parcelAtCustomer.CustomerInP = parcel.Receiver;
            else
                parcelAtCustomer.CustomerInP = parcel.Sender;
            return parcelAtCustomer;
        }
        /// <summary>
        /// returns a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId)
        {
            try
            {
                Parcel parcel = new();
                IDAL.DO.Parcel parcelDal = idal1.GetParcel(parcelId);
                parcelDal.CopyPropertiestoIBL(parcel);
                idal1.GetCustomer(parcelDal.Sender).CopyPropertiestoIBL(parcel.Sender);// converts to CustomerInParcel
                idal1.GetCustomer(parcelDal.Receiver).CopyPropertiestoIBL(parcel.Receiver);
                GetDrone(parcelDal.DroneId).CopyPropertiestoIBL(parcel.Dr);
                return parcel;
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Parcel.\n,", ex);
            }
        }

        /// <summary>
        /// returns all parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelToList> GetAllParcels()
        {
            List<ParcelToList> tempList = new();
            idal1.GetAllParcels().CopyPropertyListtoIBLList(tempList);
            return tempList;
        }

        /// <summary>
        /// returns all unassigned parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelToList> GetAllUnassignedParcels()
        {
            List<ParcelToList> tempList = new();
            idal1.GetAllParcels(p => p.Assigned == null).CopyPropertyListtoIBLList(tempList);
            foreach(ParcelToList p in tempList)
            {
                p.parcelStatus = GetParcelStatus(p.Id);
            }
            return tempList;
        }

    }
}
