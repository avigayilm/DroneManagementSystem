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
                // List<IDAL.DO.Station> tempStat = (List<IDAL.DO.Station>)idal1.GetAllStations();
                // newDrone.Loc = new() { Longitude = tempStat[index].Longitude, Latitude = tempStat[index].Latitude };
                newDrone.Loc = new() { Longitude = GetStation(stationId).Loc.Longitude, Latitude = GetStation(stationId).Loc.Latitude };
                DroneToList newDroneToList = new();
               
                newDrone.CopyProperties(newDroneToList);
                newDroneToList.ParcelId = newDrone.ParcelInTrans.Id;
                newDroneToList.Loc = new() { Latitude = newDrone.Loc.Latitude, Longitude = newDrone.Loc.Longitude };
                droneBL.Add(newDroneToList);// adding a droneToList
                                      //adding the drone to the dalObject list
                
                IDAL.DO.Drone droneTemp = new();
                object obj1 = droneTemp;
                newDrone.CopyProperties(obj1);
                droneTemp = (IDAL.DO.Drone)obj1;
                idal1.AddDrone(droneTemp);// adding the drone to the dallist
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
                DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
                if (tempDron == default)
                    throw new RetrievalException("Couldn't get the Drone.\n,");
                else
                    tempDron.Model = model;
                

            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new UpdateIssueException("Couldn't update the drone.\n,", ex);
            }
        }

        /// <summary>
        /// returns the location of the drone
        /// </summary>
        /// <param name="p"></param>
        /// <param name="tempBl"></param>
        /// <returns></returns>
        internal Location DroneLocation(IDAL.DO.Parcel p, DroneToList tempBl)
        {
            Location locTemp = new();
            if (p.Created != null && p.PickedUp == null)//if assigned but not yet collected
            {
                //location of deone will be in  station closed to the sender
                var stationTemp = idal1.SmallestDistanceStation(p.SenderId);
                locTemp.Longitude = stationTemp.Longitude;
                locTemp.Latitude = stationTemp.Latitude;
            }
            if (p.Created != null && p.PickedUp != null)// if package is picked up
            {
                //location wIll be at the sender
                List<IDAL.DO.Customer> tempCustomerList = (List<IDAL.DO.Customer>)idal1.GetAllCustomers();
                int customerIndex = tempCustomerList.FindIndex(c => c.Id == p.SenderId);
                if (customerIndex != -1)
                {
                    locTemp.Longitude = tempCustomerList[customerIndex].Longitude;
                    locTemp.Latitude = tempCustomerList[customerIndex].Latitude;
                }

            }
            return locTemp;
        }


      
        public Drone GetDrone(int droneId)
        {
                DroneToList droneToList = getDroneToList(droneId);
                Drone drone = new();
                droneToList.CopyProperties(drone);
                drone.Loc = new Location();
                droneToList.Loc.CopyProperties(drone.Loc);
                if (droneToList.ParcelId == 0)// if the drone doesn't hold a parcel
                    drone.ParcelInTrans = null;
                else
                    drone.ParcelInTrans = GetParcelInTransfer(droneToList.ParcelId);
            //droneToList.Loc = drone.Loc;
                return drone;
           
        }

       
        public DroneToList getDroneToList(int droneId)
        {
                DroneToList tempDron = droneBL.FirstOrDefault(d => d.Id == droneId);
                if(tempDron == default)
                    throw new RetrievalException("Couldn't get the Drone.\n,");
                return tempDron;
        }

        public IEnumerable<DroneToList> GetAllDrones()
        {
            return droneBL;
        }




    }
}
