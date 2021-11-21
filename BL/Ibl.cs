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
        public void AddStation(Station tempStat);
        public void AddCustomer(Customer newCustomer);
        public void AddDrone(Drone newDrone, int stationId);
        public void AddParcel(Parcel newParcel);
        public void UpdateDrone(int droneId, string model);
        public ParcelInTransfer GetParcelInTransfer(int parcelId);
        public void UpdateStation(int stationId, string name, int chargingSlots);
        public void UpdateCustomer(int customerId, string name, string phone);
      
    }
}
