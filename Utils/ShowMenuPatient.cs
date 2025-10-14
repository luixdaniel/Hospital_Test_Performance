using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Utils
{
    public class ShowMenuPatient
    {
        public static void DisplayMenuPatient(Hospital_Test_Performance.Service.PatientManager manager)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("Patient Management Menu:");
                Console.WriteLine("1. Register a new patient");
                Console.WriteLine("2. View patient details");
                Console.WriteLine("3. Search patient by Document Number");
                Console.WriteLine("4. Update patient information");
                Console.WriteLine("5. Delete a patient record");
                Console.WriteLine("6. Back to main menu");
                Console.Write("Select an option: ");

                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        manager.RegistrarCliente();
                        break;
                    case "2":
                        manager.ListarClientes();
                        break;
                    case "3":
                        Console.Write("Enter document number: ");
                        var doc = Console.ReadLine();
                        manager.FindByDocument(doc ?? string.Empty);
                        break;
                    case "4":
                        Console.Write("Enter document number to update: ");
                        var docUpdate = Console.ReadLine();
                        manager.UpdateByDocument(docUpdate ?? string.Empty);
                        break;
                    case "5":
                        Console.Write("Enter document number to delete: ");
                        var docDel = Console.ReadLine();
                        manager.DeleteByDocument(docDel ?? string.Empty);
                        break;
                    case "6":
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