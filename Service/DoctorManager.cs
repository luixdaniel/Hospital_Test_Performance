using System;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Models;

namespace Hospital_Test_Performance.Service
{
    public class DoctorManager
    {
        private readonly DatabaseContent _db;

        public DoctorManager(DatabaseContent db)
        {
            _db = db;
        }

        public void RegistrarDoctor()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();
            Console.Write("DOB (yyyy-MM-dd): ");
            DateTime.TryParse(Console.ReadLine(), out var dob);
            Console.Write("Phone: ");
            var phone = Console.ReadLine();
            Console.Write("Address: ");
            var address = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("Specialty: ");
            var specialty = Console.ReadLine();
            Console.Write("License: ");
            var license = Console.ReadLine();

            var d = new Doctor(0, name ?? string.Empty, dob, phone ?? string.Empty, address ?? string.Empty, email ?? string.Empty, specialty ?? string.Empty, license ?? string.Empty, string.Empty);
            d.Registrar(_db);
        }

        public void ListarDoctores()
        {
            foreach (var d in _db.Doctors)
            {
                Console.WriteLine($"{d.Id}: {d.Name} - {d.Specialty}");
            }
        }
    }
}