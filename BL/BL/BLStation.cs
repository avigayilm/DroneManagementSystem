using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;
using DO;
using DalApi;

namespace BL
{
    public partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(BO.Station station)
        {
            try
            {
                if (station.AvailableChargeSlots < 0)
                    throw new BO.InvalidInputException("The number of charging slots is less than 0");
                if (station.Id <= 0)
                    throw new InvalidInputException("The Id is less than zero");
                if (string.IsNullOrEmpty(station.Name))
                    throw new InvalidInputException("The name is\n");
                if (station.Loc.Latitude <= -90.0 || station.Loc.Latitude >= 90.0)// out of range of latitude
                    throw new InvalidInputException("The latitude is not in a existing range(between -90 and 90)");
                if (station.Loc.Longitude <= -180.0 || station.Loc.Longitude >= 180.0)// out of range of latitude
                    throw new InvalidInputException("The Longitude is not in a existing range(betweeen -180 and 180)");
                DO.Station st = new DO.Station();
                object obj = st;
                station.CopyProperties(obj);
                st = (DO.Station)obj;
                st.Latitude = station.Loc.Latitude;
                st.Longitude = station.Loc.Longitude;
                lock (idal1)
                {
                    idal1.AddStation(st);
                }
                station.Charging = new List<DroneInCharge>();
            }

            catch (DO.DuplicateIdException ex)
            {
                throw new AddingException("Couldn't add the station.", ex);
            }
            catch (DO.MissingIdException ex)
            {
                throw new AddingException("Couldn't add the station.", ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(int stationId, string name, int chargingSlots)
        {
            try
            {
                lock (idal1)
                {
                    idal1.UpdateStation(stationId, name, chargingSlots);
                }
                
            }
            catch (DO.MissingIdException ex)
            {
                throw new UpdateIssueException("Invalid ID.", ex);
            }
        }

        /// <summary>
        /// returns a the closest station with available stations
        /// </summary>
        /// <param name="withCharging"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        internal DO.Station FindClosestStation(DroneToList dr)
        {
            double minDistance = double.MaxValue;
            DO.Station temp = new();
            lock (this)
            {
                List<DO.Station> tempList = idal1.GetAllStations(s => s.AvailableChargeSlots > 0).ToList();
                foreach (var (st, distance) in from DO.Station st in tempList
                                               let distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, st.Longitude, st.Latitude)
                                               where distance < minDistance
                                               select (st, distance))
                {
                    minDistance = distance;
                    temp = st;
                }
            }
            
            return temp;
        }

        /// <summary>
        /// checks if the drone has enough battery to fly to the station
        /// </summary>
        /// <param name="withCharging"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        internal bool CanReachstation(DroneToList dr, DO.Station station)
        {
            if (BatteryUsage(Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, station.Longitude, station.Latitude), 0) < dr.Battery)
                return true;
            throw new BatteryIssueException("Not enough battery to fly to the station");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Station GetStation(int stationId)
        {
            try
            {
                BO.Station station = new BO.Station();
                lock (idal1)
                {
                    DO.Station stationDal = idal1.GetStation(stationId); //get the station from DAL
                    stationDal.CopyProperties(station); // copy its preperties to BL
                    station.Loc = new Location() { Longitude = stationDal.Longitude, Latitude = stationDal.Latitude };
                    station.Charging = from item in idal1.GetDroneChargeList(d => d.StationId == stationId)
                                       let temp = droneBL.FirstOrDefault(curDrone => curDrone.Id == item.DroneId)
                                       select new DroneInCharge
                                       {
                                           Id = item.DroneId,
                                           Battery = temp != default ? temp.Battery : throw new RetrievalException("the Id number doesnt exist\n")
                                       };
                    station.Charging = getAllDroneInCharge(stationId).Item1.ToList();
                }
                
                return station;
            }

            catch (DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Station.", ex);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetAllStation(Predicate<StationToList> predicate = null)
        {
            List<StationToList> tempList = new List<StationToList>();
            lock (idal1)
            {
                idal1.GetAllStations().CopyPropertyListtoIBLList(tempList); //will return a list according to truth value of predicate
                foreach (StationToList temp in tempList)
                {
                    var slots = idal1.AvailableAndOccupiedSlots(temp.Id);
                    temp.OccupiedSlots = slots.Item1;
                    temp.AvailableChargeSlots = slots.Item2;
                }
            }
           
            return tempList;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int stationId)
        {
            try
            {
                idal1.DeleteStation(stationId);
            }
            catch (MissingIdException ex)
            {
                throw new RetrievalException(ex.Message);
            }
        }

    }
}

//internal (DO.Station, double) FindClosestPossibleStation1(DroneToList dr)
//{

//    double minDistance = double.MaxValue; DO.Station temp = new();
//    List<DO.Station> AvailableChargeList = idal1.GetAllStations(s => s.AvailableChargeSlots > 0).ToList();
//    foreach (DO.Station st in AvailableChargeList)
//    {
//        double distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, st.Longitude, st.Latitude);
//        if (distance < minDistance)
//        {
//            minDistance = distance;
//            temp = st;
//        }
//    }
//    if (BatteryUsage(minDistance, 0) < dr.Battery)
//        return (temp, minDistance);
//    throw new BatteryIssueException("Not enough battery to fly to closest station\n");
//}