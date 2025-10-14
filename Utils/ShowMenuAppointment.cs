using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Utils
{
    public class ShowMenuAppointment
    {
        public static void DisplayMenuAppointment()
        {
            Console.Clear();
            Console.WriteLine("Appointment Management Menu:");
            Console.WriteLine("1. Schedule a new appointment");
            Console.WriteLine("2. View appointment details");
            Console.WriteLine("3. Update appointment information");
            Console.WriteLine("4. Cancel an appointment");
            Console.WriteLine("5. Back to main menu");
            Console.Write("Select an option: ");
                bool back = false;
                while (!back)
                {
                    Console.Clear();
                    Console.WriteLine("Appointment Management Menu:");
                    Console.WriteLine("1. Schedule a new appointment");
                    Console.WriteLine("2. View appointment details");
                    Console.WriteLine("3. Update appointment information");
                    Console.WriteLine("4. Cancel an appointment");
                    Console.WriteLine("5. Back to main menu");
                    Console.Write("Select an option: ");

                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            Console.WriteLine("[Placeholder] Scheduling a new appointment...");
                            // TODO: call appointment scheduling logic
                            break;
                        case "2":
                            Console.WriteLine("[Placeholder] Showing appointment details...");
                            break;
                        case "3":
                            Console.WriteLine("[Placeholder] Updating appointment...");
                            break;
                        case "4":
                            Console.WriteLine("[Placeholder] Cancelling appointment...");
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