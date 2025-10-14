using System;
using System.Collections.Generic;
using System.Linq;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Interface;
using Hospital_Test_Performance.Models;

namespace Hospital_Test_Performance.Repository
{
    public class DoctorRepository : IRepository<Doctor>
    {
        /// <summary>
        /// Repository handling CRUD operations for Doctor entities in the in-memory DatabaseContent.
        /// Ensures document uniqueness and performs simple updates.
        /// </summary>
        private readonly DatabaseContent _db;

        public DoctorRepository(DatabaseContent db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void Add(Doctor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(entity.DocumentNumber)) throw new ArgumentException("DocumentNumber is required", nameof(entity));

            if (_db.Doctors.Exists(d => d.DocumentNumber.Equals(entity.DocumentNumber, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A doctor with the same document already exists.");

            entity.Id = _db.Doctors.Count > 0 ? _db.Doctors[^1].Id + 1 : 1;
            _db.Doctors.Add(entity);
        }

        public bool DeleteByDocument(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber)) return false;
            var d = _db.Doctors.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
            if (d == null) return false;
            _db.Doctors.Remove(d);
            return true;
        }

        public IEnumerable<Doctor> GetAll()
        {
            return _db.Doctors.AsReadOnly();
        }

        public Doctor? GetByDocument(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber)) return null;
            return _db.Doctors.Find(x => x.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));
        }

        public void Update(Doctor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(entity.DocumentNumber)) throw new ArgumentException("DocumentNumber is required", nameof(entity));

            var existing = _db.Doctors.Find(x => x.Id == entity.Id || x.DocumentNumber.Equals(entity.DocumentNumber, StringComparison.OrdinalIgnoreCase));
            if (existing == null) throw new InvalidOperationException("Doctor not found");

            var other = _db.Doctors.Find(x => x.DocumentNumber.Equals(entity.DocumentNumber, StringComparison.OrdinalIgnoreCase) && x.Id != existing.Id);
            if (other != null) throw new InvalidOperationException("Another doctor already has the same document number.");

            existing.Name = entity.Name;
            existing.DateOfBirth = entity.DateOfBirth;
            existing.Telefono = entity.Telefono;
            existing.Address = entity.Address;
            existing.Email = entity.Email;
            existing.DocumentNumber = entity.DocumentNumber;
            existing.Specialty = entity.Specialty;
            existing.LicenseNumber = entity.LicenseNumber;
            existing.Department = entity.Department;
        }
    }
}