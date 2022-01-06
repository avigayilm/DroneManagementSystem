using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace Dal
{
    internal sealed partial class DalXml
    {

        public void CheckDuplicateStation(int stationId)
        {
            //List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            loadingToList(ref stations, StationXml);
            if (DataSource.stationList.Exists(s => s.Id == stationId))
            {
                throw new DuplicateIdException("station already exists\n");
            }
        }

        public void AddStation(Station stat)
        {
            //List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //if (stations.Exists(s => s.Id == stat.Id))
            //{
            //    throw new DuplicateIdException("station already exists\n");
            //}
            //loadingToList(ref stations, StationXml);
            CheckDuplicateStation(stat.Id);
            stations.Add(stat);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        /// <summary>
        /// funciton that changes the amount of chargeslots in a station according to the given parameter.
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="n"></param>
        public void ChangeChargeSlots(int stationId, int n)
        {
            //List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //int index = stations.FindIndex(s=> s.Id == stationId);
            //if (index==-1)
            //{
            //    throw new MissingIdException("station doesn't exists\n");
            //}
            int index = CheckExistingStation(stationId);
            var temp = stations[index];
            temp.AvailableChargeSlots += n;
            stations[index] = temp;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);

        }
        /// <summary>
        /// returns a station according to the given Id
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station GetStation(int stationId)
        {
            //List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //int index = stations.FindIndex(s => s.Id == stationId);
            //if (index == -1)
            //{
            //    throw new MissingIdException("No such Drone exists\n");
            //
            //}
            int index = CheckExistingStation(stationId);
            return stations[index];
        }

        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            loadingToList(ref stations, StationXml);
            return stations.FindAll(s => predicate == null ? true : predicate(s));
                //XMLTools.LoadListFromXMLSerializer<Station>(StationXml).FindAll(d => predicate == null ? true : predicate(d));
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
            //List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            loadingToList(ref stations, StationXml);
            IEnumerator<Station> iter = stations.GetEnumerator();
            for (int i = 0; i < stations.Count; i++)
            {
                distancekm = Bonus.Haversine(stations[i].Longitude, stations[i].Latitude, temp.Longitude, temp.Latitude);
                if (distancekm < minDistance)
                {
                    minDistance = distancekm;
                    index = i;// the new index is the index with the smallest distance
                }
            }
            return stations[index];
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
            //List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //int index = stations.FindIndex(s => s.Id == stationId);
            //if (index == -1)
            //{
            //    throw new MissingIdException("No such Customer exists\n");
            //}
            int index = CheckExistingStation(stationId);
            Station tempStation = stations[index];
            if (name != null)
                tempStation.Name = name;
            if (chargeSlots != -1)// chekc if chargeslots was enetered
                tempStation.AvailableChargeSlots = chargeSlots; /*AvailableAndOccupiedSlots(stationId).Item2;*/// input=total chargeslots, we only save the availablechargeslots
            stations[index] = tempStation;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }




        public (int, int) AvailableAndOccupiedSlots(int id)
        {
            Station st = GetStation(id);
            loadingToList(ref droneCharges, DroneChargeXml);
            return (droneCharges.Where(s => s.StationId == st.Id).Count(), st.AvailableChargeSlots);
        }


        public int CheckExistingStation(int stationId)
        {
            //List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            loadingToList(ref stations, StationXml);
            int index = stations.FindIndex(s => s.Id == stationId);
            if (index == -1)
            {
                throw new MissingIdException("No such Station exists\n");
            }
            return index;
        }

        //public Station GetStation(int ID)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateStation(int stationId, string name, int chargeSlots)
        //{
        //    throw new NotImplementedException();
        //}

        //public Station SmallestDistanceStation(string cusId)
        //{
        //    throw new NotImplementedException();
        //}

        //public int CheckExistingStation(int stationId)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ChangeChargeSlots(int stationId, int n)
        //{
        //    throw new NotImplementedException();
        //}

        //public (int, int) AvailableAndOccupiedSlots(int id)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
