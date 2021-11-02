using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace IDAL
{
    namespace DO
    {
        namespace DalObject
        {
            public class DalObject
            {
                public DalObject()// constructor for DalObject
                {
                    DataSource.Initialize();
                }

                public void AddStation(Station Victoria)
                {
                    DataSource.stationList.Add(Victoria);
                }

                public void AddDrone(Drone Flyboy)
                {
                    DataSource.dronesList.Add(Flyboy);
                }

                public void AddCustomer(Customer me)
                {
                    DataSource.customerList.Add(me);
                }

                public void AddParcel(Parcel Fedex)
                {
                    Fedex.ID = ++DataSource.PackageCounter;
                    DataSource.parcelList.Add(Fedex);
                }

                public void ParcelDrone(int ParcelId,int droneId)// we initilized the parcels with empty droneid so don't we need to add a drone id
                {

                    DataSource.parcelList.ForEach(p => { if (p.ID == ParcelId) { p.droneId = droneId; } p.requested = DateTime.Now; } );// looking for an avialable drone and setting the Id of that drone, to be the DroneId of hte parcel
                }
                
                
                public void ParcelPickedUp(int parcelId, DateTime day)
                {
  
                    DataSource.parcelList.ForEach(p => { if (p.ID == parcelId) { DataSource.dronesList.ForEach(d => { if (d.ID == p.droneId && d.Status == DroneStatuses.available) d.Status = DroneStatuses.delivery; }); p.pickedUp = DateTime.Now; } });// checking if the drone is still available and then change the status of the drone to delivery
                }

                public void ParcelDelivered(int parcelId, DateTime day)//when the parcel is delivered, the drone will be available again
                {
                    DataSource.parcelList.ForEach(p => { if (p.ID == parcelId) {ChangeStatus(p.droneId,DroneStatuses.available); p.requested = DateTime.Now; } });
                }
                
                // function that changes the status of the drone according to the given parameter.
                public void ChangeStatus(int DroneId,DroneStatuses st) 
                {
                    DataSource.dronesList.ForEach(d => { if (d.ID == DroneId) d.Status = st; });
                }

                public void ChangeChargeSlots(int stationId,int n)
                {
                    DataSource.stationList.ForEach(s => { if (s.ID == stationId) { s.ChargeSlots += n; } });
                }

                // sending a drone to charge in a station, adding the drone to the dronechargelist
                public void SendToCharge(int DroneId, int StationId)
                {
                    //// making a new Dronecharge
                    DroneCharge DC = new DroneCharge();
                    DC.droneId = DroneId;
                    DC.stationId = StationId;
                    ChangeStatus(DroneId, DroneStatuses.maintenance);
                    ChangeChargeSlots(StationId, -1);
                    DataSource.chargeList.Add(DC);
                }

                //Once the drone is charged release the drone from the station, update the chargeslots, and remove the drone from the dronechargelist.
                public void BatteryCharged(DroneCharge Buzzer)
                {
                    ChangeStatus(Buzzer.droneId, DroneStatuses.available);
                    ChangeChargeSlots(Buzzer.stationId, 1);
                    DataSource.chargeList.ForEach(c => { if (c.droneId == Buzzer.droneId) { DataSource.chargeList.Remove(c); } });

                }

                // The display functions return a string with all the information of the lists
                public string DisplayParcel(int ID)
                {
                    int i;
                    for (i = 0; i < DataSource.parcelList.Count && DataSource.parcelList[i].ID != ID; i++) ;
                    return DataSource.parcelList[i].ToString();
                }

                public  string DisplayCustomer(string ID)
                {
                    int i;
                    for (i = 0; i < DataSource.customerList.Count && DataSource.customerList[i].ID != ID; i++) ;
                    return DataSource.customerList[i].ToString();
                }

                public string DisplayDrone(int ID)
                {
                    int i;
                    for (i = 0; i < DataSource.dronesList.Count && DataSource.dronesList[i].ID != ID; i++) ;
                    return DataSource.dronesList[i].ToString();
                }

                public string DisplayStation(int ID)
                {
                    int i;
                    for (i = 0; i < DataSource.stationList.Count && DataSource.stationList[i].ID != ID; i++) ;
                    return DataSource.stationList[i].ToString();
                }

                // The Display list funcitons return the whole list
                public List<Station> DisplayStationList()
                {
                    
                    return DataSource.stationList;
                } 

                
                public List<Customer> DisplayCustomerList()
                {
                    return DataSource.customerList;
                }

                public List<Drone> DisplayDroneList()
                {
                    return DataSource.dronesList;
                }

                public List<Parcel> DisplayParcelList()
                {
                    return DataSource.parcelList;
                }

                public List<DroneCharge> DisplayDroneChargeList()// Display all the parcels in the array
                {
                    return DataSource.chargeList;
                }

                // returns a list with parcels that have not been assigned to a drone
                public List<Parcel> DisplayvacantParcel()
                {
                   
                    List<Parcel> temp = new List<Parcel>();
                    DataSource.parcelList.ForEach(p => { if (p.droneId == 0) { temp.Add(p); } });
                    return temp;
                }

                // returns the list with the stations that have availble charging
                public List<Station> DisplayStationWithCharging()
                {
                    List<Station> temp = new List<Station>();

                    DataSource.stationList.ForEach(p => { if (p.ChargeSlots > 0) { temp.Add(p); } });
                    return temp;
                }

            }
        }
    }
}
   
