using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Database
{
    /// <summary>
    /// In-memory datastore used by the demo application. Holds lists of Patients, Doctors and Appointments.
    /// This is a simple stand-in for a real database.
    /// </summary>
    public class DatabaseContent
    {
        public List<Models.Patient> Patients { get; set; }

        public List<Models.Doctor> Doctors { get; set; }

    public List<Models.Appointment> Appointments { get; set; }
    // History of sent or attempted emails
    public List<Models.EmailRecord> EmailHistory { get; set; }

        public DatabaseContent()
        {
            Patients = new List<Models.Patient>
            {
                // Seed some demo patients
                new Models.Patient { Id = 1, Name = "Juan Perez", DateOfBirth = new DateTime(1985,5,12), Telefono = "5551234", Address = "Calle Falsa 123", Email = "ceraluis4@gmail.com", DocumentNumber = "12345678" },
                new Models.Patient { Id = 2, Name = "María Gómez", DateOfBirth = new DateTime(1990,8,3), Telefono = "5555678", Address = "Avenida Siempreviva 742", Email = "ceraluis4@gmail.com", DocumentNumber = "87654321" }
            };

            Doctors = new List<Models.Doctor>
            {
                // Seed some demo doctors
                new Models.Doctor(1, "Dr. Laura Méndez", new DateTime(1978,3,21), "5550001", "Hospital Central", "laura.mendez@hospital.com", "Cardiology", "LIC12345", "Cardio") { DocumentNumber = "0001" },
                new Models.Doctor(2, "Dr. Miguel Torres", new DateTime(1980,6,10), "5550002", "Clinic Norte", "miguel.torres@clinic.com", "Neurology", "LIC23456", "Neuro") { DocumentNumber = "0002" },
                new Models.Doctor(3, "Dr. Ana Ruiz", new DateTime(1985,11,2), "5550003", "Children's Hospital", "ana.ruiz@kids.com", "Pediatrics", "LIC34567", "Pedi") { DocumentNumber = "0003" }
            };

            Appointments = new List<Models.Appointment>
            {
                // Seed some demo appointments
                new Models.Appointment { Id = 1, PatientDocument = "12345678", DoctorDocument = "0002", AppointmentDate = new DateTime(2025,10,15,10,30,0), Notes = "Sample seeded appointment" },
                new Models.Appointment { Id = 2, PatientDocument = "87654321", DoctorDocument = "0001", AppointmentDate = new DateTime(2025,10,16,9,0,0), Notes = "Second seeded appointment" },
                new Models.Appointment { Id = 3, PatientDocument = "12345678", DoctorDocument = "0003", AppointmentDate = new DateTime(2025,10,17,14,0,0), Notes = "Third seeded appointment" }
            };

            EmailHistory = new List<Models.EmailRecord>();
        }
    }
}