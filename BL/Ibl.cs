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
        public void ReleasingDroneFromCharge(int droneId, double chargingTime);
        public void SendingDroneToCharge(int droneId);
        public void AssignParcelToDrone(int droneId);
        public void DeliverParcelByDrone(int droneId);
        public void CollectingParcelByDrone(int droneId);
        public Customer GetCustomer(string customerId);
        public IEnumerable<CustomerToList> GetAllCustomers();
        public int GetDroneIndex(int droneId);
        internal Location DroneLocation(IDAL.DO.Parcel p, DroneToList tempBl);

        public DroneToList getDroneToList(int droneId);
        public Drone GetDrone(int droneId);
        public IEnumerable<DroneToList> GetAllDrones();
        public ParcelAtCustomer GetParcelAtCustomer(int parcelId);
        public ParcelStatuses GetParcelStatus(Parcel parcel);
        public Parcel GetParcel(int parcelId);
        public IEnumerable<ParcelToList> GetAllParcels();
        public IEnumerable<ParcelToList> GetAllUnassignedParcels();
        public Station GetStation(int stationId);
        public IEnumerable<StationToList> GetAllStation();
        public IEnumerable<StationToList> GetAllStationsWithCharging();

    }
}
