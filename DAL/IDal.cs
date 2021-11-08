﻿using System;
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
        public void ParcelDelivered(int parcelId, DateTime day);
        //public void ChangeDroneStatus(int DroneId, DroneStatuses st);
        public void ChangeChargeSlots(int stationId, int n);
        public void SendToCharge(int droneId, int stationId);
        public void BatteryCharged(int droneId, int stationId);
        public Parcel GetParcel(int ID);
        public Customer GetCustomer(string ID);
        public Drone GetDrone(int ID);
        public Station GetStation(int ID);
        public IEnumerable<Customer> GetAllCustomers();
        public IEnumerable<Drone> GetAllDrones();
        public IEnumerable<Parcel> GetParcelList();
        //public IEnumerable<DroneCharge> GetDroneChargeList);
        public IEnumerable<Parcel> GetvacantParcel();
        public double[] DronePwrUsg();
        public IEnumerable<Station> GetStationWithCharging();
        public IEnumerable<Parcel> UndeliveredParcels();
        public Station smallestDistanceStation(string cusId);
    }
}