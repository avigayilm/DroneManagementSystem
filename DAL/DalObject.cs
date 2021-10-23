using System;
using IDAL.DO;


namespace IDAL
{
    namespace DO
    {
        namespace DalObject
        {
            public class DalObject
            {
                public static void addStation(Station Victoria)
                {
                    DataSource.stationList.Add(Victoria);
                    //Victoria.ID = DataSource.Config.stationIndex;
                    //DataSource.StationArr[DataSource.Config.stationIndex] = Victoria;
                    //DataSource.Config.stationIndex++;
                    //return DataSource.Config.stationIndex - 1;
                }

                public static void addDrone(Drone Flyboy)
                {
                    DataSource.dronesList.Add(Flyboy);
                    //Flyboy.ID = DataSource.Config.droneIndex;
                    //DataSource.DronesArr[DataSource.Config.droneIndex] = Flyboy;
                    //DataSource.Config.droneIndex++;
                    //return DataSource.Config.droneIndex - 1;
                }

                public static void addCustomer(Customer me)
                {
                    DataSource.customerList.Add(me);
                    //DataSource.CustomerArr[DataSource.Config.customerIndex] = me;
                    //DataSource.Config.customerIndex++;
                    //return DataSource.Config.customerIndex - 1;
                }

                public static void addParcel(Parcel Fedex)
                {
                    DataSource.parcelList.Add(Fedex);
                    //Fedex.ID = DataSource.Config.parcelIndex;
                    //DataSource.ParcelArr[DataSource.Config.parcelIndex] = Fedex;
                    //DataSource.Config.parcelIndex++;
                    //return DataSource.Config.parcelIndex - 1;
                }

                public static void ParcelDrone(int ParcelId)// we initilized the parcels with empty droneid so don't we need to add a drone id
                {
                    int j,i;
                    for (j = 0; j < DataSource.dronesList.Count && DataSource.dronesList[j].Status != 0; j++) ;
                    for (i = 0; i < DataSource.parcelList.Count && DataSource.parcelList[i].ID != ParcelId; i++) ;// find the index of the parcel
                    Parcel temp = DataSource.parcelList[i];
                    temp.DroneId = DataSource.dronesList[j].ID;
                    DataSource.parcelList[i] = temp;
                }

                public static void ParcelPickedUp(int parcelId, DateTime day)
                {
                    int i,j;
                    for (i = 0; i < DataSource.parcelList.Count && DataSource.parcelList[i].ID != parcelId; i++) ;// find the index of the parcel
                    Parcel temp = DataSource.parcelList[i];
                    temp.PickedUp = day;
                    DataSource.parcelList[i] = temp;
                    int Droneid = DataSource.parcelList[i].DroneId;// finding the Id of the drone delivering the parel
                    for (j = 0; j < DataSource.dronesList.Count && DataSource.dronesList[j].ID != Droneid; j++) ;// find the index of the droneid
                    Drone temp2= DataSource.dronesList[j];
                    temp2.Status = DroneStatuses.delivery;//updating parcel status to delivery
                    DataSource.dronesList[i] = temp2;
                }

                public static void ParcelDelivered(int parcelId, DateTime day)
                {
                    int i,j;
                    for (i = 0; i < DataSource.parcelList.Count && DataSource.parcelList[i].ID != parcelId; i++) ;// find the index of the parcel
                    Parcel temp = DataSource.parcelList[i];
                    temp.PickedUp = day;
                    DataSource.parcelList[i] = temp;
                    int Droneid = DataSource.parcelList[i].DroneId;// finding the Id of the drone delivering the parel
                    for (j = 0; j < DataSource.dronesList.Count && DataSource.dronesList[j].ID != Droneid; j++) ;// find the index of the droneid
                    Drone temp2 = DataSource.dronesList[j];
                    temp2.Status = DroneStatuses.delivery;//updating parcel status to delivery
                    DataSource.dronesList[i] = temp2;
                }

                public static DroneCharge SendToCharge(int DroneId, int StationId)
                {
                    // making a new Dronecharge
                    DroneCharge DC = new DroneCharge();
                    DC.droneId = DroneId;
                    DC.stationId = StationId;
                    int i,j;
                    for (i = 0; i < DataSource.dronesList.Count && DataSource.dronesList[i].ID != DroneId; i++) ;// find the index of the droneid
                    Drone temp = DataSource.dronesList[i];
                    temp.Status = DroneStatuses.maintenance;
                    DataSource.dronesList[i] = temp;
                    for (j = 0; j < DataSource.stationList.Count && DataSource.stationList[j].ID != StationId; j++) ;
                    Station temp2 = DataSource.stationList[j];
                    temp2.ChargeSlots -= 1;
                    DataSource.stationList[i] = temp2;

                    return DC;
                }

