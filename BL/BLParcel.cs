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
                    throw new InvalidInputException("invalid Id input");
                if (newParcel.Priority != Priorities.Emergency && newParcel.Priority != Priorities.Fast && newParcel.Priority != Priorities.Normal)
                    throw new InvalidInputException("Invalid weightCategory");
                if (newParcel.Weight != WeightCategories.Heavy && newParcel.Weight != WeightCategories.Light && newParcel.Weight != WeightCategories.Medium)
                    throw new InvalidInputException("Invalid weightCategory");
                if (string.IsNullOrEmpty(newParcel.Sender.Id))
                    throw new InvalidInputException("invalid inputId");
                if (string.IsNullOrEmpty(newParcel.Receiver.Id))
                    throw new InvalidInputException("invalid inputId");
                newParcel.Delivered = null;
                newParcel.Assigned = null;
                newParcel.PickedUp = null;
                newParcel.Dr = null;
                IDAL.DO.Parcel parcelTemp = new();
                object obj1 = parcelTemp;
                newParcel.CopyProperties(obj1);
                parcelTemp = (IDAL.DO.Parcel)obj1;
                parcelTemp.SenderId = newParcel.Sender.Id;// only insert if sender exists?
                parcelTemp.ReceiverId = newParcel.Receiver.Id;
                idal1.AddParcel(parcelTemp);// can we print the parcelId here
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new AddingException("Couldn't add the parcel.", ex);
            }
        }

        /// <summary>
        /// assigns a fitting parcel to given drone updating parcel in idal and drone list in bl
        /// </summary>
        /// <param name="droneId"></param>
        public void AssignParcelToDrone(int droneId)
        {
            DroneToList drone = getDroneToList(droneId);
            var tempParcelList = idal1.GetAllParcels(p => p.Assigned == null && (int)p.Weight <= (int)drone.Weight);//.OrderBy(i => (int)i.Priority).ThenBy(i => (int)i.Weight).ThenByDescending(i => DroneDistanceFromParcel(drone, i)); //data layer parcel list
            int maxW = 0, maxPri = 0;
            double minDis = 0.0;

            if (drone.Status == DroneStatuses.Available)
            {
                IDAL.DO.Parcel? fittingPack = null; // new null temp parcel
                foreach (IDAL.DO.Parcel pack in tempParcelList)
                {
                    double droneDisFromPack = DroneDistanceFromParcel(drone, pack);
                    // pack is only eligible if its weight , distance and priority are best
                    if ((int)pack.Priority > maxPri ||
                    (int)pack.Priority >= maxPri && (int)pack.Weight > maxW ||
                    (int)pack.Priority >= maxPri && (int)pack.Weight >= maxW && droneDisFromPack < minDis)
                    {
                        // distance from receiver of package to closest charging station
                        double toStat = StationDistanceFromCustomer(idal1.GetCustomer(pack.SenderId), idal1.SmallestDistanceStation(pack.SenderId));
                        double betweenCus = DistanceBetweenCustomers(idal1.GetCustomer(pack.SenderId), idal1.GetCustomer(pack.ReceiverId));//distance between sender to receiver
                        if (BatteryUsage(droneDisFromPack, 0) + BatteryUsage(toStat, 0) + BatteryUsage(betweenCus, (int)pack.Weight + 1) < drone.Battery)//enough battery to make the trip
                            fittingPack = pack;//so far this is the most fitting pack for the drone.
                        maxPri = (int)pack.Priority;
                        maxW = (int)pack.Weight;
                        minDis = droneDisFromPack;
                    }

                    }
                    if (fittingPack != null) // if indeed a fitting package has been found
                    {
                        drone.Status = DroneStatuses.Delivery;//update drone list in BL
                        idal1.ParcelDrone(fittingPack.GetValueOrDefault().Id, droneId);//update parcel in IDAL
                    }
                    else
                        throw new BatteryIssueException("Drone hasn't got enough battery to carry any parcel");
                }
        }

      
        public void CollectingParcelByDrone(int droneId)
        {
                DroneToList drone = droneBL.FirstOrDefault(d => d.Id == droneId);
                IDAL.DO.Parcel tempPack = idal1.GetParcel(drone.ParcelId);
                if (!(drone.Status == DroneStatuses.Delivery && tempPack.PickedUp == null))
                    throw new DeliveryIssueException("Parcel cannot be picked up by drone\n");
                drone.Battery -= BatteryUsage(DroneDistanceFromParcel(drone, tempPack), 0);
                IDAL.DO.Customer tempCus = idal1.GetCustomer(tempPack.SenderId);
                Location tempLoc = new()
                {
                    Longitude = tempCus.Longitude,
                    Latitude = tempCus.Latitude
                };
                drone.Loc.Longitude = tempCus.Longitude;
                idal1.ParcelPickedUp(tempPack.Id);//update parcel to picked up in idal


        }

        public void DeliverParcelByDrone(int droneId)
        {
                DroneToList tempDro = droneBL.FirstOrDefault(d => d.Id == droneId);
                if(tempDro == default)
                    throw new RetrievalException("Couldn't get the Drone.\n,");
                IDAL.DO.Parcel tempPack = idal1.GetParcel(tempDro.ParcelId);
                if (!(tempDro.Status == DroneStatuses.Delivery && tempPack.Delivered == null))
                    throw new DeliveryIssueException("Parcel cannot be delivered by drone\n");
                IDAL.DO.Customer tempCus = idal1.GetCustomer(tempPack.ReceiverId);
                tempDro.Battery -= BatteryUsage(DistanceBetweenCustomers(idal1.GetCustomer(tempPack.SenderId), tempCus), (int)tempPack.Weight + 1);//calculates the battery usage in delivery according to the weight of the package
                tempDro.Loc.Longitude = tempCus.Longitude;
                tempDro.Loc.Latitude = tempCus.Latitude;
                tempDro.Status = DroneStatuses.Available;
                idal1.ParcelDelivered(tempPack.Id);
        }
       
        public ParcelInTransfer GetParcelInTransfer(int parcelId)
        {
            Parcel parcel = new();
            ParcelInTransfer parcelInTrans = new();
            parcel = GetParcel(parcelId);
            parcel.CopyProperties(parcelInTrans);
            parcelInTrans.DeliverdTo = GetCustomer(parcel.Receiver.Id).Loc;
            parcelInTrans.PickedUp = GetCustomer(parcel.Sender.Id).Loc;
            parcelInTrans.Distance = Bonus.Haversine(parcelInTrans.DeliverdTo.Longitude, parcelInTrans.DeliverdTo.Latitude, parcelInTrans.PickedUp.Longitude, parcelInTrans.PickedUp.Latitude);
            parcelInTrans.Sender = parcel.Sender;
            parcelInTrans.Receiver = parcel.Receiver;
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
        internal ParcelStatuses GetParcelStatus(int parcelId)
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
     
        public ParcelAtCustomer GetParcelAtCustomer(int parcelId)
        {
            Parcel parcel = new();
            ParcelAtCustomer parcelAtCustomer = new();
            parcel = GetParcel(parcelId);
            parcel.CopyProperties(parcelAtCustomer);
            parcelAtCustomer.ParcelStatus = GetParcelStatus(parcelId);
            if (parcel.Dr.Loc == GetCustomer(parcel.Sender.Id).Loc)// if the location is same as sender
                parcelAtCustomer.CustomerInP = parcel.Receiver;
            else
                parcelAtCustomer.CustomerInP = parcel.Sender;
            return parcelAtCustomer;
        }
       
        public Parcel GetParcel(int parcelId)
        {
            try
            {
                Parcel parcel = new();
                IDAL.DO.Parcel parcelDal = idal1.GetParcel(parcelId);
                parcelDal.CopyProperties(parcel);
                parcel.Sender = GetCustomerInParcel(parcelDal.SenderId);
                parcel.Receiver = GetCustomerInParcel(parcelDal.ReceiverId);
                //idal1.GetCustomer(parcelDal.Sender).CopyProperties(parcel.Sender);// converts to CustomerInParcel
                //idal1.GetCustomer(parcelDal.Receiver).CopyProperties(parcel.Receiver);
                parcel.Dr = new();
                if (parcelDal.DroneId != 0)
                {
                    Drone dr = GetDrone(parcelDal.DroneId);
                    dr.CopyProperties(parcel.Dr);
                    parcel.Dr.Loc = new();
                    dr.Loc.CopyProperties(parcel.Dr.Loc);
                }
                return parcel;
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Parcel.", ex);
            }
        }

     
        public IEnumerable<ParcelToList> GetAllParcels()
        {
            List<ParcelToList> tempList = new();
            //idal1.GetAllParcels().ToList().ForEach(p => p.CopyProperties(parcel)); 
            List<IDAL.DO.Parcel> tempParcelList = idal1.GetAllParcels().ToList();
            foreach (IDAL.DO.Parcel p in tempParcelList)
            {
                ParcelToList parcel = new();
                p.CopyProperties(parcel);
                parcel.parcelStatus = GetParcelStatus(p.Id);
                tempList.Add(parcel);
            
            }
            
            return tempList;
        }

     
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
