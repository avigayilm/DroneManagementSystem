using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal
{
    internal sealed partial class DalXml
    {
        //public int AddParcel(Parcel pack)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ParcelDrone(int parcelId, int droneId)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ParcelPickedUp(int parcelId)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ParcelDelivered(int parcelId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Parcel GetParcel(int ID)
        //{
        //    throw new NotImplementedException();
        //}


        //public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateParcel(int parcelId, string recId)
        //{
        //    throw new NotImplementedException();
        //}

        //public void DeleteParcel(int parcelId)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// adds a parcel to the parcellist
        /// </summary>
        /// <param name="pack"></param>
        public int AddParcel(Parcel pack)
        {
            //CheckDuplicateParcel(pack.Id);
            //pack.Id = ++DataSource.Config.LastParcelNumber;
            //DataSource.parcelList.Add(pack);
            //return pack.Id;

            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            if (parcels.Exists(p => p.Id == pack.Id))
                throw new DuplicateIdException("Customer already exists\n");
            pack.Id = ++DataSource.Config.LastParcelNumber;
            parcels.Add(pack);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
            return pack.Id;
        }

        public void UpdateParcel(int parcelId, string recId)
        {
            //int index = CheckExistingParcel(parcelId);
            //Parcel tempParcel = DataSource.parcelList[index];
            //tempParcel.ReceiverId = recId;
            //DataSource.parcelList[index] = tempParcel;
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int index = parcels.FindIndex(p => p.Id == parcelId);
            if (index == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            Parcel tempParcel = parcels[index];
            tempParcel.ReceiverId = recId;
            parcels[index] = tempParcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);

        }

        /// <summary>
        /// the drone picks up the parcel, therefore updating the drone's status to delivery 
        /// and updating the picked up time
        /// </summary>
        /// <param name="parcelId"></param>
        public void ParcelPickedUp(int parcelId)
        {
            parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int pIndex = parcels.FindIndex(p => p.Id == parcelId);
            if (pIndex == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            var parcelTemp = parcels[pIndex];
            //temp2.status = DroneStatuses.Available;
            parcelTemp.PickedUp = DateTime.Now;
            // DataSource.[droneIndex] = temp2;
            parcels[pIndex] = parcelTemp;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }


        /// <summary>
        ///  funciton updated the parcel to delivered. It changes the drone's status to available
        ///  and updates the time of requested
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="day"></param>
        public void ParcelDelivered(int parcelId)//when the parcel is delivered, the drone will be available again
        {
            parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int pIndex = parcels.FindIndex(p => p.Id == parcelId);
            if (pIndex == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            //int dIndex = drones.FindIndex(d => d.Id == parcels[pIndex].DroneId);
            // var droneTemp = drones[dIndex];
            var parcelTemp =  parcels[pIndex];
            //temp2.status = DroneStatuses.Available;
            parcelTemp.Created = DateTime.Now;
            // DataSource.[droneIndex] = temp2;
            parcels[pIndex] = parcelTemp;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        /// <summary>
        /// assigning a drone to a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void ParcelDrone(int parcelId, int droneId)
        {
            parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int index = parcels.FindIndex(p => p.Id == parcelId);
            if (index == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            Parcel temp = parcels[index];
            temp.DroneId = droneId;
            temp.Created = DateTime.Now;
            parcels[index] = temp;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        /// <summary>
        /// The get functions return a string with all the information of the lists
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId)
        {
 
            parcels = XMLTools. LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int index = parcels.FindIndex(p => p.Id == parcelId);
            if (index == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            Parcel tempParcel = parcels[index];
            return tempParcel;
        }
        /// <summary>
        /// returns a parcellist
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null) 
        {
            parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml); //there is usage of parcels so as to avoid multiple loading 
            return parcels.FindAll(x => predicate == null ? true : predicate(x) && !x.Delete);
        }

        /// <summary>
        /// bonus: Doesn't completely delete the parcel but it has a sign that it is deleted
        /// </summary>
        /// <param name="parcelId"></param>

        public void DeleteParcel(int parcelId)
        {
            parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int pIndex = parcels.FindIndex(p => p.Id == parcelId);
            Parcel tempParcel = parcels[pIndex];
            tempParcel.Delete = true;
            parcels[pIndex] =  tempParcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
            //if (pIndex == -1)
            //{
            //    throw new MissingIdException("No such parcel exists\n");
            //}
            //var temp = DataSource.parcelList[index];
            //temp.Delete = true;
            //DataSource.parcelList[index] = temp;
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
            if (DataSource.parcelList[index].Delete)
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

            if (DataSource.parcelList.Exists(p => p.Id == parcelId && !p.Delete))
            {
                throw new DuplicateIdException($"Parcel with { parcelId } id already exists\n");
            }
        }

    }
}
