using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DalApi;
using DO;

namespace Dal
{
    internal sealed partial class DalXml
    {
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void CheckDuplicateStation(int stationId)
        {
            loadingToList(ref stations, StationXml);
            if (stations.Exists(s => s.Id == stationId && !s.Deleted))
            {
                throw new DuplicateIdException("station already exists\n");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station stat)
        {
            CheckDuplicateStation(stat.Id);
            stations.Add(stat);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ChangeChargeSlots(int stationId, int n)
        {
            try
            {
                int index = CheckExistingStation(stationId);
                var temp = stations[index];
                
                temp.AvailableChargeSlots += n;
                if (temp.AvailableChargeSlots < 0)
                    throw new ArgumentException("number of slots cannot be smaller than 0");
                stations[index] = temp;
                XMLTools.SaveListToXMLSerializer(stations, StationXml);
            }
            catch (MissingIdException ex)
            {
                if(n>=0)
                throw ex;
            }  
            
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationId)
        {
            int index = CheckExistingStation(stationId);
            return stations[index];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            loadingToList(ref stations, StationXml);
            return stations.FindAll(s => predicate == null ? true : predicate(s) && !s.Deleted);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station SmallestDistanceStation(string cusId)
        {
            Customer temp = GetCustomer(cusId);
            double minDistance = double.PositiveInfinity;//starting with unlimited
            double distancekm;
            int index = -1;
            loadingToList(ref stations, StationXml);
            IEnumerator<Station> iter = stations.GetEnumerator();
            for (int i = 0; i < stations.Count; i++)
            {
                distancekm = Bonus.Haversine(stations[i].Longitude, stations[i].Latitude, temp.Longitude, temp.Latitude);
                if (distancekm < minDistance && !stations[i].Deleted )
                {
                    minDistance = distancekm;
                    index = i;// the new index is the index with the smallest distance
                }
            }
            return stations[index];
            // returns the station with the smallest distance to customer
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(int stationId, string name, int chargeSlots)
        {
            int index = CheckExistingStation(stationId);
            Station tempStation = stations[index];
            if (name != null)
                tempStation.Name = name;
            if (chargeSlots != -1)// chekc if chargeslots was enetered
                tempStation.AvailableChargeSlots = chargeSlots; /// input=total chargeslots, we only save the availablechargeslots
            stations[index] = tempStation;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public (int, int) AvailableAndOccupiedSlots(int id)
        {
            Station st = GetStation(id);
            loadingToList(ref droneCharges, DroneChargeXml);
            return (droneCharges.Where(s => s.StationId == st.Id).Count(), st.AvailableChargeSlots);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public int CheckExistingStation(int stationId)
        {
            loadingToList(ref stations, StationXml);
            int index = stations.FindIndex(s => s.Id == stationId && !s.Deleted);
            if (index == -1)
            {
                throw new MissingIdException("No such Station exists\n");
            }
            return index;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int stationId)
        {
            loadingToList(ref stations, StationXml);
            int sIndex = stations.FindIndex(s => s.Id == stationId);
            if (sIndex == -1)
            {
                throw new MissingIdException("No such station exists\n");
            }
            if (stations[sIndex].Deleted)
            {
                throw new MissingIdException($"This Parcel:{ stationId } is deleted \n");
            }
            Station tempStation = stations[sIndex];
            tempStation.Deleted = true;
            stations[sIndex] = tempStation;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

    }
}
