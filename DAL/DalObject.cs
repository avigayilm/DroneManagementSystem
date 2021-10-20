using System;
using IDAL.DO;
using DalObject;


namespace IDAL
{
    namespace DO
    {
        namespace DalObject
        {
            public class DalObject
            {
                public int addStation(Station Victoria)
                {
                    DataSource.StationArr[DataSource.Config.stationIndex] = Victoria;
                    DataSource.Config.stationIndex++;
                    return DataSource.Config.stationIndex - 1;
                }

                public int addDrone(Drone Flyboy)
                {
                    DataSource.DronesArr[DataSource.Config.droneIndex] = Flyboy;
                    DataSource.Config.droneIndex++;
                    return DataSource.Config.droneIndex - 1;
                }

                public int addCustomer(Customer me)
                {

                    DataSource.CustomerArr[DataSource.Config.customerIndex] = me;
                    DataSource.Config.customerIndex++;
                    return DataSource.Config.customerIndex - 1;
                }

                public int addParcel(Parcel Fedex)
                {
                    DataSource.ParcelArr[DataSource.Config.parcelIndex] = Fedex;
                    DataSource.Config.parcelIndex++;
                    return DataSource.Config.parcelIndex - 1;
                }

                public void ParcelDrone(int ParcelId)
                {
                    int j;
                    for (j = 0; j < DataSource.Config.droneIndex && DataSource.DronesArr[j].Status != 0; j++) ;
                    DataSource.ParcelArr[ParcelId].DroneId = DataSource.DronesArr[j].ID;
                }

                public void ParcelPickedUp(int parcelId, DateTime day)
                {
                    DataSource.ParcelArr[parcelId].PickedUp = day;//updating the DroneId of hte parcel
                    DataSource.DronesArr[DataSource.ParcelArr[parcelId].DroneId].Status = 2;//updating parcel status to delivery
                }

                public void ParcelDelivered(int parcelId, DateTime day)
                {
                    int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    int i;
                    DataSource.ParcelArr[parcelId].Delivered = day;//updating the time of delivery to today
                    DataSource.DronesArr[DataSource.ParcelArr[parcelId].DroneId].Status = 0;//updating parcel status to delivery
                }

                public DroneCharge SendToCharge(int DroneId, int StationId)
                {
                    DroneCharge DC = new DroneCharge();
                    DC.droneId = DroneId;
                    DC.stationId = StationId;

                    // making a new Dronecharge            
                    DataSource.DronesArr[DroneId].Status = 1;//updating the drone to maintenance
                    DataSource.StationArr[StationId].ChargeSlots -= 1;

                    return DC;
                }

                public void BatteryCharged(DroneCharge Buzzer)
                {

                    DataSource.DronesArr[Buzzer.droneId].Status = 0;//updating the DroneId of hte parcel
                    DataSource.StationArr[Buzzer.stationId].ChargeSlots -= 1;

                }

                public string DisplayParcel(int ID)
                {
                    return DataSource.ParcelArr[ID].ToString();
                }

                public string DisplayCustomer(string ID)
                {
                    int j;
                    for (j = 0; j < DataSource.Config.customerIndex && DataSource.CustomerArr[j].ID != ID; j++) ;
                    return DataSource.CustomerArr[j].ToString();
                }

                public string DisplayDrone(int ID)
                {
                    return DataSource.DronesArr[ID].ToString();
                }

                public string DisplayStation(int ID)
                {
                    return DataSource.StationArr[ID].ToString();
                }

                public void DisplayStationList()// Print the list with all the stations
                {
                    int size = DataSource.Config.stationIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                        Console.WriteLine(DataSource.StationArr[i].ToString() + "\n");
                }

                public void DisplayCustomerList()// Display the list with all the customers
                {
                    int size = DataSource.Config.customerIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                        Console.WriteLine(DataSource.CustomerArr[i].ToString() + "\n");
                }

                public void DisplayParcelList()// Display all the parcels in the array
                {
                    int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                        Console.WriteLine(DataSource.ParcelArr[i].ToString() + "\n");
                }

                public void DisplayvacantParcel()// print the parcels that have not been assigned to a drone
                {
                    int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                        if (DataSource.ParcelArr[i].DroneId == 0)// if the parcel was not assigned yet to a drone
                            Console.WriteLine(DataSource.ParcelArr[i].ToString() + "\n");
                }

                public void StationWithCharging()// prints the stations that have availble charging
                {
                    int size = DataSource.Config.stationIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.StationArr[i].ChargeSlots != 0)// if there are chargeslots in the station
                            Console.WriteLine(DataSource.ParcelArr[i].ToString() + "\n");
                    }
                }

                //function receives coordinates 
                public string DecimalToSexagesimal(double coord, char latOrLot)
                {
                    char direction;
                    if (latOrLot == 't')
                        if (coord >= 0)
                            direction = 'N';
                        else
                            direction = 'S';
                    else
                        if (coord >= 0)
                        direction = 'E';
                    else
                        direction = 'W';

                    coord = coord * -1;
                    int deg = (int)(coord / 1);
                    int min = (int)((coord % 1) * 60) / 1;
                    double sec = (((coord % 1) * 60) % 1) * 60;
                    const string quote = "\"";
                    string toReturn = deg + "° " + min + $"' " + sec + quote + direction + "\n";
                    return toReturn;
                }

            }



        }
    }
}
