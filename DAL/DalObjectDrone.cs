using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;


namespace DAL
{
    public partial class DalObject
    {
        public void AddDrone(Drone dro)// check if the drone lready exists
        {
            int index = DataSource.parcelList.FindIndex(d => d.id == dro.id);
            if (index != -1)
            {
                throw new MissingIdException("Id exists already\n");
            }
            else
            {
                DataSource.dronesList.Add(dro);
            }
        }


        /// <summary>
        /// assigning a drone to a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void ParcelDrone(int parcelId, int droneId)// we initilized the parcels with empty droneid so don't we need to add a drone id
        {
            // looking for an avialable drone and setting the Id of that drone, to be the DroneId of hte parcel
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.id == parcelId);
            if (parcelIndex == -1)
            {
                throw new MissingIdException("No such parcel\n");
            }
            else
            {
                var temp = DataSource.parcelList[parcelIndex];
                temp.droneId = droneId;
                temp.requested = DateTime.Now;
                DataSource.parcelList[parcelIndex] = temp;
            }
        }

        /// <summary>
        /// function that changes the status of the drone according to the given parameter.
        /// </summary>
        /// <param name="DroneId"></param>
        /// <param name="st"></param>
        public void ChangeDroneStatus(int DroneId, DroneStatuses st)
        {
            int droneIndex = DataSource.dronesList.FindIndex(d => d.id == DroneId);
            if (droneIndex == -1)
                throw new MissingIdException("No such drone\n");
            else
            {
                var temp = DataSource.dronesList[droneIndex];
                temp.status = st;
                DataSource.dronesList[droneIndex] = temp;
            }
        }


        public Drone GetDrone(int ID)
        {
            int index = DataSource.dronesList.FindIndex(d => d.id == ID);
            if (index == -1)
            {
                throw new MissingIdException("No such drone\n");
            }
            else
            {
                return DataSource.dronesList[index];
            }

            // return DataSource.dronesList.Find(d => d.id == ID);
        }

        public IEnumerable<Drone> GetAllDrones()
        {
            List<Drone> list = new();
            DataSource.dronesList.ForEach(d => list.Add(d));
            return (IEnumerable<Drone>)list;
        }

    }
}
