using System;
using System.Collections.Generic;
using System.Linq;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Interface;
using Hospital_Test_Performance.Models;

namespace Hospital_Test_Performance.Repository
{
    public class PatientRepository : IRepository<Patient>
    {
        private readonly DatabaseContent _db;

    /// <summary>
    /// Repository handling basic CRUD operations for Patient entities against DatabaseContent.
    /// Performs simple validations like unique DocumentNumber and Id assignment.
    /// </summary>
        public PatientRepository(DatabaseContent db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void Add(Patient entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(entity.DocumentNumber)) throw new ArgumentException("DocumentNumber is required", nameof(entity));

            if (_db.Patients.Exists(p => p.DocumentNumber.Equals(entity.DocumentNumber, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A patient with the same document already exists.");

            entity.Id = _db.Patients.Count > 0 ? _db.Patients[^1].Id + 1 : 1;
            _db.Patients.Add(entity);
        }

        public bool DeleteByDocument(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber)) return false;
            var p = _db.Patients.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (p == null) return false;
            _db.Patients.Remove(p);
            return true;
        }

        public IEnumerable<Patient> GetAll()
        {
            return _db.Patients.AsReadOnly();
        }

        public Patient? GetByDocument(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber)) return null;
            return _db.Patients.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
        }

        public void Update(Patient entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(entity.DocumentNumber)) throw new ArgumentException("DocumentNumber is required", nameof(entity));

            var existing = _db.Patients.Find(x => x.Id == entity.Id || x.DocumentNumber.Equals(entity.DocumentNumber, StringComparison.OrdinalIgnoreCase));
            if (existing == null) throw new InvalidOperationException("Patient not found");

            var other = _db.Patients.Find(x => x.DocumentNumber.Equals(entity.DocumentNumber, StringComparison.OrdinalIgnoreCase) && x.Id != existing.Id);
            if (other != null) throw new InvalidOperationException("Another patient already has the same document number.");

            existing.Name = entity.Name;
            existing.DateOfBirth = entity.DateOfBirth;
            existing.Telefono = entity.Telefono;
            existing.Address = entity.Address;
            existing.Email = entity.Email;
            existing.DocumentNumber = entity.DocumentNumber;
            existing.MedicalRecordNumber = entity.MedicalRecordNumber;
            existing.AdmissionDate = entity.AdmissionDate;
            existing.Ward = entity.Ward;
            existing.Bed = entity.Bed;
            existing.Diagnoses = entity.Diagnoses;
        }
    }
}