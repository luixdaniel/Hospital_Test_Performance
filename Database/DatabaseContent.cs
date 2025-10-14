using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Database
{
    public class DatabaseContent
    {
        public List<Models.Patient> Patients { get; set; }

        public List<Models.Doctor> Doctors { get; set; }

        public DatabaseContent()
        {
            Patients = new List<Models.Patient>
            {
                new Models.Patient { Id = 1, Name = "Juan Perez", DateOfBirth = new DateTime(1985,5,12), Telefono = "5551234", Address = "Calle Falsa 123", Email = "juan.perez@example.com", DocumentNumber = "12345678" },
                new Models.Patient { Id = 2, Name = "María Gómez", DateOfBirth = new DateTime(1990,8,3), Telefono = "5555678", Address = "Avenida Siempreviva 742", Email = "maria.gomez@example.com", DocumentNumber = "87654321" }
            };

            Doctors = new List<Models.Doctor>
            {
                new Models.Doctor(1, "Dr. Laura Méndez", new DateTime(1978,3,21), "5550001", "Hospital Central", "laura.mendez@hospital.com", "Cardiology", "LIC12345", "Cardio") { DocumentNumber = "0001" }
            };
        }
    }
}