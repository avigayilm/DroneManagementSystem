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
        public int AddParcel(Parcel pack)
        {
            throw new NotImplementedException();
        }

        public void ParcelDrone(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public void ParcelPickedUp(int parcelId)
        {
            throw new NotImplementedException();
        }

        public void ParcelDelivered(int parcelId)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int ID)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateParcel(int parcelId, string recId)
        {
            throw new NotImplementedException();
        }

        public void DeleteParcel(int parcelId)
        {
            throw new NotImplementedException();
        }

    }
}
