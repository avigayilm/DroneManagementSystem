using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DO;
using DalApi;


namespace BL
{
    public partial class BL
    {
        public int AddParcel(BO.Parcel newParcel)// do I have to check customer and receiver
        {
            try
            {
                //if (newParcel.Id < 0)
                //    throw new InvalidInputException("invalid Id input");
                if (newParcel.Priority != BO.Priorities.Emergency && newParcel.Priority != BO.Priorities.Fast && newParcel.Priority != BO.Priorities.Normal)
                    throw new InvalidInputException("Invalid priority");
                if (newParcel.Weight != BO.WeightCategories.Heavy && newParcel.Weight != BO.WeightCategories.Light && newParcel.Weight != BO.WeightCategories.Medium)
                    throw new InvalidInputException("Invalid weightCategory");
                if (string.IsNullOrEmpty(newParcel.Sender.Id))
                    throw new InvalidInputException("invalid inputId");
                if (string.IsNullOrEmpty(newParcel.Receiver.Id))
                    throw new InvalidInputException("invalid inputId");
                newParcel.Delivered = null;
                newParcel.Assigned = null;
                newParcel.PickedUp = null;
                newParcel.Dr = null;
                DO.Parcel parcelTemp = new DO.Parcel();
                object obj1 = parcelTemp;
                newParcel.CopyProperties(obj1);
                parcelTemp = (DO.Parcel)obj1;
                parcelTemp.SenderId = newParcel.Sender.Id;// only insert if sender exists?
                parcelTemp.ReceiverId = newParcel.Receiver.Id;
                return idal1.AddParcel(parcelTemp);// can we print the parcelId here
            }
            catch (DO.DuplicateIdException ex)
            {
                throw new AddingException("Couldn't add the parcel.", ex);
            }
        }

