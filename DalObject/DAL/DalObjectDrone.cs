﻿using System;
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
        /// adds a drone to the dronlist
        /// </summary>
        /// <param name="dro"></param>
        public void AddDrone(Drone dro)
        {
            CheckDuplicateDrone(dro.Id);
            DataSource.dronesList.Add(dro);
        }



        /// <summary>
        /// returns the drone according to the given Id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Drone GetDrone(int droneId)
        {
            int index = CheckExistingDrone(droneId);
            return DataSource.dronesList[index];
        }


        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            return DataSource.dronesList.FindAll(d => predicate == null ? true : predicate(d) && !d.Deleted);
        }
        /// <summary>
        /// updates the model of the drone, used in BL
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="model"></param>
        public void UpdateDrone(int droneId, string model)
        {
            int index = CheckExistingDrone(droneId);
            Drone tempDrone = DataSource.dronesList[index];
            tempDrone.Model = model;
            DataSource.dronesList[index] = tempDrone;

        }
        /// <summary>
        /// returns a list of all the droned chrging at a station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public IEnumerable<Drone> DronesChargingAtStation(int stationId)
        {
            List<Drone> charging = new List<Drone>();
            DataSource.chargeList.ForEach(c => { if (c.StationId == stationId) charging.Add(GetDrone(c.DroneId)); });
            return charging;
        }

        /// <summary>
        /// checks if a customer exists in the customerlist, if it doesn't it throws a MissingIdException
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        internal int CheckExistingDrone(int droneId)
        {
            int index = DataSource.dronesList.FindIndex(d => d.Id == droneId);
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
            if (DataSource.dronesList.Exists(d => d.Id == droneId && !d.Deleted))
            {
                throw new DuplicateIdException("Drone already exists\n");
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int droneId)
        {
            int dIndex = DataSource.dronesList.FindIndex(d => d.Id == droneId);
            if (dIndex == -1)
            {
                throw new MissingIdException("No such drone exists\n");
            }
            if (DataSource.dronesList[dIndex].Deleted)
            {
                throw new MissingIdException($"This Drone:{ droneId } is deleted \n");
            }
            Drone tempDrone = DataSource.dronesList[dIndex];
            tempDrone.Deleted = true;
            DataSource.dronesList[dIndex] = tempDrone;
        }
    }
}
