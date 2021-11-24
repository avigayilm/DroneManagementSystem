using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DAL;
using IDAL;

namespace BL
{
    public partial class BL
    {
        /// <summary>
        /// adding a station to the list in the Datalayer
        /// </summary>
        /// <param name="station"></param>
        public void AddStation(Station station)
        {
            try
            {
                if (station.AvailableChargeSlots < 0)
                    throw new InvalidInputException("The number of charging slots is less than 0 \n");
                if (station.Id <= 0)
                    throw new InvalidInputException("The Id is less than zero \n");
                if (string.IsNullOrEmpty(station.Name))
                    throw new InvalidInputException("The name is\n");
                if (station.Loc.Latitude <= -90.0 || station.Loc.Latitude >= 90.0)// out of range of latitude
                    throw new InvalidInputException("The latitude is not in a existing range(between -90 and 90) \n");
                if (station.Loc.Longitude <= -180.0 || station.Loc.Longitude >= 180.0)// out of range of latitude
                    throw new InvalidInputException("The longitude is not in a existing range(betweeen -180 and 180)\n");
                IDAL.DO.Station st = new();
                object obj = st;
                station.CopyProperties(obj);
                st = (IDAL.DO.Station)obj;
                st.Latitude = station.Loc.Latitude;
                st.Longitude = station.Loc.Longitude;
                idal1.AddStation(st);
                station.Charging = new();
            }
            //catch(InvalidInputException ex)
            //{
            //    throw new AddingException("Couldn't add the station.\n,", ex);
            //}
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new AddingException("Couldn't add the station.\n,", ex);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new AddingException("Couldn't add the station.\n,", ex);
            }
        }



        /// <summary>
        /// updates the name and amount of chargingslots of the station
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="name"></param>
        /// <param name="chargingSlots"></param>
        public void UpdateStation(int stationId, string name, int chargingSlots)
        {
            try
            {
                idal1.UpdateStation(stationId, name, chargingSlots);
            }
            catch (IDAL.DO.MissingIdException ex)
            {
                throw new UpdateIssueException("Invalid ID.\n,", ex);
            }


        }

        /// <summary>
        /// returns a the closest station with available stations
        /// </summary>
        /// <param name="withCharging"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        internal IDAL.DO.Station FindClosestStation(DroneToList dr)
        {

            double minDistance = double.MaxValue; IDAL.DO.Station temp = new();
            List<IDAL.DO.Station> tempList =idal1.GetAllStations(s=>s.AvailableChargeSlots>0).ToList();
            foreach (IDAL.DO.Station st in tempList)
            {
                double distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, st.Longitude, st.Latitude);
                if (distance < minDistance)
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
        internal bool CanReachstation(DroneToList dr,IDAL.DO.Station station)
        {
            //IDAL.DO.Station closestStation = FindClosestStation(dr);
            if (BatteryUsage(Bonus.Haversine(dr.Loc.Longitude,dr.Loc.Latitude,station.Longitude,station.Latitude), 0) < dr.Battery)
                return true;
            throw new BatteryIssueException("Not enough battery to fly to the station\n");
        }

        //internal (IDAL.DO.Station, double) FindClosestPossibleStation1(DroneToList dr)
        //{

        //    double minDistance = double.MaxValue; IDAL.DO.Station temp = new();
        //    List<IDAL.DO.Station> AvailableChargeList = idal1.GetAllStations(s => s.AvailableChargeSlots > 0).ToList();
        //    foreach (IDAL.DO.Station st in AvailableChargeList)
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

      
        public Station GetStation(int stationId)
        {
            try
            {
                Station station = new();
                IDAL.DO.Station stationDal = idal1.GetStation(stationId);
                stationDal.CopyProperties(station);
                station.Loc = new() { Longitude = stationDal.Longitude, Latitude = stationDal.Latitude };
                station.Charging = getAllDroneInCharge(stationId).Item1.ToList();
                return station;
            }

            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Station.\n,", ex);
            }
        }

    
        public IEnumerable<StationToList> GetAllStation(Predicate<IDAL.DO.Station> predicate = null)
        {
            List<StationToList> tempList = new();
            
            idal1.GetAllStations(predicate).CopyPropertyListtoIBLList(tempList);
            foreach (StationToList temp in tempList)
            {
                var slots = idal1.AvailableAndOccupiedSlots(temp.Id);
                temp.OccupiedSlots = slots.Item1;
                temp.AvailableChargeSlots = slots.Item2;
            }
            return tempList;
        }

        /// <summary>
        /// returns all stations with available charging slots
        /// </summary>
        //public IEnumerable<StationToList> GetAllStationsWithCharging()
        //{
        //    List<StationToList> tempList = new();
        //    idal1.GetAllStations(s => s.AvailableChargeSlots > 0).ToList().CopyPropertyListtoIBLList(tempList);
        //    return tempList;
        //}
    }
}
