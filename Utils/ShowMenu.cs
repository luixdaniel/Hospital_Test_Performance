using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Utils
{
    public class ShowMenu
    {
        public static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Hospital Management System Menu:");
            Console.WriteLine("1. patient procedures");
            Console.WriteLine("2. Doctor procedures");
            Console.WriteLine("3. make an appointment");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");
        }
    }
}