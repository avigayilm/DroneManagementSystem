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
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            if (DataSource.stationList.Exists(s => s.Id == stationId))
            {
                throw new DuplicateIdException("station already exists\n");
            }
        }

        public void AddStation(Station stat)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            if (stations.Exists(s => s.Id == stat.Id))
            {
                throw new DuplicateIdException("station already exists\n");
            }
            //CheckDuplicateStation(stat.Id);
            stations.Add(stat);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public Station GetStation(int ID)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(int stationId, string name, int chargeSlots)
        {
            throw new NotImplementedException();
        }

        public Station SmallestDistanceStation(string cusId)
        {
            throw new NotImplementedException();
        }

        public int CheckExistingStation(int stationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            throw new NotImplementedException();
        }

        public void ChangeChargeSlots(int stationId, int n)
        {
            throw new NotImplementedException();
        }

        public (int, int) AvailableAndOccupiedSlots(int id)
        {
            throw new NotImplementedException();
        }

    }
}
