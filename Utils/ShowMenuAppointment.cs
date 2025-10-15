using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Utils
{
    public class ShowMenuAppointment
    {
    public static void DisplayMenuAppointment(Hospital_Test_Performance.Service.AppointmentManager manager)
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
                    Console.WriteLine("3. Mark appointment as Attended");
                    Console.WriteLine("4. Mark appointment as Cancelled");
                    Console.WriteLine("5. Mark appointment as Pending");
                    Console.WriteLine("6. List email history");
                    Console.WriteLine("7. Retry email send by id");
                    Console.WriteLine("8. Back to main menu");
                    Console.Write("Select an option: ");

                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            manager.ScheduleAppointment();
                            // TODO: call appointment scheduling logic
                            break;
                        case "2":
                            manager.ListAppointments();
                            break;
                        case "3":
                            Console.Write("Enter appointment ID to mark as Attended: ");
                            if (int.TryParse(Console.ReadLine(), out var idA)) manager.MarkAttended(idA);
                            break;
                        case "4":
                            Console.Write("Enter appointment ID to mark as Cancelled: ");
                            if (int.TryParse(Console.ReadLine(), out var idC)) manager.MarkCancelled(idC);
                            break;
                        case "5":
                            Console.Write("Enter appointment ID to mark as Pending: ");
                            if (int.TryParse(Console.ReadLine(), out var idP)) manager.MarkPending(idP);
                            break;
                        case "6":
                            manager.ListEmailHistory();
                            break;
                        case "7":
                            Console.Write("Enter email record ID to retry: ");
                            if (int.TryParse(Console.ReadLine(), out var emailId)) manager.RetryEmail(emailId);
                            break;
                        case "8":
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