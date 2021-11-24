﻿using IBL.BO;
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
        /// <summary>
        /// adding a drone to the list of the datalayer
        /// </summary>
        /// <param name="newDrone"></param>
        /// <param name="stationId"></param>
        public void AddDrone(Drone newDrone, int stationId);
        public void AddParcel(Parcel newParcel);
        public void UpdateDrone(int droneId, string model);
        /// <summary>
        /// returning a ParcelInTransfer
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public ParcelInTransfer GetParcelInTransfer(int parcelId);
        public void UpdateStation(int stationId, string name, int chargingSlots);
        public void SendingDroneToCharge(int droneId);
        public void AssignParcelToDrone(int droneId);
        /// <summary>
        /// pre assigned drone now collects parcel
        /// </summary>
        /// <param name="droneId"></param>
        public void DeliverParcelByDrone(int droneId);
        /// <summary>
        /// pre assigned drone now collects parcel
        /// </summary>
        /// <param name="droneId"></param>
        public void CollectingParcelByDrone(int droneId);
        /// <summary>
        /// returns a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>

        public Customer GetCustomer(string customerId);
        /// <summary>
        /// returns a list of customers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerToList> GetAllCustomers();
        /// <summary>
        /// returns a DroneToList
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public DroneToList getDroneToList(int droneId);
        /// <summary>
        /// returns a drone according to the droneId
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone GetDrone(int droneId);
        /// <summary>
        /// returns a list of drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneToList> GetAllDrones();
        /// <summary>
        /// returning a ParcelAtCustomer
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public ParcelAtCustomer GetParcelAtCustomer(int parcelId);
        /// <summary>
        /// returns a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId);
        /// <summary>
        /// returns all parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelToList> GetAllParcels();
        /// <summary>
        /// returns all unassigned parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelToList> GetAllUnassignedParcels();
        /// <summary>
        /// returns a station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station GetStation(int stationId);
        /// <summary>
        /// returns a list with all stations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StationToList> GetAllStation();
        public IEnumerable<StationToList> GetAllStationsWithCharging();
        /// <summary>
        /// updates the name and phone of the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustomer(string customerId, string name, string phone);
        //public ParcelStatuses GetParcelStatus(int parcelId)
        //;
        /// <summary>
        /// release teh drone from charge, updates the battery according to the charging time
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="chargingTime"></param>
        public void ReleasingDroneFromCharge(int droneId, double chargingTime);
        public DroneInCharge getDroneInCharge(int droneId);
        public CustomerInParcel GetCustomerInParcel(string customerId);
    }
}
