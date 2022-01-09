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
        public void AddDrone(Drone dro)
        {
            //List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            //if (drones.Exists(d => d.Id == d.Id))
            //    throw new DuplicateIdException("Drone already exists\n");
            CheckDuplicateDrone(dro.Id);
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)
        {
            //List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            //int index = drones.FindIndex(d => d.Id == droneId);
            //if (index == -1)
            //{
            //    throw new MissingIdException("No such Drone exists\n");
            //}
            int index = CheckExistingDrone(droneId);
            return drones[index];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            loadingToList(ref drones, DroneXml);
            return drones.FindAll(d => predicate == null ? true : predicate(d));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int droneId, string model)
        {
            //List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            //int index = drones.FindIndex(d => d.Id == droneId);
            //if(index==-1)
            //{
            //    throw new MissingIdException("No such Drone exists\n");
            //}
            int index = CheckExistingDrone(droneId);
            Drone tempDrone = drones[index];
            tempDrone.Model = model;
            drones[index] = tempDrone;
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> DronesChargingAtStation(int stationId)
        {
            List<Drone> charging = new List<Drone>();
            loadingToList(ref droneCharges, DroneChargeXml);
            droneCharges.ForEach(d => { if (d.StationId == stationId) charging.Add(GetDrone(d.DroneId)); });
            return charging;
        }

        /// <summary>
        /// checks if a customer exists in the customerlist, if it doesn't it throws a MissingIdException
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        internal int CheckExistingDrone(int droneId)
        {
            //List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            loadingToList(ref drones, DroneXml);
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
        internal void CheckDuplicateDrone(int droneId)
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
