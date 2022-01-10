using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net.Mail;
using System.Net;
using BO;
using BL;
using BlApi;
using System.ComponentModel;
using System.Threading;

namespace BL
{
    internal class Simulation
    {
        enum Maintenance { Finding, Going, Charging }
        private const int VELOCITY = 100;
        private const int DELAY = 500;
        private const double SECONDS_PASSED = DELAY / 1000.0;
        private const double WayTraveled = VELOCITY * SECONDS_PASSED;

        private BL bl;
        private int droneId;
        private Func<bool> func;
        private Action reportProgress;

        public Simulation(BL Bl, int droneId, Func<bool> threadStop, Action reportProgress)
        {
            this.bl = Bl;
            this.droneId = droneId;
            this.func = threadStop;
            var dal = bl.idal1;
            this.reportProgress = reportProgress;
            DroneToList drone = bl.GetAllDrones(x => x.Id == droneId).First();
            int? parcelId = null;
            int? StationId = null;
            Station station = null;
            double distance = 0.0;
            int baterryUsage = 0;
            DO.Parcel? parcel = null;
            bool pickedUp = false;
            Customer customer = null;
            Maintenance maintenance = Maintenance.Finding;

            void initDelivery(int Id)
            {

                parcel = dal.GetParcel(Id);
                baterryUsage = dal.DronePwrUsg()[(int)((DO.Parcel)parcel).Weight + 1];// (int)Enum.Parse(typeof(BatteryUsage), parcel?.Weight.ToString());
                pickedUp = parcel?.PickedUp is not null;
                customer = bl.GetCustomer((pickedUp ? parcel?.ReceiverId : parcel?.SenderId));
            }

            do
            {
                //(var next, var id)=drone.nextAction(bl);
                switch (drone)
                {
                    case DroneToList { Status: DroneStatuses.Available }:
                        if (!sleepDelayTime()) break;

                        lock (bl)
                        {
                            // parcelId = dal.GetAllParcels(p => p.Assigned == null && (int)p.Weight <= (int)drone.Weight && BatteryUsage(droneDisFromPack, 0) + BatteryUsage(toStat, 0) + BatteryUsage(betweenCus, (int)pack.Weight + 1) < drone.Battery)
                            //    dal.GetAllParcels(p => p.Created == null && (int)p.Weight <= (int)drone.Weight
                            //&& (WeightCategories)(p.Weight) <= drone.Weight
                            //&& (Bl, (int)p?.Id) < drone.Battery)
                            //    .OrderByDescending(p => p?.Priority)
                            //    .thenByDescending(parcel => parcel?.Weight)
                            //    .FirstOrDefault()?.Id;
                            bl.AssignParcelToDrone(drone.Id);
                            parcelId = drone.ParcelId;
                            switch (parcelId, drone.Battery)
                            {
                                case (null, 100):
                                    drone.Status = DroneStatuses.Available;
                                    break;
                                case (null, _):
                                    StationId = bl.FindClosestStation(drone).Id;
                                    if (StationId != null)
                                    {
                                        drone.Status = DroneStatuses.Maintenance;
                                        maintenance = Maintenance.Finding;
                                        // bl.idal1.
                                        bl.idal1.SendToCharge(droneId, (int)StationId);
                                    }
                                    break;
                                case (_, _):
                                    try
                                    {
                                        //dal.ParcelSchedule((int)parcelId, droneId);
                                        //bl.CollectingParcelByDrone(drone.Id);
                                        //bl.DeliverParcelByDrone(droneId);
                                        //drone.ParcelId = (int)parcelId;
                                        initDelivery((int)parcelId);
                                        drone.Status = DroneStatuses.Delivery;
                                    }
                                    catch (DO.MissingIdException ex)
                                    {
                                        throw new RetrievalException("Internal error getting parcel or customer", ex);
                                    }
                                    break;
                            }
                        }
                        break;
                    case DroneToList { Status: DroneStatuses.Maintenance }: // if the drone is in maintenance
                        switch (maintenance)
                        {
                            case Maintenance.Finding:
                                lock (Bl)
                                {
                                    try
                                    { 
                                        station = Bl.GetStation(StationId ?? dal.GetDroneChargeList(dc=> dc.DroneId == drone.Id).First().StationId);
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        throw new RetrievalException("Could not find wanted station", ex);
                                    }
                                    distance = bl.DroneDistanceFromStation(drone,null,station);
                                    maintenance = Maintenance.Going;
                                }
                                break;
                            case Maintenance.Going:
                                if (distance < 0.01)// if its 'at' the station
                                    lock (Bl)
                                    {
                                        drone.Loc = station.Loc;
                                        maintenance = Maintenance.Charging;
                                    }
                                else
                                {
                                    if (!sleepDelayTime())
                                        break;
                                    lock (Bl)
                                    {
                                        double actualDistance = distance < WayTraveled ? distance : WayTraveled;
                                        distance -= actualDistance;
                                       // drone.Battery = Max(0.0, droneBattery - actualDistance * Bl.BatteryUsages[DRONE_FREE]);
                                        drone.Battery = Math.Max(0, drone.Battery - ((int)Math.Round(actualDistance) * dal.DronePwrUsg()[0]));
                                    }
                                }
                                break;

                            case Maintenance.Charging:
                                if (drone.Battery == 100)
                                    lock (Bl)
                                    {
                                        drone.Status = DroneStatuses.Available;
                                        Bl.ReleasingDroneFromCharge(droneId);
                                        //Bl.idal1.StationDroneOut(station.Id);
                                    }
                                else
                                {
                                    if (!sleepDelayTime())
                                        break;
                                    lock (Bl)
                                    {
                                        //drone.Battery = int.MinValue(100, drone.Battery + Bl.BatteryUsages[DRONE_CHARGE] * SECONDS_PASSED);
                                        int batteryCharge = (int)(SECONDS_PASSED/3600) * dal.DronePwrUsg()[4];
                                        drone.Battery =Math.Min(100, drone.Battery + batteryCharge);
                                    }
                                }
                                break;
                            default:
                                throw new DroneChargeException("Internal error:wrong maintenace substat");
                        }
                        break;
                    case DroneToList { Status: DroneStatuses.Delivery }:
                        lock (Bl)
                        {
                            //    try
                            //    {
                            //        if (parcelId == null)
                            //            initDelivery((int)drone.DeliveryId);
                            //    }
                            //    catch (DO.MissingIdException ex)
                            //    {
                            //        throw new BadStatusException("Internal error getting parcel", ex);
                            //    }
                            distance = bl.DronedistanceFromCustomer(drone, customer);// drone.Distance(customer);
                                                                                     // pickedUp? drone.Battery==drone.Battery-bl.BatteryUsage(distance, (int)((DO.Parcel)parcel).Weight + 1)
                        }
                        if (distance < 0.01 || drone.Battery <= drone.Battery - bl.BatteryUsage(distance, pickedUp ? ((int)((DO.Parcel)parcel).Weight + 1) : 0) + 3)
                            lock (Bl)
                            {
                                drone.Loc = customer.Loc;
                                if (pickedUp)
                                {
                                    bl.DeliverParcelByDrone(droneId);
                                    //bl.DeliverParcelByDrone((int)parcel?.Id);
                                    drone.Status = DroneStatuses.Available;
                                }
                                else
                                {
                                    bl.CollectingParcelByDrone(droneId);
                                    //Dal.parcelPickup((int)parcel ? Id);
                                    customer = Bl.GetCustomer(parcel?.ReceiverId);
                                    pickedUp = true;
                                }
                            }
                        else
                        {
                            if (!sleepDelayTime()) break;
                            lock (Bl)
                            {
                                double actualDistance = distance < WayTraveled ? distance : WayTraveled;
                                double Proportion = actualDistance / distance;
                                int usg = pickedUp ? ((int)((DO.Parcel)parcel).Weight + 1) : 0;
                                drone.Battery = Math.Max(0,drone.Battery -((int) Math.Round( actualDistance) * dal.DronePwrUsg()[usg]));
                                double lat = drone.Loc.Latitude + (customer.Loc.Latitude - drone.Loc.Latitude) * Proportion;
                                double lon = drone.Loc.Longitude + (customer.Loc.Longitude - drone.Loc.Longitude) * Proportion;
                                drone.Loc = new() { Latitude = lat, Longitude = lon };
                            }
                        }
                        break;

                    default:
                        throw new StatusException("Internal error:no suitable status");
                }
                reportProgress();
            } while (!threadStop());
        }
        private static bool sleepDelayTime()
        {
            // try { Thread.Sleep(DELAY); }
            try { Thread.Sleep(DELAY); }
            catch (ThreadInterruptedException)
            {
                return false;
            }
            return true;
        }
    }
}