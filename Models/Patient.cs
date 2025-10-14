using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Models
{
    /// <summary>
    /// Represents a patient in the hospital. Inherits common person fields from Person.
    /// </summary>
    public class Patient : Person, Hospital_Test_Performance.Interface.IRegistable
    {
        /// <summary>Medical record number or identifier for the patient.</summary>
        public string MedicalRecordNumber { get; set; } = string.Empty;

        /// <summary>Date the patient was admitted (null if not admitted).</summary>
        public DateTime? AdmissionDate { get; set; }

        /// <summary>Ward or department where the patient is located.</summary>
        public string Ward { get; set; } = string.Empty;

        /// <summary>Bed identifier within the ward.</summary>
        public string Bed { get; set; } = string.Empty;

        /// <summary>List of diagnoses for the patient.</summary>
        public List<string> Diagnoses { get; set; } = new List<string>();

        public Patient()
        {
        }

        public Patient(string medicalRecordNumber, DateTime? admissionDate = null)
        {
            if (string.IsNullOrWhiteSpace(medicalRecordNumber))
                throw new ArgumentException("medicalRecordNumber is required", nameof(medicalRecordNumber));

            MedicalRecordNumber = medicalRecordNumber;
            AdmissionDate = admissionDate;
        }

        public void Registrar(Hospital_Test_Performance.Database.DatabaseContent db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (Id == 0)
            {
                Id = db.Patients.Count > 0 ? db.Patients[^1].Id + 1 : 1;
            }
            db.Patients.Add(this);
            Console.WriteLine($"Paciente {Name} registrado con ID {Id}.");
        }
    }
}
