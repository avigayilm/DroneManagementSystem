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
                if (station.Id <= 0)// maybe I have to check that it is 3 digits
                    throw new InvalidInputException("The Id is less than zero \n");
                if (station.Loc.Latitude <= -90 || station.Loc.Latitude >= 90)// out of range of latitude
                    throw new InvalidInputException("The latitude is not in a existing range(between -90 and 90) \n");
                if (station.Loc.Longitude <= -180 || station.Loc.Longitude >= 180)// out of range of latitude
                    throw new InvalidInputException("The longitude is not in a existing range(betweeen -180 and 180)\n");
                IDAL.DO.Station st = new();
                station.CopyPropertiestoIDAL(st);
                idal1.AddStation(st);
                station.Charging = new();
            }
            catch (IDAL.DO.DuplicateIdException ex)
            {
                throw new AddingException("Couldn'd add the station.\n,", ex);
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
                throw new MissingIdException("Invalid ID.\n,", ex);
            }


        }

        /// <summary>
        /// returns a station which is has charging and the drone has enough battery to fly to
        /// </summary>
        /// <param name="withCharging"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        internal IDAL.DO.Station FindClosestPossibleStation(DroneToList dr)
        {

            double minDistance = double.MaxValue; IDAL.DO.Station temp = new();
            foreach (IDAL.DO.Station st in idal1.GetAllStations())
            {
                double distance = Bonus.Haversine(dr.Loc.Longitude, dr.Loc.Latitude, st.Longitude, st.Latitude);
                if (distance < minDistance && st.AvailableChargeSlots > 0)
                {
                    minDistance = distance;
                    temp = st;
                }
            }
            if (BatteryUsage(minDistance, 0) < dr.Battery) return temp;
            throw new catgettochargeException;
        }

        public Station GetStation(int stationId)
        {
            try
            {
                Station station = new();
                IDAL.DO.Station stationDal = idal1.GetStation(stationId);
                stationDal.CopyPropertiestoIBL(station);
                List<IDAL.DO.Station> chargingListIdal = (List<IDAL.DO.Station>)idal1.DronesChargingAtStation(stationId);
                chargingListIdal.CopyPropertyListtoIBLList(station.Charging);// converts the list to a DroneInChargeLists
                return station;
            }

            catch (IDAL.DO.MissingIdException ex)
            {
                throw new RetrievalException("Couldn't get the Station.\n,", ex);
            }
        }


        public IEnumerable<StationToList> GetAllStation()
        {
            List<StationToList> tempList = new();
            int[] slots;
            idal1.GetAllStations().CopyPropertyListtoIBLList(tempList);
            foreach (StationToList temp in tempList)
            {
                slots = idal1.AvailableAndEmptySlots(temp.Id);
                temp.OccupiedSlots = slots[0];
                temp.AvailableSlots = slots[1];
            }
            return tempList;
        }

        /// <summary>
        /// returns all stations with available charging slots
        /// </summary>
        public IEnumerable<StationToList> GetAllStationsWithCharging()
        {
            List<StationToList> tempList = new();
            idal1.GetStationWithCharging().CopyPropertyListtoIBLList(tempList);
            return tempList;
        }
    }
}
