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
        public void AddDrone(DroneToList newDrone, int stationId)
        {
            try
            {
                if (newDrone.Id < 0)
                    throw new InvalidInputException("invalid Id input \n");
                if (newDrone.Loc.Latitude <= -90 || newDrone.Loc.Latitude >= 90)// out of range of latitude
                    throw new InvalidInputException("The latitude is not in a existing range(between -90 and 90) \n");
                if (newDrone.Loc.Longitude <= -180 || newDrone.Loc.Longitude >= 180)// out of range of latitude
                    throw new InvalidInputException("The longitude is not in a existing range(betweeen -180 and 180)\n");
                if (newDrone.Weight != WeightCategories.Heavy && newDrone.Weight != WeightCategories.Light && newDrone.Weight != WeightCategories.Medium)
                    throw new InvalidInputException("Invalid weightCategory \n");
                if (newDrone.Status != DroneStatuses.Available && newDrone.Status != DroneStatuses.Maintenance && newDrone.Status != DroneStatuses.Delivery)
                    throw new InvalidInputException("Invalid status \n");
                newDrone.Battery = rand.Next(20, 40);
                newDrone.Status = DroneStatuses.Maintenance;
                //location of station id
                List<IDAL.DO.Station> tempStat = (List<IDAL.DO.Station>)idal1.GetAllStations();
                int index = tempStat.FindIndex(d => d.Id == stationId);
                newDrone.Loc = new() { Longitude = tempStat[index].Longitude, Latitude = tempStat[index].Latitude };
                droneBL.Add(newDrone);// adding a droneToList
                                      //adding the drone to the dalObject list
                IDAL.DO.Drone droneTemp = new();
                newDrone.CopyPropertiestoIDAL(droneTemp);
                idal1.AddDrone(droneTemp);
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new DuplicateIdException("The Drone already exists.\n,", ex);
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
                throw new MissingIdException("Invalid ID.\n,", ex);
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
    }
}
