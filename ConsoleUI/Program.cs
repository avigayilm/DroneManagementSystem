using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IDAL.DO.Station Station = new IDAL.DO.Station();
            IDAL.DO.Customer client = new IDAL.DO.Customer
            {
                Name = "Avigayil",
                Latitude = -36.123456,
                Longitude = 29.654321,
                ID = 123,
                Phone = "052534111",
            };
            Console.WriteLine(client);
            Console.WriteLine("Press any key to continue...");

            ;
        }
    }
}
