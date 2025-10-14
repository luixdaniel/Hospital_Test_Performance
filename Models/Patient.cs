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

            // If document number provided, ensure uniqueness
            if (!string.IsNullOrWhiteSpace(DocumentNumber))
            {
                var exists = db.Patients.Exists(x => x.DocumentNumber.Equals(DocumentNumber, StringComparison.OrdinalIgnoreCase) && x.Id != this.Id);
                if (exists)
                {
                    Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Cannot register patient. Document '{DocumentNumber}' already exists.");
                    var existing = db.Patients.Find(x => x.DocumentNumber.Equals(DocumentNumber, StringComparison.OrdinalIgnoreCase));
                    if (existing != null)
                    {
                        Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Existing patient: {existing.Id}: {existing.Name} - {existing.Email} - Doc: {existing.DocumentNumber}");
                    }
                    return;
                }
            }

            if (Id == 0)
            {
                Id = db.Patients.Count > 0 ? db.Patients[^1].Id + 1 : 1;
            }
            db.Patients.Add(this);
            Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Patient {Name} registered with ID {Id}.");
        }
    }
}
