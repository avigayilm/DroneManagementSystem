using System;
using IDAL.DO;
//using DalObject;

namespace DalObject
{
    internal class DataSource
    {
        //string[] prefices = new string[] {"058", "054", "053", "052"}
        
        public static Drone[] DronesArr = new Drone[10];
        public static Station[] StationArr = new IDAL.DO.Station[5];
        public static Customer[] CustomerArr = new Customer[100];
        public static Parcel[] ParcelArr = new Parcel[1000];


        internal class Config
        {
            internal static int droneIndex = 0;
            internal static int stationIndex = 0;
            internal static int customerIndex = 0;
            internal static int parcelIndex = 0;
            // public int runnerID;

        }
        public static void Initialize()
                {
                    Random r = new Random();
                    //int num0   = r.Next(0, 3);
                    //int num4   = r.Next(1000, 9999);   // creates a number between 100 and 9999
                    //int WeightRand   = r.Next(0, 3);
                    //int numW   = r.Next(0.0, 100.0);// creates a number between 0 and 100  
                    //int num1   = r.Next(0, 10);
                    //int numLattitude   = r.Next(-90, 90);
                    //int numLlong  = r.Next(-180, 180);
                   

                    //lop for updete 5 drone
                    for (int i = Config.droneIndex; i < 5; i++)
			           {
                         DataSource.DronesArr[i].ID =num4 ;
                         DataSource.DronesArr[i].Model = ("A" + r.Next(0, 10)) + string(r.Next(100, 1000));
                         DataSource.DronesArr[i].MaxWeight = r.Next(0, 3) ;
                         DataSource.DronesArr[i].Battery = r.Next(0.0, 100.0);;
                         DataSource.DronesArr[i].Status = r.Next(0, 3);
			           }

                    //lop for 2 station
                    for (int i = DataSource.Config.stationIndex, string str ='A' ; i < 2; i++,str++)
			            {
                        DataSource.StationArr[i].ID = r.Next(100, 1000);
                        DataSource.StationArr[i].Name = str;
                        DataSource.StationArr[i].Longitude = r.Next(-180, 180) ;
                        DataSource.StationArr[i].Lattitude = r.Next(-90, 90) ;
                        DataSource.StationArr[i].ChargeSlots = r.Next(1, 10);
			            }

                    //lop for 10 customer
                    for (int i = customerIndex; i < 10; i++)
			        {
                        CustomerArr[i].longitude= r.Next(-180, 180);
                        CustomerArr[i].lattitude= r.Next(-90, 90);
                        CustomerArr[i].Phone = "05" + string(r.Next(00000000,99999999));
                        CustomerArr[i].ID = string(r.Next(0,5) + string(r.Next(00000000,99999999);
			        }
                     
                     CustomerArr[customerIndex].Name = "Frodo";                      
                     CustomerArr[customerIndex].Name = "Sam";                                 
                     CustomerArr[customerIndex].Name = "Gloin";                                           
                     CustomerArr[customerIndex].Name = "Oin";         
                     CustomerArr[customerIndex].Name = "Sauron";                                    
                     CustomerArr[customerIndex].Name = "Saruman";                                     
                     CustomerArr[customerIndex].Name = "Elrond";             
                     CustomerArr[customerIndex].Name = "Galadriel";                                          
                     CustomerArr[customerIndex].name = "Legolas";                                     
                     CustomerArr[customerIndex].name = "Aragorn";                   

                    for (int i = parcelIndex; i < 10; i++)
			        {
                        DataSource.ParcelArr[i].id = r.Next(1000, 9999);
                        DataSource.ParcelArr[i].senderid = num4;
                        DataSource.ParcelArr[i].targetId = num4;
                        DateTime temp = DateTime.Now.AddDays(6);
                        //DateTime currentDate = new DateTime(Random, Random, Random);
                        //string printDate = currentDate.ToString("dd/MM/yyyy");
                        DataSource.ParcelArr[i].Requested = temp.AddDays(Math.Pow(i, 2));   
                        DataSource.ParcelArr[i].DroneId = 0;    
                        DataSource.ParcellArr[i].Scheduled = temp.AddDays(Math.Pow(2, i));
                        DataSource.ParcelArr[i].PickedUp = temp.AddDays(Math.Pow(2, i) + i);
                        DataSource.ParcelArr[i].Delivered = temp.AddDays(Math.Pow(2, i) + (2*i));
                        DataSource.ParcelArr[i].Weight = r.Next(0, 3);
                        DataSource.ParcelArr[i].Priority = r.Next(0, 3);
			        }
                }
    }
    public class DalObject
    {
        public void addStation(Station Victoria)
        {
            if (DataSource.Config.stationIndex <= 4)// there are max 5 places in the array
            {
                DataSource.StationArr[DataSource.Config.stationIndex] = Victoria;
                DataSource.Config.stationIndex++;
            }
            else
                Console.WriteLine("Error!overflow in array");

        }

        public void addDrone(Drone Flyboy)
        {
            if (DataSource.Config.droneIndex <= 9)// there are max 5 places in the array
            {
                DataSource.DronesArr[DataSource.Config.droneIndex] = Flyboy;
                DataSource.Config.droneIndex++;
            }
            else
                Console.WriteLine("Error!overflow in array");

        }

        public void addCustomer(Customer me)
        {
            if (DataSource.Config.customerIndex <= 99)// there are max 5 places in the array
            {
                DataSource.CustomerArr[DataSource.Config.customerIndex] = me;
                DataSource.Config.customerIndex++;
            }
            else
                Console.WriteLine("Error!overflow in array");

        }

        public void addParcel(Parcel Fedex)
        {
            if (DataSource.Config.parcelIndex <= 9)// there are max 5 places in the array
            {
                DataSource.ParcelArr[DataSource.Config.parcelIndex] = Fedex;
                DataSource.Config.parcelIndex++;
            }
            else
                Console.WriteLine("Error!overflow in array");
            //adding the parcels in a sorted wat so that it easy to find the parcels
        }

        public void ParcelDrone(int ParcelId,int DroneId)
        {
            int size = DataSource.Config.parcelIndex;//getting amount of parcels in the array
            int index = -1;
            for(int i=0;i<=size;i++)
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

        public void ParcelPickedUp(int parcelId, DateTime day )
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
                if (DataSource.StationArr [i].ID == StationId)// if the station was found
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

        public void BatteryCharged (DroneCharge Buzzer)
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


        





    }


}

