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
            Console.Write("Document number: ");
            var document = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(document) && _db.Doctors.Exists(x => x.DocumentNumber.Equals(document, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Cannot register doctor. Document '{document}' already exists.");
                var existing = _db.Doctors.Find(x => x.DocumentNumber.Equals(document, StringComparison.OrdinalIgnoreCase));
                if (existing != null) Console.WriteLine($"Existing doctor: {existing.Id}: {existing.Name} - {existing.Specialty} - Doc: {existing.DocumentNumber}");
                return;
            }

            var d = new Doctor(0, name ?? string.Empty, dob, phone ?? string.Empty, address ?? string.Empty, email ?? string.Empty, specialty ?? string.Empty, license ?? string.Empty, string.Empty)
            {
                DocumentNumber = document ?? string.Empty
            };
            d.Registrar(_db);
        }

        public void ListarDoctores()
        {
            foreach (var d in _db.Doctors)
            {
                Console.WriteLine($"{d.Id}: {d.Name} - {d.Specialty} - Doc: {d.DocumentNumber}");
            }
        }

        public void FindByDocument(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Console.WriteLine("Invalid document");
                return;
            }

            var d = _db.Doctors.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (d == null) Console.WriteLine("Doctor not found");
            else Console.WriteLine($"{d.Id}: {d.Name} - {d.Specialty} - Doc: {d.DocumentNumber}");
        }

        public void DeleteByDocument(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Console.WriteLine("Invalid document");
                return;
            }

            var d = _db.Doctors.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (d != null) _db.Doctors.Remove(d);
            Console.WriteLine($"Doctor with document {documentNumber} deleted (if existed).");
        }

        public void UpdateByDocument(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Console.WriteLine("Invalid document");
                return;
            }

            var d = _db.Doctors.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (d == null)
            {
                Console.WriteLine("Doctor not found");
                return;
            }

            Console.WriteLine($"Updating doctor {d.Name} (Doc: {d.DocumentNumber}). Leave empty to keep current value.");
            Console.Write($"Name ({d.Name}): ");
            var name = Console.ReadLine();
            Console.Write($"DOB ({d.DateOfBirth:yyyy-MM-dd}): ");
            var dobInput = Console.ReadLine();
            Console.Write($"Phone ({d.Telefono}): ");
            var phone = Console.ReadLine();
            Console.Write($"Address ({d.Address}): ");
            var address = Console.ReadLine();
            Console.Write($"Email ({d.Email}): ");
            var email = Console.ReadLine();
            Console.Write($"Specialty ({d.Specialty}): ");
            var specialty = Console.ReadLine();
            Console.Write($"License ({d.LicenseNumber}): ");
            var license = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name)) d.Name = name;
            if (DateTime.TryParse(dobInput, out var newDob)) d.DateOfBirth = newDob;
            if (!string.IsNullOrWhiteSpace(phone)) d.Telefono = phone;
            if (!string.IsNullOrWhiteSpace(address)) d.Address = address;
            if (!string.IsNullOrWhiteSpace(email)) d.Email = email;
            if (!string.IsNullOrWhiteSpace(specialty)) d.Specialty = specialty;
            if (!string.IsNullOrWhiteSpace(license)) d.LicenseNumber = license;

            Console.Write($"Document ({d.DocumentNumber}): ");
            var newDoc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDoc))
            {
                if (_db.Doctors.Exists(x => x.DocumentNumber.Equals(newDoc, StringComparison.OrdinalIgnoreCase) && x.Id != d.Id))
                {
                    Console.WriteLine($"Cannot change document. Document '{newDoc}' already exists.");
                    var existing = _db.Doctors.Find(x => x.DocumentNumber.Equals(newDoc, StringComparison.OrdinalIgnoreCase));
                    if (existing != null) Console.WriteLine($"Existing doctor: {existing.Id}: {existing.Name} - {existing.Specialty} - Doc: {existing.DocumentNumber}");
                }
                else
                {
                    d.DocumentNumber = newDoc;
                }
            }

            Console.WriteLine("Doctor updated.");
        }
    }
}