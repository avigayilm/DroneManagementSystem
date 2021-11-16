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
        /// adds a parcel to the parcellist
        /// </summary>
        /// <param name="pack"></param>
        public void AddParcel(Parcel pack)
        {
            if (DataSource.parcelList.Exists(p => p.Id == pack.Id))
                throw new DuplicateIdException($"Parcel with {pack.Id} id already exists\n");
            else
            {
                DataSource.parcelList.Add(pack);
                pack.Id = ++DataSource.Config.LastParcelNumber;
            }

        }

        /// <summary>
        /// the drone picks up the parcel, therefore updating the drone's status to delivery 
        /// and updating the picked up time
        /// </summary>
        /// <param name="parcelId"></param>
        public void ParcelPickedUp(int parcelId)
        {
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new MissingIdException("No such parcel exists in list\n");
            }
            else// check if it's a valid index
            {
                int droneIndex = DataSource.dronesList.FindIndex(d => d.Id == DataSource.parcelList[parcelIndex].DroneId);
                var temp2 = DataSource.dronesList[droneIndex];
                // temp2.status = DroneStatuses.Delivery;
                var temp = DataSource.parcelList[parcelIndex];
                temp.PickedUp = DateTime.Now;
                DataSource.dronesList[droneIndex] = temp2;
                DataSource.parcelList[parcelIndex] = temp;

            }
        }


        /// <summary>
        ///  funciton updated the parcel to delivered. It changes the drone's status to available
        ///  and updates the time of requested
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="day"></param>
        public void ParcelDelivered(int parcelId, DateTime day)//when the parcel is delivered, the drone will be available again
        {
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.Id == parcelId);
            if (parcelIndex == -1)
                throw new MissingIdException("No such parcel exists in list\n");
            else
            {
                int droneIndex = DataSource.dronesList.FindIndex(d => d.Id == DataSource.parcelList[parcelIndex].DroneId);
                var temp2 = DataSource.dronesList[droneIndex];
                var temp = DataSource.parcelList[parcelIndex];
                //temp2.status = DroneStatuses.Available;
                temp.Requested = DateTime.Now;
                DataSource.dronesList[droneIndex] = temp2;
                DataSource.parcelList[parcelIndex] = temp;
            }
        }

        /// <summary>
        /// The get functions return a string with all the information of the lists
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Parcel GetParcel(int ID)
        {
            if (!DataSource.parcelList.Exists(p => p.Id == ID))
                throw new MissingIdException($"No parcel with {ID} id exists\n");
            else
            {
                int index = DataSource.parcelList.FindIndex(p => p.Id == ID);
                return DataSource.parcelList[index];
            }
        }
        /// <summary>
        /// returns a parcellist
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetAllParcels()
        {
            List<Parcel> list = new();
            DataSource.parcelList.ForEach(p => list.Add(p));
            return (IEnumerable<Parcel>)list;
        }

        // returns a list with parcels that have not been assigned to a drone
        public IEnumerable<Parcel> GetvacantParcel()
        {
            List<Parcel> temp = new();
            DataSource.parcelList.ForEach(p => { if (p.DroneId == 0) temp.Add(p); });
            return (IEnumerable<Parcel>)temp;
        }

        /// <summary>
        /// returns a new list with the undelivered parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> UndeliveredParcels()
        {
            List<Parcel> undelivered = new();
            DataSource.parcelList.ForEach(p => { if (p.Delivered == null) undelivered.Add(p); });// if the parcel is not delivered add it to the list
            return undelivered;
        }
    }
}