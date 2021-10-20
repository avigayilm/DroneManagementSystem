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
                public int addStation(Station Victoria)// sending all the things that are not random, so by drone it doesn't get 2 parameters
                {

                    DataSource.StationArr[DataSource.Config.stationIndex] = new Station()
                    {
                        Name = name
                    };
                    DataSource.Config.stationIndex++;
                    return DataSource.Config.stationIndex - 1;



                }

                public int addDrone()
                {
                    DataSource.DronesArr[DataSource.Config.droneIndex] = new Drone();
                    DataSource.Config.droneIndex++;
                    return DataSource.Config.droneIndex - 1;


                }

                public void addCustomer(string name, )
                {
                    DataSource.CustomerArr[DataSource.Config.customerIndex] = new Customer();
                    DataSource.Config.customerIndex++;

                }

                public int addParcel(Parcel Fedex)
                {
                    DataSource.ParcelArr[DataSource.Config.parcelIndex] = Fedex;
                    DataSource.Config.parcelIndex++;
                    return DataSource.Config.parcelIndex - 1;// return to what index you added the parcel

                }

                public void ParcelDrone(int ParcelId, int DroneId)
                {
                    int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    int index = -1;
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.ParcelArr[i].ID == ParcelId)// if the parcel was found
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1)// if the value is not found
                        Console.WriteLine("Parcel not found");
                    //update the parcel values
                    else
                    {
                        DataSource.ParcelArr[index].DroneId = DroneId;//updating the DroneId of hte parcel
                    }


                }

                public void ParcelPickedUp(int parcelId, DateTime day)
                {
                    int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    int index = -1;
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.ParcelArr[i].ID == parcelId)// if the parcel was found
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1)// if the value is not found
                        Console.WriteLine("Parcel not found");
                    //update the parcel values
                    else
                    {
                        DataSource.ParcelArr[index].PickedUp = day;//updating the DroneId of hte parcel
                    }
                }

                public void ParcelDelivered(int parcelId, DateTime day)
                {
                    int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    int index = -1;
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.ParcelArr[i].ID == parcelId)// if the parcel was found
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1)// if the value is not found
                        Console.WriteLine("Parcel not found");
                    //update the parcel values
                    else
                    {
                        DataSource.ParcelArr[index].Delivered = day;//updating the DroneId of hte parcel
                    }
                }

                public DroneCharge SendToCharge(int DroneId, int StationId)
                {
                    int size = DataSource.Config.droneIndex;//getting amount of drones in the array
                    int size2 = DataSource.Config.stationIndex;
                    int index = -1;
                    DroneCharge DC = new DroneCharge();// making a new Dronecharge
                    for (int i = 0; i < size; i++)
                    {
                        if (DataSource.DronesArr[i].ID == DroneId)// if the parcel was found
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1)// if the value is not found
                    {
                        Console.WriteLine("Drone not found");// return a dronecharge
                        return DC;
                    }


                    //update the drone values
                    else
                    {
                        //   DataSource.DronesArr[index].Status= ;//updating the DroneId of hte parcel

                    }

                    index = -1;
                    for (int i = 0; i < size2; i++)
                    {
                        if (DataSource.StationArr[i].ID == StationId)// if the station was found
                        {
                            index = i;
                            break;
                        }
                    }

                    if (index == -1)// if the value is not found
                    {
                        Console.WriteLine("Station not found");// return a dronecharge
                        return DC;
                    }
                    else
                    {
                        DataSource.StationArr[index].ChargeSlots -= 1;
                    }

                    DC.DroneId = DroneId;
                    DC.StationId = StationId;

                    return DC;

                }

                public void BatteryCharged(DroneCharge Buzzer)
                {
                    int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    int index = -1;
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.DronesArr[i].ID == Buzzer.DroneId)// if the parcel was found
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1)// if the value is not found
                        Console.WriteLine("Parcel not found");
                    //update the parcel values
                    else
                    {
                        //   DataSource.DronesArr[index].Status= ;//updating the DroneId of hte parcel

                    }
                }

                public void DisplayDrone(int ID)
                {
                    int size = DataSource.Config.droneIndex;//getting amount of parcels in the array
                    int index = -1;
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.DronesArr[i].ID == ID)// if the parcel was found
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1)// if the value is not found
                        Console.WriteLine("Drone not found");
                    string result = DataSource.DronesArr[index].ToString();
                    Console.WriteLine(result);
                }

                public void DisplayStation(int ID)// Print the station fitting to the given ID
                {
                    int size = DataSource.Config.stationIndex;//getting amount of parcels in the array
                    int index = -1;
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.StationArr[i].ID == ID)// if the parcel was found
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1)// if the value is not found
                        Console.WriteLine("Station not found");
                    string result = DataSource.StationArr[index].ToString();
                    Console.WriteLine(result);
                }

                public string DisplayParcel(int ID)// Print the Parcel fitting to the given ID
                {

                    return DataSource.ParcelArr[ID].ToString();

                    int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    int index = -1;
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.ParcelArr[i].ID == ID)// if the parcel was found
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1)// if the value is not found
                        Console.WriteLine("Parcel not found");
                    string result = DataSource.ParcelArr[index].ToString();
                    Console.WriteLine(result);
                }

                public void DisplayCustomer(int ID)// Print the customer fitting the given ID
                {
                    int size = DataSource.Config.customerIndex;//getting amount of parcels in the array
                    int index = -1;
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.CustomerArr[i].ID == ID)// if the parcel was found
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1)// if the value is not found
                        Console.WriteLine("Customer not found");
                    string result = DataSource.CustomerArr[index].ToString();// Put all the information in a string
                    Console.WriteLine(result);// print the string
                }

                public void DisplayStationList()// Print the list with all the stations
                {
                    int size = DataSource.Config.stationIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                    {
                        string result = DataSource.StationArr[i].ToString();
                        Console.WriteLine(result);
                        Console.WriteLine("\n");
                    }

                }

                public void DisplayCustomerList()// Display the list with all the customers
                {
                    int size = DataSource.Config.customerIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                    {
                        string result = DataSource.CustomerArr[i].ToString();
                        Console.WriteLine(result);
                        Console.WriteLine("\n");
                    }

                }

                public void DisplayParcelList()// Display all the parcels in the array
                {
                    int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                    {
                        string result = DataSource.ParcelArr[i].ToString();
                        Console.WriteLine(result);
                        Console.WriteLine("\n");
                    }
                }

                public void DisplayvacantParcel()// print the parcels that have not been assigned to a drone
                {
                    int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.ParcelArr[i].DroneId == 0)// if the parcel was not assigned yet to a drone
                        {
                            string result = DataSource.ParcelArr[i].ToString();
                            Console.WriteLine(result);
                            Console.WriteLine("\n");
                        }
                    }
                }

                public void StationwithCharging()// prints the stations that have availble charging
                {
                    int size = DataSource.Config.stationIndex;//getting amount of parcels in the array
                    for (int i = 0; i <= size; i++)
                    {
                        if (DataSource.StationArr[i].ChargeSlots != 0)// if there are chargeslots in the station
                        {
                            string result = DataSource.ParcelArr[i].ToString();
                            Console.WriteLine(result);
                            Console.WriteLine("\n");
                        }
                    }
                }

            }





    }


}

