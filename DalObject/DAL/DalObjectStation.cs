using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;
using DalApi;


namespace Dal
{
    internal sealed partial class DalObject
    {

        /// <summary>
        /// adding the station to the lists
        /// </summary>
        /// <param name="stat"></param>
        public void AddStation(Station stat)
        {
            CheckDuplicateStation(stat.Id);
            DataSource.stationList.Add(stat);
        }

        /// <summary>
        /// funciton that changes the amount of chargeslots in a station according to the given parameter.
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="n"></param>
        public void ChangeChargeSlots(int stationId, int n)
        {
            int stationIndex = CheckExistingStation(stationId);
            var temp = DataSource.stationList[stationIndex];
            temp.AvailableChargeSlots += n;
            DataSource.stationList[stationIndex] = temp;

        }
        /// <summary>
        /// returns a station according to the given Id
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station GetStation(int stationId)
        {

            int index = CheckExistingStation(stationId);
            return DataSource.stationList[index];
        }
        /// <summary>
        ///  returns the list with the stations that have availble charging
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<Station> GetStationWithCharging()
        //{
        //    List<Station> temp = new();
        //    DataSource.stationList.ForEach(p => { if (p.AvailableChargeSlots > 0) { temp.Add(p); } });
        //    return (IEnumerable<Station>)temp;
        //}

        /// <summary>
        /// returns the whole list of stations
        /// <summary>
        /// <returns></returns>
        //public IEnumerable<Station> GetAllStations()
        //{
        //    List<Station> tempStationList = new();
        //    DataSource.stationList.ForEach(s => tempStationList.Add(s));
        //    return (IEnumerable<Station>)tempStationList;
        //}
        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            return DataSource.stationList.FindAll(x => predicate == null ? true : predicate(x));
        }
        /// <summary>
        /// returns the nearest station to a customer
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public Station SmallestDistanceStation(string cusId)
        {
            Customer temp = GetCustomer(cusId);
            double minDistance = double.PositiveInfinity;//starting with unlimited
            double distancekm;
            int index = -1;
            IEnumerator<Station> iter = DataSource.stationList.GetEnumerator();
            for (int i = 0; i < DataSource.stationList.Count; i++)
            {
                distancekm = Bonus.Haversine(DataSource.stationList[i].Longitude, DataSource.stationList[i].Latitude, temp.Longitude, temp.Latitude);
                if (distancekm < minDistance)
                {
                    minDistance = distancekm;
                    index = i;// the new index is the index with the smallest distance
                }
            }
            return DataSource.stationList[index];
            // returns the station with the smallest distance to customer
            //while(iter.MoveNext())
            //    distancekm = Bonus.Haversine(iter.Current.longitude, iter.Current.latitude, temp.longitude, temp.latitude);
            //if (distancekm < minDistance) ;
            //DataSource.stationList.ForEach(s => { double distancekm = Bonus.Haversine(s.longitude, s.latitude, temp.longitude, temp.latitude); if (distancekm < minDistance) minDistance = distancekm; });

        }
        /// <summary>
        /// updates the station with chargeslots and name.
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="name"></param>
        /// <param name="chargeSlots"></param>
        public void UpdateStation(int stationId, string name, int chargeSlots)
        {
            int index = CheckExistingStation(stationId);
            DataSource.stationList.FindIndex(s => s.Id == stationId);
            Station tempStation = DataSource.stationList[index];
            if (name != null)
                tempStation.Name = name;
            if (chargeSlots != -1)// chekc if chargeslots was enetered
                tempStation.AvailableChargeSlots = chargeSlots; /*AvailableAndOccupiedSlots(stationId).Item2;*/// input=total chargeslots, we only save the availablechargeslots
            DataSource.stationList[index] = tempStation;
        }



        
        public (int, int) AvailableAndOccupiedSlots(int id)
        {
            Station st = GetStation(id);
            return (DataSource.chargeList.Where(s => s.StationId == st.Id).Count(),st.AvailableChargeSlots);
        }

      
        public int CheckExistingStation(int stationId)
        {
            int index = DataSource.stationList.FindIndex(s => s.Id == stationId);
            if (index == -1)
            {
                throw new MissingIdException("No such Station exists\n");
            }
            return index;
        }

        /// <summary>
        /// checks if a station already exists, if it does it throws a duplicateIdException
        /// </summary>
        /// <param name="stationId"></param>
        public void CheckDuplicateStation(int stationId)
        {
            if (DataSource.stationList.Exists(s => s.Id == stationId))
            {
                throw new DuplicateIdException("station already exists\n");
            }
        }
    
    }
}