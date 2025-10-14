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

            var p = new Patient { Name = name ?? string.Empty, DateOfBirth = dob, Telefono = phone ?? string.Empty, Address = address ?? string.Empty, Email = email ?? string.Empty };
            p.Registrar(_db);
        }

        public void ListarClientes()
        {
            foreach (var p in _db.Patients)
            {
                Console.WriteLine($"{p.Id}: {p.Name} - {p.Email}");
            }
        }

        public void BuscarClientePorId(int id)
        {
            var p = _db.Patients.Find(x => x.Id == id);
            if (p == null) Console.WriteLine("Cliente no encontrado");
            else Console.WriteLine($"{p.Id}: {p.Name} - {p.Email}");
        }

        public void EliminarCliente(int id)
        {
            var p = _db.Patients.Find(x => x.Id == id);
            if (p != null) _db.Patients.Remove(p);
            Console.WriteLine($"Cliente {id} eliminado (si existía).");
        }
    }
}