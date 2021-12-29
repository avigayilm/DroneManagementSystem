using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace DalApi
{
    public interface Idal
    {
        // public static DalObject Instance

        public void AddLogin(Login log);
        void AddStation(Station stat);
        void AddDrone(Drone dro);
        void AddCustomer(Customer cus);
        int AddParcel(Parcel pack);
         void ParcelDrone(int parcelId, int droneId);
         void ParcelPickedUp(int parcelId);
        void ParcelDelivered(int parcelId);
       //public IEnumerable<Parcel> DeliveredParcels();
        //public void ChangeDroneStatus(int DroneId, DroneStatuses st);
        //public IEnumerable<Parcel> UnAssignedParcels();
        List<Customer> CustomersDeliverdTo();
        public int CheckExistingCustomer(string customerId);

        void ChangeChargeSlots(int stationId, int n);
        /// <summary>
        /// returns tuple first occupied slots second available slots
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        (int, int) AvailableAndOccupiedSlots(int id);
        void SendToCharge(int droneId, int stationId);
        void BatteryCharged(int droneId, int stationId);
        Parcel GetParcel(int ID);
        /// <summary>
        /// returns the customer according to the given ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Customer GetCustomer(string ID);
        Drone GetDrone(int ID);
        Station GetStation(int ID);
        /// <summary>
        /// returns the list of customers as Ienumerable
        /// </summary>
        /// <returns></returns>
        IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null);
        /// <summary>
        /// returns the list of all drones as Ienumerable
        /// </summary>
        /// <returns></returns>
        IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null);
        IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null);
        /// <summary>
        /// returns a list of Dronecharge
        /// </summary>
        /// <returns></returns>
        IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate = null);
       // public IEnumerable<Parcel> GetvacantParcel();
        double[] DronePwrUsg();

       // public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null);
        //public IEnumerable<Station> GetStationWithCharging();
       // public IEnumerable<Parcel> UndeliveredParcels();
        void UpdateDrone(int droneId, string model);
        void UpdateStation(int stationId, string name, int chargeSlots);
        /// <summary>
        /// updates the customer, gets a customerId and changes the name and phone
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        void UpdateCustomer(string customerId, string name, string phone);
        Station SmallestDistanceStation(string cusId);
      
        
        IEnumerable<Drone> DronesChargingAtStation(int stationId);
        void CheckDuplicateStation(int stationId);
        /// <summary>
        /// checks if a station exists in the customerlist, if it doesn't it throws a MissingIdException
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        int CheckExistingStation(int stationId);
 
        IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null);
        public bool ValidateLogin(string user, string pass);

        /// <summary>
        /// updates the parcel receiver
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="recId"></param>
        public void UpdateParcel(int parcelId, string recId);

    }
}
