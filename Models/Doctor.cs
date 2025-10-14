using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Models
{
    /// <summary>
    /// Represents a doctor in the hospital and inherits common person fields from Person.
    /// </summary>
    public class Doctor : Person, Hospital_Test_Performance.Interface.IRegistable
    {
        public string Specialty { get; set; } = string.Empty;

        public string LicenseNumber { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public Doctor()
        {
        }

        public Doctor(int id, string name, DateTime dateOfBirth, string phone, string address, string email,
            string specialty = "", string licenseNumber = "", string department = "")
            : base(id, name, dateOfBirth, phone, address, email)
        {
            Specialty = specialty ?? string.Empty;
            LicenseNumber = licenseNumber ?? string.Empty;
            Department = department ?? string.Empty;
        }

        public override string ToString()
        {
            return $"Dr. {Name} ({Specialty}) - License: {LicenseNumber}";
        }

        public void Registrar(Hospital_Test_Performance.Database.DatabaseContent db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (Id == 0)
            {
                Id = db.Doctors.Count > 0 ? db.Doctors[^1].Id + 1 : 1;
            }
            db.Doctors.Add(this);
            Console.WriteLine($"Doctor {Name} registrado con ID {Id}.");
        }
    }
}