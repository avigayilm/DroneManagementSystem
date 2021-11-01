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
                public static void AddStation(Station Victoria)
                {
                    DataSource.stationList.Add(Victoria);
                }

                public static void AddDrone(Drone Flyboy)
                {
                    DataSource.dronesList.Add(Flyboy);
                }

                public static void AddCustomer(Customer me)
                {
                    DataSource.customerList.Add(me);
                }

                public static void AddParcel(Parcel Fedex)
                {
                    Fedex.ID = ++DataSource.PackageCounter;
                    DataSource.parcelList.Add(Fedex);
                }

                public static void ParcelDrone(int ParcelId,int droneId)// we initilized the parcels with empty droneid so don't we need to add a drone id
                {

                    DataSource.parcelList.ForEach(p => { if (p.ID == ParcelId) { p.DroneId = droneId; } p.requested = DateTime.Now; } );// looking for an avialable drone and setting the Id of that drone, to be the DroneId of hte parcel
                }
                
                
                public static void ParcelPickedUp(int parcelId, DateTime day)
                {
  
                    DataSource.parcelList.ForEach(p => { if (p.ID == parcelId) { DataSource.dronesList.ForEach(d => { if (d.ID == p.DroneId && d.Status == DroneStatuses.available) d.Status = DroneStatuses.delivery; }); p.PickedUp = DateTime.Now; } });// checking if the drone is still available and then change the status of the drone to delivery
                }

                public static void ParcelDelivered(int parcelId, DateTime day)//when the parcel is delivered, the drone will be available again
                {
                    DataSource.parcelList.ForEach(p => { if (p.ID == parcelId) {ChangeStatus(p.DroneId,DroneStatuses.available); p.requested = DateTime.Now; } });
                }
                
                // function that changes the status of the drone according to the given parameter.
                public static void ChangeStatus(int DroneId,DroneStatuses st) 
                {
                    DataSource.dronesList.ForEach(d => { if (d.ID == DroneId) d.Status = st; });
                }

                public static void ChangeChargeSlots(int stationId,int n)
                {
                    DataSource.stationList.ForEach(s => { if (s.ID == stationId) { s.ChargeSlots += n; } });
                }

                // sending a drone to charge in a station, adding the drone to the dronechargelist
                public static void SendToCharge(int DroneId, int StationId)
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
                public static void BatteryCharged(DroneCharge Buzzer)
                {
                    ChangeStatus(Buzzer.droneId, DroneStatuses.available);
                    ChangeChargeSlots(Buzzer.stationId, 1);
                    DataSource.chargeList.ForEach(c => { if (c.droneId == Buzzer.droneId) { DataSource.chargeList.Remove(c); } });

                }

                // The display functions return a string with all the information of the lists
                public static string DisplayParcel(int ID)
                {
                    int i;
                    for (i = 0; i < DataSource.parcelList.Count && DataSource.parcelList[i].ID != ID; i++) ;
                    return DataSource.parcelList[i].ToString();
                }

                public static string DisplayCustomer(string ID)
                {
                    int i;
                    for (i = 0; i < DataSource.customerList.Count && DataSource.customerList[i].ID != ID; i++) ;
                    return DataSource.customerList[i].ToString();
                }

                public static string DisplayDrone(int ID)
                {
                    int i;
                    for (i = 0; i < DataSource.dronesList.Count && DataSource.dronesList[i].ID != ID; i++) ;
                    return DataSource.dronesList[i].ToString();
                }

                public static string DisplayStation(int ID)
                {
                    int i;
                    for (i = 0; i < DataSource.stationList.Count && DataSource.stationList[i].ID != ID; i++) ;
                    return DataSource.stationList[i].ToString();
                }

                // The Display list funcitons return the whole list
                public static List<Station> DisplayStationList()
                {
                    
                    return DataSource.stationList;
                } 

                
                public static List<Customer> DisplayCustomerList()
                {
                    return DataSource.customerList;
                }

                public static List<Drone> DisplayDroneList()
                {
                    return DataSource.dronesList;
                }

                public static List<Parcel> DisplayParcelList()
                {
                    return DataSource.parcelList;
                }

                public static List<DroneCharge> DisplayDroneChargeList()// Display all the parcels in the array
                {
                    return DataSource.chargeList;
                }

                // returns a list with parcels that have not been assigned to a drone
                public static List<Parcel> DisplayvacantParcel()
                {
                   
                    List<Parcel> temp = new List<Parcel>();
                    DataSource.parcelList.ForEach(p => { if (p.DroneId == 0) { temp.Add(p); } });
                    return temp;
                }

                // returns the list with the stations that have availble charging
                public static List<Station> DisplayStationWithCharging()
                {
                    List<Station> temp = new List<Station>();

                    DataSource.stationList.ForEach(p => { if (p.ChargeSlots > 0) { temp.Add(p); } });
                    return temp;
                }

                //function receives coordinates 
                public static string DecimalToSexagesimal(double coord, char latOrLot)
                {
                    char direction;// funciton receives char to decide wheter it is t=latitude and n=longitude
                    if (latOrLot == 't')// if latitude
                        if (coord >= 0)//determines how many minutse norht or south 0 is the equator  larger then 0 is north smaller is south
                            direction = 'E';
                        else
                        {
                            direction = 'W';
                            coord *= -1;
                        }
                    else//if longitude
                        if (coord >= 0) //determines how many minutse east or west, 0 is Grinwich  larger then 0 is east smaller is west
                         direction = 'N';
                    else
                    {
                        direction = 'S';
                        coord *= -1; 
                    }  
                    //determines the various sexagesimal factors
                    int deg = (int)coord;
                    double minWDec = (coord - deg) * 60;
                    int min = (int)minWDec;
                    double decSec = (minWDec - min) * 60;
                    double sec = Math.Round(decSec, 4, MidpointRounding.AwayFromZero);
                    const string quote = "\"  ";
                    string toReturn = deg + "° " + min + $"' " + sec + quote + direction;
                    return toReturn;
                }

                // computes half a versine of the angle
                public static double Hav(double radian)
                {
                    return Math.Sin(radian / 2) * Math.Sin(radian / 2); 
                }

                //returns an angle in radians
                public static double Radians(double degree)
                {
                    return degree * Math.PI / 180;
                }

                //receiving 2 points the haversine formula returns the distance (in km) between the 2
                public static double Haversine(double lon1, double lat1, double lon2, double lat2)
                {
                    const int RADIUS = 6371;//earths radius in KM

                    double radLon = Radians(lon2 - lon1);//converts differance btween the points to radians
                    double radLat = Radians(lat2 - lat1);
                    double havd = Hav(radLat) + (Math.Cos(Radians(lat2)) * Math.Cos(Radians(lat1)) * Hav(radLon));//haversine formula determines the spherical distance between the two points using given versine
                    double distance = 2 * RADIUS * Math.Asin(havd);
                    return distance;
                }
                //function determines the distance between a point and station/customer
                public static double Distance(int ID, double lonP, double latP)
                {
                    if (ID > 9999)//if its a customer
                        foreach (Customer cus in DataSource.customerList) { if (int.Parse(cus.ID) == ID) return Haversine(lonP, latP, cus.longitude, cus.latitude); }

                    // DataSource.customerList.ForEach(c => { if (int.Parse(c.ID) == ID) { return Haversine(lonP, latP, c.longitude, c.latitude); });//returns in a string the distnace between the  customer and given point                   
                    else//its a station
                        //DataSource.stationsList.ForEach(s => { if (s.ID == ID) { return Haversine(lonP, latP, s.longitude, s.latitude); });//returns in a string the distnace between the  station and given point                   
                        foreach (Station KingsX in DataSource.stationList)  { if (KingsX.ID == ID) return Haversine(lonP, latP, KingsX.Longitude, KingsX.Latitude); }
                    return 0.0;// default return
                }
            }
        }
    }
}
   
