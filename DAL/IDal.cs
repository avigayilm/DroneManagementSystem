using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace IDAL
{
    public interface IDal
    {

        //public DalObject();
        public void AddStation(Station stat);
        public void AddDrone(Drone dro);
        public void AddCustomer(Customer cus);
        public void AddParcel(Parcel pack);
        public void ParcelDrone(int parcelId, int droneId);
        public void ParcelPickedUp(int parcelId);
        public void ParcelDelivered(int parcelId);
       //public IEnumerable<Parcel> DeliveredParcels();
        //public void ChangeDroneStatus(int DroneId, DroneStatuses st);
        //public IEnumerable<Parcel> UnAssignedParcels();
        public List<Customer> CustomersDeliverdTo();

        public void ChangeChargeSlots(int stationId, int n);
        /// <summary>
        /// returns tuple first occupied slots second available slots
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public (int, int) AvailableAndOccupiedSlots(int id);
        public void SendToCharge(int droneId, int stationId);
        public void BatteryCharged(int droneId, int stationId);
        public Parcel GetParcel(int ID);
        /// <summary>
        /// returns the customer according to the given ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Customer GetCustomer(string ID);
        public Drone GetDrone(int ID);
        public Station GetStation(int ID);
        /// <summary>
        /// returns the list of customers as Ienumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null);
        /// <summary>
        /// returns the list of all drones as Ienumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null);
        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null);
        /// <summary>
        /// returns a list of Dronecharge
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDroneChargeList();
       // public IEnumerable<Parcel> GetvacantParcel();
        public double[] DronePwrUsg();

       // public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null);
        //public IEnumerable<Station> GetStationWithCharging();
       // public IEnumerable<Parcel> UndeliveredParcels();
        public void UpdateDrone(int droneId, string model);
        public void UpdateStation(int stationId, string name, int chargeSlots);
        /// <summary>
        /// updates the customer, gets a customerId and changes the name and phone
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustomer(string customerId, string name, string phone);
        public Station SmallestDistanceStation(string cusId);
      
        
        public IEnumerable<Drone> DronesChargingAtStation(int stationId);
        public void CheckDuplicateStation(int stationId);
        /// <summary>
        /// checks if a station exists in the customerlist, if it doesn't it throws a MissingIdException
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public int CheckExistingStation(int stationId);
 
        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null);

    }
}
