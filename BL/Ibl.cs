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
        public void AddDrone(DroneToList newDrone, int stationId);
        void AddParcel(Parcel newParcel);
        void updateDrone(int droneId, string model);
        public ParcelInTransfer GetParcelInTransfer(int parcelId);
        void UpdateStation(int stationId, string name, int chargingSlots);
        void UpdateCustomer(int customerId, string name, string phone);
        void SendingDroneToCharge(int droneId);
        int GetDroneIndex(int ID);
        double BatteryUsage(double distance, int pwrIndex);
        IDAL.DO.Station FindPossibleStation(List<IDAL.DO.Station> withCharging, DroneToList dr);
        void ReleasingDroneFromCharge(int droneId, double chargingTime);
        double DroneDistanceFromParcel(DroneToList dr, IDAL.DO.Parcel par);
        double StationDistanceFromCustomer(IDAL.DO.Customer cus, IDAL.DO.Station st);
        double DistanceBetweenCustomers(IDAL.DO.Customer cus1, IDAL.DO.Customer cus2);
        void AssignParcelToDrone(int droneId);
        void CollectingParcelByDrone(int droneId);
        void DeliverParcelByDrone(int droneId);
        Station GetStation(int stationId);
        DroneToList getDroneToList(int droneId);
        Drone getDrone(int droneId);
        Customer getCustomer(string customerId);
        Parcel getParcel(int parcelId);
        void getStation(int stationId);
        IEnumerable<StationToList> GetAllStation();
        IEnumerable<DroneToList> GetAllDrones();
        IEnumerable<CustomerToList> GetAllCustomers();
        IEnumerable<PackageToList> GetAllParcels();
        IEnumerable<PackageToList> GetAllUnassignedParcels();
        IEnumerable<StationToList> GetAllStationsWithCharging();
    }
}
