using System;
using DalObject;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {
        enum MenuOptions { Exit, Add, Update, Show_One, Show_List }
        enum EntityOptions { Exit, Station, Drone, Customer, Parcel }
        enum UpdateOptions { Exit, Assignment, Pickedup, Delivery, Recharge }
        enum ListOptions { Exit, BaseStations, Drones, Customers, Parcels, UnAssignmentParcels, AvailableChargingStations }

        private static void ShowMenu()
        {
            EntityOptions entityOption;
            int menuOption;
            do
            {
                Console.WriteLine("options:\n 1-Add, \n 2 Update,\n,3 Show Item,\n 4-showL list\n, 0-Exit");
                menuOption = Convert.ToInt32(Console.ReadLine());
                switch (menuOption)
                {
                    case MenuOptions.Add:
                        {
                            Console.WriteLine("what do you want to add?\n 1:Station,\n 2:Drone,\n 3:Customer\n,4:Parcel\n");
                            entityOption = Convert.ToInt32(Console.ReadLine());
                            switch (entityOption)
                            {
                                case EntityOptions.Station:
                                    {
                                        Console.WriteLine("Enter the name, longitude, latitude, and chargeslots\n ");
                                        string inputname = Console.ReadLine();
                                        int positions, longitudeInput, latitudeInput;
                                        int.TryParse(Console.ReadLine(), out positions);
                                        int.TryParse(Console.ReadLine(), out longitudeInput);
                                        int.TryParse(Console.ReadLine(), out latitudeInput);
                                        Station Victoria = new Station()
                                        {
                                            Name = inputname,
                                            ChargeSlots = positions,
                                            Longitude = longitudeInput,
                                            Latitude = latitudeInput,


                                        };
                                        IDAL.DO.DalObject.DalObject.addStation(Victoria);
                                        break;
                                    }
                                case EntityOptions.Customer:
                                    {

                                        Console.WriteLine("Enter the name, phone, latitude, and chargeslots\n ");
                                        string inputname = Console.ReadLine();
                                        string inputphone = Console.ReadLine();
                                        int longitudeInput, latitudeInput;
                                        int.TryParse(Console.ReadLine(), out longitudeInput);
                                        int.TryParse(Console.ReadLine(), out latitudeInput);
                                        Customer newCustomer = new Customer()
                                        {
                                            Name = inputname,
                                            Phone = inputphone,
                                            Longitude = longitudeInput,
                                            Latitude = latitudeInput,


                                        };
                                        IDAL.DO.DalObject.DalObject.addCustomer(newCustomer);
                                        break;
                                    }
                                case EntityOptions.Drone:
                                    {

                                        Console.WriteLine("Enter the name, longitude, latitude, and chargeslots\n ");
                                        string inputmodel = Console.ReadLine();
                                        string inputphone = Console.ReadLine();
                                        int longitudeInput, latitudeInput;
                                        int.TryParse(Console.ReadLine(), out longitudeInput);
                                        int.TryParse(Console.ReadLine(), out latitudeInput);
                                        Drone newDrone = new Drone()
                                        {
                                            Model =


                                        };
                                        IDAL.DO.DalObject.DalObject.addDrone(newDrone);
                                        break;
                                    }
                                case EntityOptions.Parcel:
                                    {

                                        Console.WriteLine("Enter the name, longitude, latitude, and chargeslots\n ");
                                        string inputmodel = Console.ReadLine();
                                        string inputphone = Console.ReadLine();
                                        int longitudeInput, latitudeInput;
                                        int.TryParse(Console.ReadLine(), out longitudeInput);
                                        int.TryParse(Console.ReadLine(), out latitudeInput);
                                        Drone newParcel = new Parcel()
                                        {
                                        };
                                        IDAL.DO.DalObject.DalObject.addParcel(newParcel);
                                        break;
                                    }


                            }
                            break;
                        }
                    case MenuOptions.Update:
                        {
                            break;

                        }
                    case MenuOptions.Show_List:
                        {
                            break;
                        }
                    case MenuOptions.Show_One:
                        {
                            break;

                        }
                    case MenuOptions.Exit:
                        {
                            break;
                        }

                }
            }
        }
    
        static void Main(string[] args)
        {
            
    

            
        }
    }
}
