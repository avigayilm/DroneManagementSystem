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
        /// adding a drone to the list of the datalayer
        /// </summary>
        /// <param name="newDrone"></param>
        /// <param name="stationId"></param>
        public void AddDrone(Drone newDrone, int stationId)
        {
            try
            {
                if (newDrone.Id < 0)
                    throw new InvalidInputException("invalid Id input \n");
                if (newDrone.Weight != WeightCategories.Heavy && newDrone.Weight != WeightCategories.Light && newDrone.Weight != WeightCategories.Medium)
                    throw new InvalidInputException("Invalid weightCategory \n");
                int index=idal1.CheckExistingStation(stationId);
                newDrone.Battery = rand.Next(20, 40);
                newDrone.Status = DroneStatuses.Maintenance;
                //location of station id
                List<IDAL.DO.Station> tempStat = (List<IDAL.DO.Station>)idal1.GetAllStations();
                newDrone.Loc = new() { Longitude = tempStat[index].Longitude, Latitude = tempStat[index].Latitude };
                DroneToList newDroneToList = new();
                newDrone.CopyPropertiestoIBL(newDroneToList);
                droneBL.Add(newDroneToList);// adding a droneToList
                                      //adding the drone to the dalObject list
                IDAL.DO.Drone droneTemp = new();
                newDrone.CopyPropertiestoIDAL(droneTemp);
                idal1.AddDrone(droneTemp);// adding the drone to the dallist
            }
            catch(InvalidInputException ex)
            {
                throw new AddingException("Couldn't add the drone.\n,", ex);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new AddingException("Couldn't add the drone.\n,", ex);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new AddingException("Couldn't add the drone.\n,", ex);
            }

        }

        /// <summary>
        /// updates the drone's model
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        public void UpdateDrone(int droneId, string model)
        {
            try
            {
                idal1.UpdateDrone(droneId, model);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new UpdateIssueException("Couldn't update the drone.\n,", ex);
            }
        }

        public int GetDroneIndex(int droneId)
        {
            try
            {
                int index = idal1.CheckExistingDrone(droneId);
                return index;
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Drone.\n,", ex);
            }
        }

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

        public DroneToList getDroneToList(int droneId)
        {
            try
            {
                int index = idal1.CheckExistingDrone(droneId);
                return droneBL[index];
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Drone.\n,", ex);
            }
        }

        public Drone GetDrone(int droneId)
        {
            DroneToList droneToList = getDroneToList(droneId);
            Drone drone = new();
            droneToList.CopyPropertiestoIBL(drone);
            if (droneToList.ParcelId == 0)// if the drone doesn't hold a parcel
                drone.ParcelInTrans = null;
            else
                drone.ParcelInTrans = GetParcelInTransfer(droneToList.ParcelId);
            return drone;

        }


        public IEnumerable<DroneToList> GetAllDrones()
        {
            return droneBL;
        }


    }
}
