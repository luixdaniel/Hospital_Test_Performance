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
                Console.WriteLine("3. Search patient by ID");
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
                        Console.Write("Enter ID: ");
                        if (int.TryParse(Console.ReadLine(), out var id)) manager.BuscarClientePorId(id);
                        break;
                    case "4":
                        Console.Write("Enter ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out var idDel)) manager.EliminarCliente(idDel);
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