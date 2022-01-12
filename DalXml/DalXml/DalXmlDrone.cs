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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)
        {
            int index = CheckExistingDrone(droneId);
            return drones[index];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            loadingToList(ref drones, DroneXml);
            return drones.FindAll(d => predicate == null ? true : predicate(d) && !d.Deleted);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int droneId, string model)
        {
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
            if (drones.Exists(d => d.Id == droneId && !d.Deleted))
            {
                throw new DuplicateIdException("Drone already exists\n");
            }
        }



        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int droneId)
        {
            loadingToList(ref drones, DroneXml);
            int dIndex = drones.FindIndex(d => d.Id == droneId);
            if (dIndex == -1)
            {
                throw new MissingIdException("No such drone exists\n");
            }
            if (drones[dIndex].Deleted)
            {
                throw new MissingIdException($"This Drone:{ droneId } is deleted \n");
            }
            Drone tempDrone = drones[dIndex];
            tempDrone.Deleted = true;
            drones[dIndex] = tempDrone;
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

    }
}
