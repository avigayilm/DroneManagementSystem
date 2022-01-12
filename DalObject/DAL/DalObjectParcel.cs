using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;
using DalApi;


namespace Dal
{
    internal sealed partial class DalObject
    {

        public int AddParcel(Parcel pack)
        {
            CheckDuplicateParcel(pack.Id);
            pack.Id = ++DataSource.Config.LastParcelNumber;
            DataSource.parcelList.Add(pack);
            return pack.Id;
        }

        public void UpdateParcel(int parcelId, string recId)
        {
            int index = CheckExistingParcel(parcelId);
            if (DataSource.parcelList[index].Delete) 
            {  
                throw new MissingIdException($"This Parcel:{ parcelId } is deleted \n");  
            }
            Parcel tempParcel = DataSource.parcelList[index];
            tempParcel.ReceiverId = recId;
            DataSource.parcelList[index] = tempParcel;
        }

   
        public void ParcelPickedUp(int parcelId)
        {
            int parcelIndex = CheckExistingParcel(parcelId);
            int droneIndex = DataSource.dronesList.FindIndex(d => d.Id == DataSource.parcelList[parcelIndex].DroneId);
            var temp = DataSource.parcelList[parcelIndex];
            temp.PickedUp = DateTime.Now;
            DataSource.parcelList[parcelIndex] = temp;
        }


        public void ParcelDelivered(int parcelId)//when the parcel is delivered, the drone will be available again
        {
            int parcelIndex = CheckExistingParcel(parcelId);
            int droneIndex = DataSource.dronesList.FindIndex(d => d.Id == DataSource.parcelList[parcelIndex].DroneId);
            var temp2 = DataSource.dronesList[droneIndex];
            var temp = DataSource.parcelList[parcelIndex];
            temp.Delivered = DateTime.Now;
            DataSource.dronesList[droneIndex] = temp2;
            DataSource.parcelList[parcelIndex] = temp;
        }

        public void ParcelDrone(int parcelId, int droneId)
        {
            int parcelIndex = CheckExistingParcel(parcelId);
            var temp = DataSource.parcelList[parcelIndex];
            temp.DroneId = droneId;
            temp.Created = DateTime.Now;
            DataSource.parcelList[parcelIndex] = temp;

        }


        public Parcel GetParcel(int parcelId)
        {
            int index = CheckExistingParcel(parcelId);
            return DataSource.parcelList[index];
        }

        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        { 
            return DataSource.parcelList.FindAll(x => predicate == null ? true : predicate(x)&&!x.Delete);
        }


        public void DeleteParcel(int parcelId)
        {
            int index = CheckExistingParcel(parcelId);
            var temp = DataSource.parcelList[index];
            temp.Delete = true;
            DataSource.parcelList[index] = temp;
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
            if(DataSource.parcelList[index].Delete)
            {
                throw new MissingIdException($"This Parcel:{ parcelId } is deleted \n");
            }

            return index;
        }

        /// <summary>
        /// checks if a station already exists, if it does exists it checks if it was deleted if it does it throws a duplicateIdException
        /// </summary>
        /// <param name="stationId"></param>
        /// <exception cref="IDAL.DO.DuplicateIdException"></exception>
        void CheckDuplicateParcel(int parcelId)
        {
            
            if (DataSource.parcelList.Exists(p => p.Id == parcelId && !p.Delete ))
            {
                throw new DuplicateIdException($"Parcel with { parcelId } id already exists\n");
            }
        }
    }
}