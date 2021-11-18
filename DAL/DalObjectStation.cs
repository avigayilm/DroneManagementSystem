using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;


namespace DAL
{
    public partial class DalObject
    {

        /// <summary>
        /// adding the station to the lists
        /// </summary>
        /// <param name="stat"></param>
        public void AddStation(Station stat)
        {
            bool exists = DataSource.stationList.Exists(s => s.Id == stat.Id);
            if (exists)
            {
                throw new DuplicateIdException("Station already exists in list\n");
            }
            else
            {
                DataSource.stationList.Add(stat);
            }
        }

        /// <summary>
        /// funciton that changes the amount of chargeslots in a station according to the given parameter.
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="n"></param>
        public void ChangeChargeSlots(int stationId, int n)
        {
            int stationIndex = DataSource.stationList.FindIndex(s => s.Id == stationId);
            if (stationIndex == -1)
                throw new MissingIdException("No such station exists in list\n");
            else
            {
                var temp = DataSource.stationList[stationIndex];
                temp.ChargeSlots += n;
                DataSource.stationList[stationIndex] = temp;
            }
        }
        /// <summary>
        /// returns a station according to the given Id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Station GetStation(int ID)
        {

            int index = DataSource.stationList.FindIndex(s => s.Id == ID);
            if (index == -1)
            {
                throw new MissingIdException("No such station\n");
            }
            else
            {
                return DataSource.stationList[index];
            }
        }
        /// <summary>
        ///  returns the list with the stations that have availble charging
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStationWithCharging()
        {
            List<Station> temp = new();
            DataSource.stationList.ForEach(p => { if (p.ChargeSlots > 0) { temp.Add(p); } });
            return (IEnumerable<Station>)temp;
        }

        /// <summary>
        /// returns the whole list of stations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetAllStations()
        {
            List<Station> tempStationList = new();
            DataSource.stationList.ForEach(s => tempStationList.Add(s));
            return (IEnumerable<Station>)tempStationList;
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

        public void UpdateStation(int stationId, string name,int chargeSlots)
        {
            int index = DataSource.stationList.FindIndex(s => s.Id == stationId);
            if (index == -1)
            {
                throw new MissingIdException("No such station\n");
            }
            else
            {
                Station tempStation = DataSource.stationList[index];
                if(name!=("\n"))
                    tempStation.Name = name;
                if (chargeSlots != 10)// chekc if a phone was entere
                    tempStation.ChargeSlots = chargeSlots;
                DataSource.stationList[index] = tempStation;
            }

         
        }
        /// <summary>
        /// returns an array with number of empty slots in index 1 and occupied slots in index 2 in a station
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int[] AvailableAndEmptySlots(int id)
        {
            Station st = GetStation(id);
            int[] slots = new int [2];
            slots[0] = DataSource.chargeList.Where(s => s.StationId == st.Id).Count();
            slots[1] = st.ChargeSlots;
            return slots;
        }

    }
}