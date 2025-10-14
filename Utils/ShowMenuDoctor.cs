using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Utils
{
    public class ShowMenuDoctor
    {
        public static void DisplayMenuDoctor(Hospital_Test_Performance.Service.DoctorManager manager)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("Doctor Management Menu:");
                Console.WriteLine("1. Register a new doctor");
                Console.WriteLine("2. View doctor details");
                Console.WriteLine("3. Update doctor information");
                Console.WriteLine("4. Delete a doctor record");
                Console.WriteLine("5. Back to main menu");
                Console.Write("Select an option: ");

                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        manager.RegistrarDoctor();
                        break;
                    case "2":
                        manager.ListarDoctores();
                        break;
                    case "3":
                        Console.WriteLine("[Placeholder] Updating doctor information...");
                        break;
                    case "4":
                        Console.WriteLine("[Placeholder] Deleting a doctor record...");
                        break;
                    case "5":
                        back = true;
                        continue;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }
    }
}