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

        public void AddDrone(BO.Drone newDrone, int stationId)
        {
            try
            {
                if (newDrone.Id < 0)
                    throw new InvalidInputException("invalid Id input");
                if (newDrone.Weight != BO.WeightCategories.Heavy && newDrone.Weight != BO.WeightCategories.Light && newDrone.Weight != BO.WeightCategories.Medium)
                    throw new InvalidInputException("Invalid weightCategory");

                int index = idal1.CheckExistingStation(stationId);
                BO.Station tempSt = GetStation(stationId);
                newDrone.Battery = rand.Next(20, 40);
                newDrone.Status = DroneStatuses.Maintenance;
                newDrone.Loc = new() { Longitude = tempSt.Loc.Longitude, Latitude = tempSt.Loc.Latitude };
                DroneToList newDroneToList = new();

                newDrone.CopyProperties(newDroneToList);
                newDroneToList.Loc = new() { Latitude = newDrone.Loc.Latitude, Longitude = newDrone.Loc.Longitude };
                droneBL.Add(newDroneToList);// adding a droneToList
                                            //adding the drone to the dalObject list
                DO.Drone droneTemp = new();
                object obj1 = droneTemp;
                newDrone.CopyProperties(obj1);
                droneTemp = (DO.Drone)obj1;
                idal1.AddDrone(droneTemp);// adding the drone to the dallist
                idal1.SendToCharge(newDrone.Id, stationId);//sending the drone to charge
            }

            catch (DO.MissingIdException ex)
            {
                throw new AddingException("Couldn't add the drone.", ex);
            }
            catch (DO.DuplicateIdException ex)
            {
                throw new AddingException("Couldn't add the drone.", ex);
            }
        }

        public void UpdateDrone(int droneId, string model)
        {
            try
            {
                idal1.UpdateDrone(droneId, model);
                DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
                if (tempDron == default)
                    throw new RetrievalException("Couldn't get the Drone.");
                else
                    tempDron.Model = model;
            }
            catch (DO.MissingIdException ex)
            {
                throw new UpdateIssueException("Couldn't update the drone.", ex);
            }
        }

        /// <summary>
        /// returns the location of the drone
        /// </summary>
        /// <param name="p"></param>
        /// <param name="tempBl"></param>
        /// <returns></returns>
        internal Location DroneLocation(DO.Parcel p, DroneToList tempBl)
        {
            Location locTemp = new();
            if (p.Created != null && p.PickedUpTime == null)//if assigned but not yet collected
            {
                //location of deone will be in  station closed to the sender
                var stationTemp = idal1.SmallestDistanceStation(p.SenderId);
                locTemp.Longitude = stationTemp.Longitude;
                locTemp.Latitude = stationTemp.Latitude;
            }
            if (p.Created != null && p.PickedUpTime != null)// if package is picked up
            {
                //location wIll be at the sender
                List<DO.Customer> tempCustomerList = (List<DO.Customer>)idal1.GetAllCustomers();
                int customerIndex = tempCustomerList.FindIndex(c => c.Id == p.SenderId);
                if (customerIndex != -1)
                {
                    locTemp.Longitude = tempCustomerList[customerIndex].Longitude;
                    locTemp.Latitude = tempCustomerList[customerIndex].Latitude;
                }

            }
            return locTemp;
        }

        public BO.Drone GetDrone(int droneId)
        {
            DroneToList droneToList = getDroneToList(droneId);
            BO.Drone drone = new();
            droneToList.CopyProperties(drone);
            drone.Loc = new Location();
            droneToList.Loc.CopyProperties(drone.Loc);
            //drone.Loc = new() { Longitude = droneToList.Loc.Longitude, Latitude = droneToList.Loc.Latitude };
            if (droneToList.ParcelId == 0)// if the drone doesn't hold a parcel
                drone.ParcelInTrans = null;
            else
                drone.ParcelInTrans = GetParcelInTransfer(droneToList.ParcelId);
            return drone;
        }

        public DroneToList getDroneToList(int droneId)
        {
            DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
            if (tempDron == default)
                throw new RetrievalException("Couldn't get the Drone.");
            return tempDron;
        }

        public IEnumerable<DroneToList> GetAllDrones(Predicate<DroneToList> predicate = null)
        {
            return droneBL.FindAll(d => predicate == null ? true : predicate(d));

        }




    }
}
