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
        /// <summary>
        /// adds a drone to the dronlist
        /// </summary>
        /// <param name="dro"></param>
        void AddDrone(Drone dro);
        /// <summary>
        /// ading a customer to the cusotmer list
        /// </summary>
        /// <param name="cus"></param>
        void AddCustomer(Customer cus);

        /// <summary>
        /// adds a parcel to the parcellist
        /// </summary>
        /// <param name="pack"></param>
        int AddParcel(Parcel pack);

        /// <summary>
        /// assigning a drone to a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        void ParcelDrone(int parcelId, int droneId);

        /// <summary>
        /// the drone picks up the parcel, therefore updating the drone's status to delivery 
        /// and updating the picked up time
        /// </summary>
        /// <param name="parcelId"></param>
        void ParcelPickedUp(int parcelId);

        /// <summary>
        ///  funciton updated the parcel to delivered. It changes the drone's status to available
        ///  and updates the time of requested
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="day"></param>
        void ParcelDelivered(int parcelId);
        //public IEnumerable<Parcel> DeliveredParcels();
        //public void ChangeDroneStatus(int DroneId, DroneStatuses st);
        //public IEnumerable<Parcel> UnAssignedParcels();

        /// <summary>
        /// returns the list of customers having received a parcel
        /// </summary>
        /// <returns></returns>
        IEnumerable<Customer> CustomersDeliverdTo();

        /// <summary>
        /// checks if a customer exists in the customerlist, if it doesn't it throws a MissingIdException
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public int CheckExistingCustomer(string customerId);

        /// <summary>
        /// funciton that changes the amount of chargeslots in a station according to the given parameter.
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="n"></param>
        void ChangeChargeSlots(int stationId, int n);
        /// <summary>
        /// returns tuple first occupied slots second available slots
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        (int, int) AvailableAndOccupiedSlots(int id);
       
        /// <summary>
        /// sending a drone to charge in a station, adding the drone to the dronechargelist
        /// </summary>
        /// <param name="DroneId"></param>
        /// <param name="StationId"></param>
        bool SendToCharge(int droneId, int stationId);

        /// <summary>
        /// Once the drone is charged release the drone from the station, update the chargeslots, and remove the drone from the dronechargelist.
        /// </summary>
        /// <param name="Buzzer"></param>
        void BatteryCharged(int droneId, int stationId);

        /// <summary>
        /// The get functions return a string with all the information of the lists
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Parcel GetParcel(int ID);
        /// <summary>
        /// returns the customer according to the given ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Customer GetCustomer(string ID);
        
        /// <summary>
        /// returns the drone according to the given Id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
      
        Drone GetDrone(int ID);

        /// <summary>
        /// returns a station according to the given Id
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns a parcellist
        /// </summary>
        /// <returns></returns>
        IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null);
        /// <summary>
        /// returns a list of Dronecharge
        /// </summary>
        /// <returns></returns>
        List<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate = null);
       // public IEnumerable<Parcel> GetvacantParcel();
        int[] DronePwrUsg();

        // public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null);
        //public IEnumerable<Station> GetStationWithCharging();
        // public IEnumerable<Parcel> UndeliveredParcels();

        /// <summary>
        /// updates the model of the drone, used in BL
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        void UpdateDrone(int droneId, string model);

        /// <summary>
        /// updates the station with chargeslots and name.
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="name"></param>
        /// <param name="chargeSlots"></param>
        void UpdateStation(int stationId, string name, int chargeSlots);
        /// <summary>
        /// updates the customer, gets a customerId and changes the name and phone
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        void UpdateCustomer(string customerId, string name, string phone);

        /// <summary>
        /// returns the nearest station to a customer
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        Station SmallestDistanceStation(string cusId);

        /// <summary>
        /// returns a list of all the drones charging at a station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// bonus: Doesn't completely delete the parcel but it has a sign that it is deleted
        /// </summary>
        /// <param name="parcelId"></param>
        public void DeleteParcel(int parcelId);

        /// <summary>
        /// bonus: Doesn't completely delete the station but it has a sign that it is deleted
        /// </summary>
        /// <param name="stationId"></param>
        public void DeleteStation(int stationId);

        /// <summary>
        /// bonus: Doesn't completely delete the drone but it has a sign that it is deleted
        /// </summary>
        /// <param name="droneId"></param>
        public void DeleteDrone(int droneId);

        /// <summary>
        /// bonus: Doesn't completely delete the customer but it has a sign that it is deleted
        /// </summary>
        /// <param name="customerId"></param>
        public void DeleteCustomer(string customerId);


    }
}
