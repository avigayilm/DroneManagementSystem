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
                throw new DuplicateIdException("The Station already exists.\n,", ex);
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
    }
}