                public static void BatteryCharged(DroneCharge Buzzer)
                {
                    int i,j = 0;
                    for (i = 0; i < DataSource.dronesList.Count && DataSource.dronesList[i].ID != Buzzer.droneId; i++) ;
                    Drone temp = DataSource.dronesList[i];
                    temp.Status = DroneStatuses.available;
                    DataSource.dronesList[i] = temp;
                    for (j = 0; j < DataSource.stationList.Count && DataSource.stationList[j].ID != Buzzer.stationId; j++) ;
                    Station temp2 = DataSource.stationList[j];
                    temp2.ChargeSlots += 1;// there is a new chargeslot free
                    DataSource.stationList[i] = temp2;

                }

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

                // Print the list with all the stations
                public static Station[] DisplayStationList()
                {
                    return DataSource.StationArr;
                }

                // Display the list with all the customers
                public static Customer[] DisplayCustomerList()
                {
                    return DataSource.CustomerArr;
                }

                public static Drone[] DisplayDroneList()
                {
                    return DataSource.DronesArr;
                }

                public static Parcel[] DisplayParcelList()// Display all the parcels in the array
                {
                    return DataSource.ParcelArr;
                }

                // print the parcels that have not been assigned to a drone
                public static Parcel[] DisplayvacantParcel()
                {
                    int counter = 0, size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                        if (DataSource.ParcelArr[i].DroneId == 0)// if the parcel was not assigned yet to a drone
                            counter++;
                    Parcel[] newList = new Parcel[counter];
                    for (int i = 0, j = 0; i <= size; j++)
                    {
                        if (DataSource.ParcelArr[j].DroneId == 0)
                        {
                            newList[i] = new Parcel();
                            newList[i] = DataSource.ParcelArr[j];
                            i++;
                        }
                    }
                    return newList;
                }

                // prints the stations that have availble charging
                public static Station[] StationWithCharging()
                {
                    int counter = 0, size = DataSource.Config.stationIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                        if (DataSource.StationArr[i].ChargeSlots > 0)// if the parcel was not assigned yet to a drone
                            counter++;
                    Station[] newList = new Station[counter];
                    for (int i = 0, j = 0; i <= size; j++)
                    {
                        if (DataSource.StationArr[j].ChargeSlots > 0)
                        {
                            newList[i] = new Station();
                            newList[i] = DataSource.StationArr[j];
                            i++;
                        }
                    }
                    return newList;
                }

                //function receives coordinates 
                public static string DecimalToSexagesimal(double coord, char latOrLot)// funciton receives char to decide wheter it is t=latitude and n=lonitude.
                {
                    char direction;
                    if (latOrLot == 't')// if latitude
                        if (coord >= 0)
                            direction = 'N';
                        else
                        {
                            direction = 'S';
                            coord = coord * -1;
                        }
                    else
                        if (coord >= 0)
                        direction = 'E';
                    else
                    {
                        direction = 'W';
                        coord = coord * -1; 
                    }                    
                    int deg = (int)(coord / 1);
                    int min = (int)((coord % 1) * 60) / 1;
                    double sec = (((coord % 1) * 60) % 1) * 60;
                    const string quote = "\"";
                    string toReturn = deg + "° " + min + $"' " + sec + quote + direction;
                    return toReturn;
                }

                public static double Hav(double radian)
                {
                    return Math.Sin(radian / 2) * Math.Sin(radian / 2);
                }

                public static double Radians(double degree)
                {
                    return degree * Math.PI / 180;
                }

                public static double Haversine(double lon1, double lat1, double lon2, double lat2)
                {
                    const double PI = Math.PI;//receives the value of PI 
                    const int RADIUS = 6371;//earths radius

                    double radLon = Radians(lon2 - lon1);
                    double radLat = Radians(lat2 - lat1);
                    double havd = Hav(radLat) + (Math.Cos(Radians(lat2)) * Math.Cos(Radians(lat1)) * Hav(radLon));
                    double distance = 2 * RADIUS * Math.Asin(havd);
                    return distance;
                }
            }

        }
    }
}
