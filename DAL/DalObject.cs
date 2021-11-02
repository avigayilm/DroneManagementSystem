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
        /// 
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

        public void ParcelDrone(int parcelId, int droneId)// we initilized the parcels with empty droneid so don't we need to add a drone id
        {
            // looking for an avialable drone and setting the Id of that drone, to be the DroneId of hte parcel
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.id == parcelId);
            
            var temp = DataSource.parcelList[parcelIndex];
            temp.droneId = droneId;
            temp.requested = DateTime.Now;
            DataSource.parcelList[parcelIndex] = temp;
        }

        public void ParcelPickedUp(int parcelId)
        {
            int parcelIndex = DataSource.parcelList.FindIndex(p => p.id == parcelId);
            int droneIndex = DataSource.dronesList.FindIndex(d => d.id == DataSource.parcelList[parcelIndex].droneId);
            var temp2 = DataSource.dronesList[droneIndex];
            var temp = DataSource.parcelList[parcelIndex];
            temp2.status = DroneStatuses.Delivery;
            temp.pickedUp = DateTime.Now;
            DataSource.dronesList[droneIndex] = temp2;
            DataSource.parcelList[parcelIndex] = temp;
        }

        //changeee!!! cannot have for each
        public void ParcelDelivered(int parcelId, DateTime day)//when the parcel is delivered, the drone will be available again
        {
            DataSource.parcelList.ForEach(p => { if (p.id == parcelId) { ChangeDroneStatus(p.droneId, DroneStatuses.Available); p.requested = DateTime.Now; } });
        }

        // function that changes the status of the drone according to the given parameter.
        /// <summary>
        /// no for eachhhh
        /// </summary>
        /// <param name="DroneId"></param>
        /// <param name="st"></param>
        public void ChangeDroneStatus(int DroneId, DroneStatuses st)
        {
            DataSource.dronesList.ForEach(d => { if (d.id == DroneId) d.status = st; });
        }

        public void ChangeChargeSlots(int stationId, int n)
        {
            DataSource.stationList.ForEach(s => { if (s.ID == stationId) { s.ChargeSlots += n; } });
        }

        // sending a drone to charge in a station, adding the drone to the dronechargelist
        // seperate the function that are in the send to charge
        public void SendToCharge(int DroneId, int StationId)
        {
            //// making a new Dronecharge
            DroneCharge DC = new DroneCharge();
            DC.droneId = DroneId;
            DC.stationId = StationId;
            ChangeDroneStatus(DroneId, DroneStatuses.maintenance);
            ChangeChargeSlots(StationId, -1);
            DataSource.chargeList.Add(DC);
        }

        //Once the drone is charged release the drone from the station, update the chargeslots, and remove the drone from the dronechargelist.
        public void BatteryCharged(DroneCharge Buzzer)///should be seperated to fuctions each functin for each ישות
        {
            ChangeDroneStatus(Buzzer.droneId, DroneStatuses.Available);
            ChangeChargeSlots(Buzzer.stationId, 1);
            DataSource.chargeList.ForEach(c => { if (c.droneId == Buzzer.droneId) { DataSource.chargeList.Remove(c); } });

        }

        // The display functions return a string with all the information of the lists
        //all teh displays sho9uld realy just return the mofa itself like in customer
        public Parcel GetParcel(int ID)
        {
            return DataSource.parcelList.Find(p => p.id == ID);
        }
    

        public Customer GetCustomer(string ID)
        {
            return DataSource.customerList.Find(c => c.ID == ID); ;
        }

        public string GetDrone(int ID)
        {
            int i;
            for (i = 0; i < DataSource.dronesList.Count && DataSource.dronesList[i].id != ID; i++) ;
            return DataSource.dronesList[i].ToString();
        }

        public Station GetStation(int ID)
        {
            return DataSource.stationList.Find(s => s.ID == ID);
        }

        // The Display list funcitons return the whole list
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
            DataSource.stationList.ForEach(p => { if (p.ChargeSlots > 0) { temp.Add(p); } });
            return temp;
        }

    }
}


