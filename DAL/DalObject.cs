using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DalObject
    {
        /// <summary>
        /// 
        /// </summary>
        public DalObject() => DataSource.Initialize();

        /// <summary>
        /// adding the elements to the lists
        /// </summary>
        /// <param name="stat"></param>
        public void AddStation(Station stat)
        {
            DataSource.stationList.Add(stat);
        }

        public void AddDrone(Drone dro)
        {
            DataSource.dronesList.Add(dro);
        }

        public void AddCustomer(Customer cus)
        {
            DataSource.customerList.Add(cus);
        }

        public void AddParcel(Parcel pack)
        {
            pack.id = ++DataSource.Config.LastParcelNumber;
            DataSource.parcelList.Add(pack);
        }
        /// <summary>
        /// assigning a drone to a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void ParcelDrone(int parcelId, int droneId)// we initilized the parcels with empty droneid so don't we need to add a drone id
        {
            // looking for an avialable drone and setting the Id of that drone, to be the DroneId of hte parcel
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.id == parcelId);
            if (parcelIndex >= 0)
            {
                var temp = DataSource.parcelList[parcelIndex];
                temp.droneId = droneId;
                temp.requested = DateTime.Now;
                DataSource.parcelList[parcelIndex] = temp;
            }
        }

        /// <summary>
        /// the drone picks up the parcel, therefore updating the drone's status to delivery 
        /// and updating the picked up time
        /// </summary>
        /// <param name="parcelId"></param>
        public void ParcelPickedUp(int parcelId)
        {
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.id == parcelId);
            if (parcelIndex >= 0)// check if it's a valid indez
            {
                int droneIndex = DataSource.dronesList.FindIndex(d => d.id == DataSource.parcelList[parcelIndex].droneId);
                if (droneIndex >= 0)
                {
                    var temp2 = DataSource.dronesList[droneIndex];
                    temp2.status = DroneStatuses.Delivery;
                    var temp = DataSource.parcelList[parcelIndex];
                    temp.pickedUp = DateTime.Now;
                    DataSource.dronesList[droneIndex] = temp2;
                    DataSource.parcelList[parcelIndex] = temp;
                }
            }
        }

        /// <summary>
        ///  funciton updated the parcel to delivered. It changes the drone's status to available
        ///  and updates the time of requested
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="day"></param>
        public void ParcelDelivered(int parcelId, DateTime day)//when the parcel is delivered, the drone will be available again
        {
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.id == parcelId);// finding the index of the parcel
            if (parcelIndex >= 0)
            {
                int droneIndex = DataSource.dronesList.FindIndex(d => d.id == DataSource.parcelList[parcelIndex].droneId);
                if (droneIndex >= 0)
                {
                    var temp2 = DataSource.dronesList[droneIndex];
                    var temp = DataSource.parcelList[parcelIndex];
                    temp2.status = DroneStatuses.Available;
                    temp.requested = DateTime.Now;
                    DataSource.dronesList[droneIndex] = temp2;
                    DataSource.parcelList[parcelIndex] = temp;
                }
            }
        }

        /// <summary>
        /// function that changes the status of the drone according to the given parameter.
        /// </summary>
        /// <param name="DroneId"></param>
        /// <param name="st"></param>
        public void ChangeDroneStatus(int DroneId, DroneStatuses st)
        {
            int droneIndex = DataSource.dronesList.FindIndex(d => d.id == DroneId);
            if (droneIndex >= 0)
            {
                var temp = DataSource.dronesList[droneIndex];
                temp.status = st;
                DataSource.dronesList[droneIndex] = temp;
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
            if (stationIndex >= 0)
            {
                var temp = DataSource.stationList[stationIndex];
                temp.chargeSlots += n;
                DataSource.stationList[stationIndex] = temp;
            }
        }

        /// <summary>
        /// sending a drone to charge in a station, adding the drone to the dronechargelist
        /// </summary>
        /// <param name="DroneId"></param>
        /// <param name="StationId"></param>
        public void SendToCharge(int droneId, int stationId)
        {
            //// making a new Dronecharge
            DroneCharge DC = new DroneCharge();
            DC.droneId = droneId;
            DC.stationId = stationId;
            DataSource.chargeList.Add(DC);
        }
        /// <summary>
        /// Once the drone is charged release the drone from the station, update the chargeslots, and remove the drone from the dronechargelist.
        /// </summary>
        /// <param name="Buzzer"></param>
        public void BatteryCharged(int droneId, int stationId)
        {
            int droneIndex = DataSource.chargeList.FindIndex(d => d.droneId == droneId);// find the index of the dronecharge according to teh droneIndex
            if (droneIndex >= 0)
            {
                DataSource.chargeList.Remove(DataSource.chargeList[droneIndex]);// removing the drone from the chargelist
                var temp = DataSource.dronesList[droneIndex];
                temp.battery = 100.00;// battery is full
                DataSource.dronesList[droneIndex] = temp;
            }

        }

        /// <summary>
        /// The get functions return a string with all the information of the lists
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Parcel GetParcel(int ID)
        {
            return DataSource.parcelList.Find(p => p.id == ID);
        }
    

        public Customer GetCustomer(string ID)
        {
            return DataSource.customerList.Find(c => c.id == ID);
        }

        public Drone GetDrone(int ID)
        {
            return DataSource.dronesList.Find(d => d.id == ID);
        }

        public Station GetStation(int ID)
        {
            return DataSource.stationList.Find(s => s.id == ID);
        }
        /// <summary>
        /// The Display list funcitons return the whole list
        /// </summary>
        /// <returns></returns>
        public List<Station> DisplayStationList()
        {
            List<Station> list = new();
            DataSource.stationList.ForEach(s => list.Add(s));
            return list;
        }


        public List<Customer> GetAllCustomers()
        {
            List<Customer> list = new();
            DataSource.customerList.ForEach(c => list.Add(c));
            return list;
        }

        public List<Drone> GetDroneList()
        {
            List<Drone> list = new();
            DataSource.dronesList.ForEach(d => list.Add(d));
            return list;
        }

        public List<Parcel> GetParcelList()
        {
            List<Parcel> list = new();
            DataSource.parcelList.ForEach(p => list.Add(p));
            return list;
        }

        public List<DroneCharge> GetDroneChargeList()// Display all the parcels in the array
        {
            List<DroneCharge> list = new();
            DataSource.chargeList.ForEach(dc => list.Add(dc));
            return list;
        }

        // returns a list with parcels that have not been assigned to a drone
        public List<Parcel> GetvacantParcel()
        {
            List<Parcel> temp = new();
            DataSource.parcelList.ForEach(p => { if (p.droneId == 0) temp.Add(p);  });
            return temp;
        }

        // returns the list with the stations that have availble charging
        public List<Station> GetStationWithCharging()
        {
            List<Station> temp = new();
            DataSource.stationList.ForEach(p => { if (p.chargeSlots > 0) { temp.Add(p); } });
            return temp;
        }

    }
}


