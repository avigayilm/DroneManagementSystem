using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;


namespace Dal
{
    internal sealed partial class DalObject
    {
        /// <summary>
        /// adds a parcel to the parcellist
        /// </summary>
        /// <param name="pack"></param>
        public void AddParcel(Parcel pack)
        {
            CheckDuplicateParcel(pack.Id);
            pack.Id = ++DataSource.Config.LastParcelNumber;
            DataSource.parcelList.Add(pack);
            
        }

        /// <summary>
        /// the drone picks up the parcel, therefore updating the drone's status to delivery 
        /// and updating the picked up time
        /// </summary>
        /// <param name="parcelId"></param>
        public void ParcelPickedUp(int parcelId)
        {
            int parcelIndex = CheckExistingParcel(parcelId);
            int droneIndex = DataSource.dronesList.FindIndex(d => d.Id == DataSource.parcelList[parcelIndex].DroneId);
            //var temp2 = DataSource.dronesList[droneIndex];
            // temp2.status = DroneStatuses.Delivery;
            var temp = DataSource.parcelList[parcelIndex];
            temp.PickedUpTime = DateTime.Now;
            //DataSource.dronesList[droneIndex] = temp2;
            DataSource.parcelList[parcelIndex] = temp;
        }


        /// <summary>
        ///  funciton updated the parcel to delivered. It changes the drone's status to available
        ///  and updates the time of requested
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="day"></param>
        public void ParcelDelivered(int parcelId)//when the parcel is delivered, the drone will be available again
        {
            int parcelIndex = CheckExistingParcel(parcelId);
            int droneIndex = DataSource.dronesList.FindIndex(d => d.Id == DataSource.parcelList[parcelIndex].DroneId);
            var temp2 = DataSource.dronesList[droneIndex];
            var temp = DataSource.parcelList[parcelIndex];
            //temp2.status = DroneStatuses.Available;
            temp.Created = DateTime.Now;
            DataSource.dronesList[droneIndex] = temp2;
            DataSource.parcelList[parcelIndex] = temp;
        }

        /// <summary>
        /// assigning a drone to a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void ParcelDrone(int parcelId, int droneId)
        {
            int parcelIndex = CheckExistingParcel(parcelId);
            var temp = DataSource.parcelList[parcelIndex];
            temp.DroneId = droneId;
            temp.Created = DateTime.Now;
            DataSource.parcelList[parcelIndex] = temp;

        }

        /// <summary>
        /// The get functions return a string with all the information of the lists
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId)
        {
            int index = CheckExistingParcel(parcelId);
            return DataSource.parcelList[index];
        }
        /// <summary>
        /// returns a parcellist
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        { 
            return DataSource.parcelList.FindAll(x => predicate == null ? true : predicate(x));
        }

        

        /// <summary>
        /// checks if a Parcel exists in the Parcellist, if it doesn't it throws a MissingIdException
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns>index of the parcel in the list</returns>
        int CheckExistingParcel(int parcelId)
        {
            int index = DataSource.parcelList.FindIndex(p => p.Id == parcelId);
            if (index == -1)
            {
                throw new MissingIdException($"No parcel with { parcelId } id exists\n");
            }
            return index;
        }

        /// <summary>
        /// checks if a station already exists, if it does it throws a duplicateIdException
        /// </summary>
        /// <param name="stationId"></param>
        /// <exception cref="IDAL.DO.DuplicateIdException"></exception>
        void CheckDuplicateParcel(int parcelId)
        {
            if (DataSource.parcelList.Exists(s => s.Id == parcelId))
            {
                throw new DuplicateIdException($"Parcel with { parcelId } id already exists\n");
            }
        }
    }
}