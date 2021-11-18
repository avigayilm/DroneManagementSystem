using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace IBL
{
    public interface Ibl
    {
        void AddStation(Station tempStat);
        void AddCustomer(Customer newCustomer);
        void AddDrone(Drone newDrone, int stationId);
        void AddParcel(Parcel newParcel);
        void updateDrone(int droneId, string model);
        public ParcelInTransfer GetParcelInTransfer(int parcelId);
    }
}
