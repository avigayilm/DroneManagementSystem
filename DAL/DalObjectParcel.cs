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
        public void AddParcel(Parcel pack)
        {
            int index = DataSource.parcelList.FindIndex(p => p.id == pack.id);
            if (index != -1)
            {
                throw new ParcelException("Id exists already\n");
            }
            else
            {
                DataSource.parcelList.Add(pack);
                pack.id = ++DataSource.Config.LastParcelNumber;
            }

        }

        /// <summary>
        /// the drone picks up the parcel, therefore updating the drone's status to delivery 
        /// and updating the picked up time
        /// </summary>
        /// <param name="parcelId"></param>
        public void ParcelPickedUp(int parcelId)
        {
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.id == parcelId);
            if (parcelIndex == -1)
            {
                throw new ParcelException("Id not found\n");
            }
            else// check if it's a valid index
            {
                int droneIndex = DataSource.dronesList.FindIndex(d => d.id == DataSource.parcelList[parcelIndex].droneId);
                var temp2 = DataSource.dronesList[droneIndex];
                temp2.status = DroneStatuses.Delivery;
                var temp = DataSource.parcelList[parcelIndex];
                temp.pickedUp = DateTime.Now;
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
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.id == parcelId);// finding the index of the parcel
            if (parcelIndex == -1)
            {
                throw new ParcelException("Id not found\n");
            }
            else
            {
                int droneIndex = DataSource.dronesList.FindIndex(d => d.id == DataSource.parcelList[parcelIndex].droneId);
                var temp2 = DataSource.dronesList[droneIndex];
                var temp = DataSource.parcelList[parcelIndex];
                temp2.status = DroneStatuses.Available;
                temp.requested = DateTime.Now;
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
            int index = DataSource.parcelList.FindIndex(p => p.id == ID);
            if (index == -1)
            {
                throw new ParcelException("Id not found\n");
            }
            else
            {
                return DataSource.parcelList[index];
            }
        }

        public IEnumerable<Parcel> GetParcelList()
        {
            List<Parcel> list = new();
            DataSource.parcelList.ForEach(p => list.Add(p));
            return (IEnumerable<Parcel>)list;
        }

        // returns a list with parcels that have not been assigned to a drone
        public IEnumerable<Parcel> GetvacantParcel()
        {
            List<Parcel> temp = new();
            DataSource.parcelList.ForEach(p => { if (p.droneId == 0) temp.Add(p); });
            return (IEnumerable<Parcel>)temp;
        }

        /// <summary>
        /// returns a new list with the undelivered parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> UndeliveredParcels()
        {
            List<Parcel> undelivered = new();
            DataSource.parcelList.ForEach(p => { if (p.delivered == DateTime.MinValue) undelivered.Add(p); });// if he parcel is not delivered add it to the list
            return undelivered;
            //put the drone state in delivery

        }
    }