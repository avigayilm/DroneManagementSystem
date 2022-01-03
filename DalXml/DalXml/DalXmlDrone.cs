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

        /// <summary>
        /// adds a drone to the dronlist
        /// </summary>
        /// <param name="dro"></param>
        public void AddDrone(Drone dro)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (drones.Exists(d => d.Id == d.Id))
                throw new DuplicateIdException("Drone already exists\n");
            drones.Add(dro);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        ///// <summary>
        ///// function that changes the status of the drone according to the given parameter.
        ///// </summary>
        ///// <param name="DroneId"></param>
        ///// <param name="st"></param>
        //public void ChangeDroneStatus(int DroneId, DroneStatuses st)
        //{
        //    int droneIndex = DataSource.dronesList.FindIndex(d => d.id == DroneId);
        //    if (droneIndex == -1)
        //        throw new MissingIdException("No such drone\n");
        //    else
        //    {
        //        var temp = DataSource.dronesList[droneIndex];
        //        temp.status = st;
        //        DataSource.dronesList[droneIndex] = temp;
        //    }
        //}

        /// <summary>
        /// returns the drone according to the given Id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Drone GetDrone(int droneId)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int index = drones.FindIndex(d => d.Id == droneId);
            if (index == -1)
            {
                throw new MissingIdException("No such Drone exists\n");
            }
            return drones[index];
        }


        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            return XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml).FindAll(d => predicate == null ? true : predicate(d));
        }
        /// <summary>
        /// updates the model of the drone, used in BL
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        public void UpdateDrone(int droneId, string model)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int index = drones.FindIndex(d => d.Id == droneId);
            if(index==-1)
            {
                throw new MissingIdException("No such Drone exists\n");
            }
            Drone tempDrone = drones[index];
            tempDrone.Model = model;
            drones[index] = tempDrone;
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }
        /// <summary>
        /// returns a list of all the drones charging at a station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public IEnumerable<Drone> DronesChargingAtStation(int stationId)
        {
            List<Drone> charging = new List<Drone>();
            XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml).ForEach(d => { if (d.StationId == stationId) charging.Add(GetDrone(d.DroneId)); });
            return charging;
        }

        /// <summary>
        /// checks if a customer exists in the customerlist, if it doesn't it throws a MissingIdException
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        internal int CheckExistingDrone(int droneId)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int index = drones.FindIndex(d => d.Id == droneId);
            if (index == -1)
            {
                throw new MissingIdException("No such Drone exists\n");
            }
            return index;
        }

        /// <summary>
        /// checks if a customer already exists, if it does it throws a duplicateIdException
        /// </summary>
        /// <param name="customerId"></param>
        public void CheckDuplicateDrone(int droneId)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (drones.Exists(d => d.Id == droneId))
            {
                throw new DuplicateIdException("Drone already exists\n");
            }
        }
        //public void AddDrone(Drone dro)
        //{
        //    throw new NotImplementedException();
        //}

        //public Drone GetDrone(int ID)
        //{
        //    throw new NotImplementedException();
        //}
        //public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateDrone(int droneId, string model)
        //{
        //    throw new NotImplementedException();
        //}


    }
}