        public void AssignParcelToDrone(int droneId)
        {
            try
            {
                DroneToList drone = droneBL.FirstOrDefault(d => d.Id == droneId);
                var tempParcelList = idal1.GetAllParcels(p => p.Assigned == null && (int)p.Weight <= (int)drone.Weight);//.OrderBy(i => (int)i.Priority).ThenBy(i => (int)i.Weight).ThenByDescending(i => DroneDistanceFromParcel(drone, i)); //data layer parcel list
                int maxW = 0, maxPri = 0;
                double minDis = 0.0;
                if (drone.Status == DroneStatuses.Available)
                {
                    DO.Parcel? fittingPack = null; // new null temp parcel
                    foreach (DO.Parcel pack in tempParcelList)
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
                            {
                                fittingPack = pack;//so far this is the most fitting pack for the drone.
                                maxPri = (int)pack.Priority;
                                maxW = (int)pack.Weight;
                                minDis = droneDisFromPack;
                            }
                        }
                    }
                    if (fittingPack != null) // if indeed a fitting package has been found
                    {
                        drone.Status = DroneStatuses.Delivery; // update drone list in BL
                        drone.ParcelId = ((DO.Parcel)fittingPack).Id;
                        idal1.ParcelDrone(fittingPack.GetValueOrDefault().Id, droneId); // update parcel in IDAL
                    }
                    else
                        throw new BatteryIssueException("Drone hasn't got enough battery to carry any parcel");
                }
                else
                    throw new UpdateIssueException("Couldn't assign the parcel");
            }
            catch (DO.MissingIdException ex)
            {
                throw new UpdateIssueException("Couldn't assign", ex);
            }
        }
        

        public void CollectingParcelByDrone(int droneId)
        {
            try
            {
                DroneToList drone = droneBL.FirstOrDefault(d => d.Id == droneId);
                DO.Parcel tempPack = idal1.GetParcel(drone.ParcelId); //retrieve parcel assigned
                // checks that the drone is indeed ready to deliver it parcel
                if (!(drone.Status == DroneStatuses.Delivery && tempPack.PickedUp == null))
                    throw new DeliveryIssueException("Parcel cannot be picked up by drone\n");
                drone.Battery -= BatteryUsage(DroneDistanceFromParcel(drone, tempPack), 0);
                DO.Customer tempCus = idal1.GetCustomer(tempPack.SenderId);
                Location tempLoc = new Location()
                {
                    Longitude = tempCus.Longitude,
                    Latitude = tempCus.Latitude
                };
                drone.Loc.Longitude = tempCus.Longitude;
                idal1.ParcelPickedUp(tempPack.Id);//update parcel to picked up in idal
            }
            catch (DO.MissingIdException ex)
            {
                throw new RetrievalException("drone has no parcel");
            }
        }

        public void DeliverParcelByDrone(int droneId)
        {
            try
            {
                DroneToList tempDro = droneBL.FirstOrDefault(d => d.Id == droneId);
                if (tempDro == default)
                    throw new RetrievalException("Couldn't get the Drone.\n,");
                DO.Parcel tempPack = idal1.GetParcel(tempDro.ParcelId);
                if (!(tempDro.Status == DroneStatuses.Delivery && tempPack.Delivered == null))
                    throw new DeliveryIssueException("Parcel cannot be delivered by drone\n");
                DO.Customer tempCus = idal1.GetCustomer(tempPack.ReceiverId);
                tempDro.Battery -= BatteryUsage(DistanceBetweenCustomers(idal1.GetCustomer(tempPack.SenderId), tempCus), (int)tempPack.Weight + 1);//calculates the battery usage in delivery according to the weight of the package
                tempDro.Loc.Longitude = tempCus.Longitude;
                tempDro.Loc.Latitude = tempCus.Latitude;
                tempDro.Status = DroneStatuses.Available;
                tempDro.ParcelId = 0;// unassign the parcel
                idal1.ParcelDelivered(tempPack.Id);
            }
            catch (DO.MissingIdException ex)
            {
                throw new DeliveryIssueException("Couldn't Deliver", ex);
            }
        }

        public ParcelInTransfer GetParcelInTransfer(int parcelId)
        {
            DO.Parcel parcel = new DO.Parcel();
            ParcelInTransfer parcelInTrans = new ParcelInTransfer();
            parcel = idal1.GetParcel(parcelId);
            DO.Customer tempCus = idal1.GetCustomer(parcel.SenderId);

            parcel.CopyProperties(parcelInTrans);
            parcelInTrans.Sender = GetCustomerInParcel(parcel.SenderId);
            parcelInTrans.Receiver = GetCustomerInParcel(parcel.ReceiverId);
            parcelInTrans.PickedUpFrom = new Location() {Longitude=tempCus.Longitude,Latitude= tempCus.Latitude };
            tempCus = idal1.GetCustomer(parcel.ReceiverId);
            parcelInTrans.DeliverdTo= new Location() { Longitude = tempCus.Longitude, Latitude = tempCus.Latitude };
            parcelInTrans.Distance = Bonus.Haversine(parcelInTrans.DeliverdTo.Longitude, parcelInTrans.DeliverdTo.Latitude, parcelInTrans.PickedUpFrom.Longitude, parcelInTrans.PickedUpFrom.Latitude);
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
            BO.Parcel parcel = GetParcel(parcelId);
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
        
            BO.Parcel parcel = new BO.Parcel();
            ParcelAtCustomer parcelAtCustomer = new ParcelAtCustomer();
            parcel = GetParcel(parcelId);
            parcel.CopyProperties(parcelAtCustomer);
            parcelAtCustomer.ParcelStatus = GetParcelStatus(parcelId);
            DO.Customer dalCustomer = idal1.GetCustomer(parcel.Sender.Id);
            if (parcel.Dr.Loc.Longitude == dalCustomer.Longitude && parcel.Dr.Loc.Latitude==dalCustomer.Latitude)// if the location is same as sender
                parcelAtCustomer.CustomerInP = parcel.Receiver;
            else
                parcelAtCustomer.CustomerInP = parcel.Sender;
            return parcelAtCustomer;
        }

        public BO.Parcel GetParcel(int parcelId)
        {
            try
            {
                BO.Parcel parcel = new BO.Parcel();
                DO.Parcel parcelDal = idal1.GetParcel(parcelId);
                parcelDal.CopyProperties(parcel);
                parcel.Sender = GetCustomerInParcel(parcelDal.SenderId);
                parcel.Receiver = GetCustomerInParcel(parcelDal.ReceiverId);
                //idal1.GetCustomer(parcelDal.Sender).CopyProperties(parcel.Sender);// converts to CustomerInParcel
                //idal1.GetCustomer(parcelDal.Receiver).CopyProperties(parcel.Receiver);
                parcel.Dr = new DroneInParcel();
                if (parcelDal.DroneId != 0)
                {
                    BO.Drone dr = GetDrone(parcelDal.DroneId);
                    dr.CopyProperties(parcel.Dr);
                    parcel.Dr.Loc = new Location();
                    dr.Loc.CopyProperties(parcel.Dr.Loc);
                }
                return parcel;
            }
            catch (DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Parcel.", ex);
            }
        }


        public IEnumerable<ParcelToList> GetAllParcels(Predicate<DO.Parcel> predicate = null)
        {
            List<ParcelToList> tempList = new List<ParcelToList>();
            //idal1.GetAllParcels().ToList().ForEach(p => p.CopyProperties(parcel)); 
            List<DO.Parcel> tempParcelList = idal1.GetAllParcels(predicate).ToList();
            foreach (DO.Parcel p in tempParcelList)
            {
                ParcelToList parcel = new ParcelToList();
                p.CopyProperties(parcel);
                parcel.ParcelStatus = GetParcelStatus(p.Id);
                tempList.Add(parcel);
            }
            return tempList;
        }

        public void UpdateParcel(int parcelId, string recId)
        {
            try
            {
                if(idal1.GetParcel(parcelId).PickedUp != null)
                    throw new UpdateIssueException("Couldn't update the parcel.");
                idal1.UpdateParcel(parcelId, recId);
                //DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
                //if (tempDron == default)
                //    throw new RetrievalException("Couldn't get the Drone.");
                //else
                //    tempDron.Model = model;
            }
            catch (DO.MissingIdException ex)
            {
                throw new UpdateIssueException("Couldn't update the parcel.", ex);
            }
        }


        //public IEnumerable<ParcelToList> GetAllUnassignedParcels()
        //{
        //    List<ParcelToList> tempList = new();
        //    idal1.GetAllParcels(p => p.Assigned == null).CopyPropertyListtoIBLList(tempList);
        //    foreach (ParcelToList p in tempList)
        //    {
        //        p.parcelStatus = GetParcelStatus(p.Id);
        //    }
        //    return tempList;
        //}

    }
}
