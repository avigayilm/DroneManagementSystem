using System;


namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome9033();
            Welcome6996();
            Console.ReadKey();
        }


        private static void Welcome9033()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }

        static partial void Welcome6996();
    }
}
