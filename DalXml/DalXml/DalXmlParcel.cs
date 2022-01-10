using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using DalApi;

using DO;

namespace Dal
{
    internal sealed partial class DalXml
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel(Parcel pack)
        {
            //CheckDuplicateParcel(pack.Id);
            //pack.Id = ++DataSource.Config.LastParcelNumber;
            //DataSource.parcelList.Add(pack);
            //return pack.Id;
            loadingToList(ref parcels, ParcelXml);

            //parcels =  XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            if (parcels.Exists(p => p.Id == pack.Id && !p.Delete))
                throw new DuplicateIdException("Parcel already exists\n");
            XElement serialNum = XElement.Load(@"config.xml");
            pack.Id = 1 + int.Parse(serialNum.Element("runNum").Value);
            serialNum.Element("runNum").Value = pack.Id.ToString();
            parcels.Add(pack);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
            XMLTools.SaveListToXMLElement(serialNum, "config.xml");
            return pack.Id;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(int parcelId, string recId)
        {
            //int index = CheckExistingParcel(parcelId);
            //Parcel tempParcel = DataSource.parcelList[index];
            //tempParcel.ReceiverId = recId;
            //DataSource.parcelList[index] = tempParcel;
            //List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            loadingToList(ref parcels, ParcelXml);
            int index = parcels.FindIndex(p => p.Id == parcelId);
            if (index == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            if (parcels[index].Delete)
            {
                throw new MissingIdException($"This Parcel:{ parcelId } is deleted \n");
            }
            Parcel tempParcel = parcels[index];
            tempParcel.ReceiverId = recId;
            parcels[index] = tempParcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ParcelPickedUp(int parcelId)
        {
            //parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            loadingToList(ref parcels, ParcelXml);
            int pIndex = parcels.FindIndex(p => p.Id == parcelId);
            if (pIndex == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            if (parcels[pIndex].Delete)
            {
                throw new MissingIdException($"This Parcel:{ parcelId } is deleted \n");
            }
            var parcelTemp = parcels[pIndex];
            //temp2.status = DroneStatuses.Available;
            parcelTemp.PickedUp = DateTime.Now;
            // DataSource.[droneIndex] = temp2;
            parcels[pIndex] = parcelTemp;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ParcelDelivered(int parcelId)//when the parcel is delivered, the drone will be available again
        {
            //parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            loadingToList(ref parcels, ParcelXml);
            int pIndex = parcels.FindIndex(p => p.Id == parcelId);
            if (pIndex == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            if (parcels[pIndex].Delete)
            {
                throw new MissingIdException($"This Parcel:{ parcelId } is deleted \n");
            }
            //int dIndex = drones.FindIndex(d => d.Id == parcels[pIndex].DroneId);
            // var droneTemp = drones[dIndex];
            var parcelTemp =  parcels[pIndex];
            //temp2.status = DroneStatuses.Available;
            parcelTemp.Delivered = DateTime.Now;
            // DataSource.[droneIndex] = temp2;
            parcels[pIndex] = parcelTemp;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ParcelDrone(int parcelId, int droneId)
        {
            //parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            loadingToList(ref parcels, ParcelXml);
            int index = parcels.FindIndex(p => p.Id == parcelId);
            if (index == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            if (parcels[index].Delete)
            {
                throw new MissingIdException($"This Parcel:{ parcelId } is deleted \n");
            }
            Parcel temp = parcels[index];
            temp.DroneId = droneId;
            temp.Created = DateTime.Now;
            parcels[index] = temp;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int parcelId)
        {

            //parcels = XMLTools. LoadListFromXMLSerializer<Parcel>(ParcelXml);
            loadingToList(ref parcels, ParcelXml);
            int index = parcels.FindIndex(p => p.Id == parcelId);
            if (index == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            if (parcels[index].Delete)
            {
                throw new MissingIdException($"This Parcel:{ parcelId } is deleted \n");
            }
            Parcel tempParcel = parcels[index];
            return tempParcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null) 
        {
            //parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml); //there is usage of parcels so as to avoid multiple loading 
            loadingToList(ref parcels, ParcelXml);
            //return parcels.FindAll(x => predicate == null ? true : predicate(x) && !x.Delete);
            return parcels
                .Where(p => predicate == null ? true : predicate(p) && !p.Delete);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int parcelId)
        {
            //parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            loadingToList(ref parcels, ParcelXml);
            int pIndex = parcels.FindIndex(p => p.Id == parcelId);
            if (pIndex == -1)
            {
                throw new MissingIdException("No such parcel exists\n");
            }
            if (parcels[pIndex].Delete)
            {
                throw new MissingIdException($"This Parcel:{ parcelId } is deleted \n");
            }
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
            int index = parcels.FindIndex(p => p.Id == parcelId);
            if (index == -1)
            {
                throw new MissingIdException($"No parcel with { parcelId } id exists\n");
            }
            if (parcels[index].Delete)
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

            if (parcels.Exists(p => p.Id == parcelId && !p.Delete))
            {
                throw new DuplicateIdException($"Parcel with { parcelId } id already exists\n");
            }
        }

    }
}
