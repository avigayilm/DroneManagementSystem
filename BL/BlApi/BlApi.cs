using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlApi
{
    public interface Ibl
    {
        /// <summary>
        /// adding a station to the list in the Datalayer
        /// </summary>
        /// <param name="station"></param>
        void AddStation(Station tempStat);

        /// <summary>
        /// adding a customer in the list of the datalayer
        /// </summary>
        /// <param name="newCustomer"></param>
        void AddCustomer(Customer newCustomer);

        /// <summary>
        /// adding a drone to the list of the datalayer
        /// </summary>
        /// <param name="newDrone"></param>
        /// <param name="stationId"></param>
        void AddDrone(Drone newDrone, int stationId);

        /// <summary>
        /// adds a parcel to the parcelList
        /// </summary>
        /// <param name="newParcel"></param>
        void AddParcel(Parcel newParcel);

        /// <summary>
        /// updates the drone's model
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        void UpdateDrone(int droneId, string model);

        /// <summary>
        /// returning a ParcelInTransfer
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        ParcelInTransfer GetParcelInTransfer(int parcelId);

        /// <summary>
        /// updates the name and amount of chargingslots of the station
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="name"></param>
        /// <param name="chargingSlots"></param>
        void UpdateStation(int stationId, string name, int chargingSlots);

        /// <summary>
        /// sends a given drone to charge in apropriate station
        /// </summary>
        /// <param name="droneId"></param>
        void SendingDroneToCharge(int droneId);

        /// <summary>
        /// assigns a fitting parcel to given drone updating parcel in idal and drone list in bl
        /// </summary>
        /// <param name="droneId"></param>
        void AssignParcelToDrone(int droneId);

        /// <summary>
        /// pre assigned drone now collects parcel
        /// </summary>
        /// <param name="droneId"></param>
        void DeliverParcelByDrone(int droneId);

        /// <summary>
        /// pre assigned drone now collects parcel
        /// </summary>
        /// <param name="droneId"></param>
        void CollectingParcelByDrone(int droneId);

        /// <summary>
        /// returns a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns
        Customer GetCustomer(string customerId);

        /// <summary>
        /// returns a list of customers
        /// </summary>
        /// <returns></returns>
        IEnumerable<CustomerToList> GetAllCustomers();
        /// <summary>
        /// returns a DroneToList
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        DroneToList getDroneToList(int droneId);
        /// <summary>
        /// returns a drone according to the droneId
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        Drone GetDrone(int droneId);
        /// <summary>
        /// returns a list of drones
        /// </summary>
        /// <returns></returns>
        IEnumerable<DroneToList> GetAllDrones(Predicate<DroneToList> predicate = null);
        /// <summary>
        /// returning a ParcelAtCustomer
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        ParcelAtCustomer GetParcelAtCustomer(int parcelId);
        /// <summary>
        /// returns a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        Parcel GetParcel(int parcelId);
        /// <summary>
        /// returns all parcels
        /// </summary>
        /// <returns></returns>
        IEnumerable<ParcelToList> GetAllParcels(Predicate<DO.Parcel> predicate = null);
        /// <summary>
        /// returns all unassigned parcels
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<ParcelToList> GetAllUnassignedParcels();
        /// <summary>
        /// returns a station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        Station GetStation(int stationId);
        /// <summary>
        /// returns a list with all stations
        /// </summary>
        /// <returns></returns>
        IEnumerable<StationToList> GetAllStation(Predicate<StationToList> predicate = null);
        //public IEnumerable<StationToList> GetAllStationsWithCharging();
        /// <summary>
        /// updates the name and phone of the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        void UpdateCustomer(string customerId, string name, string phone);
        //public ParcelStatuses GetParcelStatus(int parcelId)
        //;
        /// <summary>
        /// release teh drone from charge, updates the battery according to the charging time
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="chargingTime"></param>
        void ReleasingDroneFromCharge(int droneId);
        /// <summary>
        /// given a customer id returns customer as customer in parcel
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        CustomerInParcel GetCustomerInParcel(string customerId);

        public void Register(BO.Customer cus, string user, string password, bool cusOrStff);
        public bool Login(string user, string pass);
        public (IEnumerable<DroneInCharge>, int) getAllDroneInCharge(int stationId);
    }
}
