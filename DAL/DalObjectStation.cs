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
        /// adding the elements to the lists
        /// </summary>
        /// <param name="stat"></param>
        public void AddStation(Station stat)
        {
            int index = DataSource.parcelList.FindIndex(s => s.id == stat.id);
            if (index != -1)
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
            int stationIndex = DataSource.stationList.FindIndex(s => s.id == stationId);
            if (stationIndex == -1)
                throw new MissingIdException("No such station exists in list\n");
            else
            {
                var temp = DataSource.stationList[stationIndex];
                temp.chargeSlots += n;
                DataSource.stationList[stationIndex] = temp;
            }
        }

        public Station GetStation(int ID)
        {

            int index = DataSource.stationList.FindIndex(s => s.id == ID);
            if (index == -1)
            {
                throw new MissingIdException("No such station\n");
            }
            else
            {
                return DataSource.stationList[index];
            }
            // return DataSource.stationList.Find(s => s.id == ID);
        }

        /// <summary>
        /// The Display list funcitons return the whole list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetAllStations()
        {
            List<Station> list = new();
            DataSource.stationList.ForEach(s => list.Add(s));
            return (IEnumerable<Station>)list;
        }

        // returns the list with the stations that have availble charging
        public IEnumerable<Station> GetStationWithCharging()
        {
            List<Station> temp = new();
            DataSource.stationList.ForEach(p => { if (p.chargeSlots > 0) { temp.Add(p); } });
            return (IEnumerable<Station>)temp;
        }

        public Station smallestDistanceStation(string cusId)
        {
            Customer temp = GetCustomer(cusId);
            double minDistance = double.PositiveInfinity;//starting with unlimited
            double distancekm;
            int index = -1;
            IEnumerator<Station> iter = DataSource.stationList.GetEnumerator();
            for (int i = 0; i < DataSource.stationList.Count; i++)
            {
                distancekm = Bonus.Haversine(DataSource.stationList[i].longitude, DataSource.stationList[i].latitude, temp.longitude, temp.latitude);
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

    }
}