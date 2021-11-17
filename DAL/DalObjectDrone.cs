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

        /// <summary>
        /// adds a drone to the dronlist
        /// </summary>
        /// <param name="dro"></param>
        public void AddDrone(Drone dro)// check if the drone lready exists
        {
            // int index = DataSource.parcelList.FindIndex(d => d.id == dro.id);
            bool exists = DataSource.dronesList.Exists(d => d.Id == dro.Id);
            if (exists)
            {
                throw new DuplicateIdException("Id exists already\n");
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
        public void ParcelDrone(int parcelId, int droneId)
        {
            // looking for an avialable drone and setting the Id of that drone, to be the DroneId of the parcel
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new MissingIdException("No such parcel\n");
            }
            else
            {
                var temp = DataSource.parcelList[parcelIndex];
                temp.DroneId = droneId;
                temp.Requested = DateTime.Now;
                DataSource.parcelList[parcelIndex] = temp;
            }
        }

        ///// <summary>
        ///// function that changes the status of the drone according to the given parameter.
        ///// </summary>
        ///// <param name="DroneId"></param>
        ///// <param name="st"></param>
        //public void ChangeDroneStatus(int DroneId, DroneStatuses st)
        //{
        //    int droneIndex = DataSource.dronesList.FindIndex(d => d.id == DroneId);
        //    if (droneIndex == -1)
        //        throw new MissingIdException("No such drone\n");
        //    else
        //    {
        //        var temp = DataSource.dronesList[droneIndex];
        //        temp.status = st;
        //        DataSource.dronesList[droneIndex] = temp;
        //    }
        //}

        /// <summary>
        /// returns the drone according to the given Id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Drone GetDrone(int ID)
        {
            int index = DataSource.dronesList.FindIndex(d => d.Id == ID);
            if (index == -1)
            {
                throw new MissingIdException("No such drone\n");
            }
            else
            {
                return DataSource.dronesList[index];
            }
        }

        /// <summary>
        /// returns the list of all drones as Ienumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetAllDrones()
        {
            List<Drone> list = new();
            DataSource.dronesList.ForEach(d => list.Add(d));
            return (IEnumerable<Drone>)list;
        }
        /// <summary>
        /// updates the model of the drone, used in BL
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        public void UpdateDrone(int droneId,string model)
        {
            int index = DataSource.dronesList.FindIndex(d => d.Id ==droneId );
            if (index == -1)
            {
                throw new MissingIdException("No such drone\n");
            }
            else
            {
                Drone tempDrone=DataSource.dronesList[index];
                tempDrone.Model = model;
                DataSource.dronesList[index] = tempDrone;
            }
        }

        public IEnumerable<Drone> DronesChargingAtStation(int stationId)
        {
            List<Drone> charging = new();
            DataSource.chargeList.ForEach(c => { if (c.StationId == stationId) charging.Add(GetDrone(c.DroneId));});
            return charging;
        }

    }
}
