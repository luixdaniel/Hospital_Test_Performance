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
    /// <summary>Medical specialty (required).</summary>
    public string Specialty { get; set; } = string.Empty;

    /// <summary>Professional license or registration number (required).</summary>
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

            // Ensure document uniqueness
            if (!string.IsNullOrWhiteSpace(DocumentNumber))
            {
                var exists = db.Doctors.Exists(x => x.DocumentNumber.Equals(DocumentNumber, StringComparison.OrdinalIgnoreCase) && x.Id != this.Id);
                if (exists)
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Cannot register doctor. Document '{DocumentNumber}' already exists.");
                    var existing = db.Doctors.Find(x => x.DocumentNumber.Equals(DocumentNumber, StringComparison.OrdinalIgnoreCase));
                    if (existing != null)
                    {
                        Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Existing doctor: {existing.Id}: {existing.Name} - {existing.Specialty} - Doc: {existing.DocumentNumber}");
                    }
                    return;
                }
            }

            if (Id == 0)
            {
                Id = db.Doctors.Count > 0 ? db.Doctors[^1].Id + 1 : 1;
            }
            db.Doctors.Add(this);
            Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Doctor {Name} registered with ID {Id}.");
        }
    }
}