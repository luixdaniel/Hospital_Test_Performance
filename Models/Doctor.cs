using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Models
{
    /// <summary>
    /// Represents a doctor in the hospital and inherits common person fields from Person.
    /// </summary>
    public class Doctor : Person
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
    }
}