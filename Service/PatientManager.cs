using System;
using System.Collections.Generic;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Models;

namespace Hospital_Test_Performance.Service
{
    public class PatientManager
    {
        private readonly DatabaseContent _db;

        public PatientManager(DatabaseContent db)
        {
            _db = db;
        }

        public void RegistrarCliente()
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
            Console.Write("Document number: ");
            var document = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(document) && _db.Patients.Exists(x => x.DocumentNumber.Equals(document, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Cannot register patient. Document '{document}' already exists.");
                var existing = _db.Patients.Find(x => x.DocumentNumber.Equals(document, StringComparison.OrdinalIgnoreCase));
                if (existing != null)
                {
                    Console.WriteLine($"Existing patient: {existing.Id}: {existing.Name} - {existing.Email} - Doc: {existing.DocumentNumber}");
                }
                return;
            }

            var p = new Patient { Name = name ?? string.Empty, DateOfBirth = dob, Telefono = phone ?? string.Empty, Address = address ?? string.Empty, Email = email ?? string.Empty, DocumentNumber = document ?? string.Empty };
            p.Registrar(_db);
        }

        public void ListarClientes()
        {
            foreach (var p in _db.Patients)
            {
                Console.WriteLine($"{p.Id}: {p.Name} - {p.Email} - Doc: {p.DocumentNumber}");
            }
        }
        public void FindByDocument(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Console.WriteLine("Documento inválido");
                return;
            }

            var p = _db.Patients.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (p == null) Console.WriteLine("Patient not found");
            else Console.WriteLine($"{p.Id}: {p.Name} - {p.Email} - Doc: {p.DocumentNumber}");
        }

        public void DeleteByDocument(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Console.WriteLine("Documento inválido");
                return;
            }

            var p = _db.Patients.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (p != null) _db.Patients.Remove(p);
            Console.WriteLine($"Patient with document {documentNumber} deleted (if existed).");
        }

        public void UpdateByDocument(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                Console.WriteLine("Documento inválido");
                return;
            }

            var p = _db.Patients.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (p == null)
            {
                Console.WriteLine("Patient not found");
                return;
            }

            Console.WriteLine($"Updating patient {p.Name} (Doc: {p.DocumentNumber}). Leave empty to keep current value.");
            Console.Write($"Name ({p.Name}): ");
            var name = Console.ReadLine();
            Console.Write($"DOB ({p.DateOfBirth:yyyy-MM-dd}): ");
            var dobInput = Console.ReadLine();
            Console.Write($"Phone ({p.Telefono}): ");
            var phone = Console.ReadLine();
            Console.Write($"Address ({p.Address}): ");
            var address = Console.ReadLine();
            Console.Write($"Email ({p.Email}): ");
            var email = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name)) p.Name = name;
            Console.Write($"Document ({p.DocumentNumber}): ");
            var newDoc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDoc))
            {
                // check uniqueness
                if (_db.Patients.Exists(x => x.DocumentNumber.Equals(newDoc, StringComparison.OrdinalIgnoreCase) && x.Id != p.Id))
                {
                    Console.WriteLine($"Cannot change document. Document '{newDoc}' already exists.");
                    var existing = _db.Patients.Find(x => x.DocumentNumber.Equals(newDoc, StringComparison.OrdinalIgnoreCase));
                    if (existing != null) Console.WriteLine($"Existing patient: {existing.Id}: {existing.Name} - {existing.Email} - Doc: {existing.DocumentNumber}");
                }
                else
                {
                    p.DocumentNumber = newDoc;
                }
            }
            if (DateTime.TryParse(dobInput, out var newDob)) p.DateOfBirth = newDob;
            if (!string.IsNullOrWhiteSpace(phone)) p.Telefono = phone;
            if (!string.IsNullOrWhiteSpace(address)) p.Address = address;
            if (!string.IsNullOrWhiteSpace(email)) p.Email = email;

            Console.WriteLine("Patient updated.");
        }
    }
}