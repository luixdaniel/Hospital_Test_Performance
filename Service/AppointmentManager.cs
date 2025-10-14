using System;
using System.Linq;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Models;

namespace Hospital_Test_Performance.Service
{
    public class AppointmentManager
    {
        private readonly DatabaseContent _db;

        public AppointmentManager(DatabaseContent db)
        {
            _db = db;
        }

        public void ScheduleAppointment()
        {
            try
            {
            Console.Write("Patient document: ");
            var patientDoc = Console.ReadLine();
            Console.Write("Doctor document: ");
            var doctorDoc = Console.ReadLine();
            Console.Write("Date and time (yyyy-MM-dd HH:mm): ");
            var dtInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(patientDoc) || string.IsNullOrWhiteSpace(doctorDoc) || string.IsNullOrWhiteSpace(dtInput))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Missing required fields.");
                return;
            }

            if (!_db.Patients.Exists(p => p.DocumentNumber.Equals(patientDoc, StringComparison.OrdinalIgnoreCase)))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Patient not found");
                return;
            }

            if (!_db.Doctors.Exists(d => d.DocumentNumber.Equals(doctorDoc, StringComparison.OrdinalIgnoreCase)))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Doctor not found");
                return;
            }

            if (!DateTime.TryParseExact(dtInput, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var when))
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Invalid date/time format. Use yyyy-MM-dd HH:mm");
                return;
            }

            // prevent double-booking: same doctor at the exact same datetime
            var doctorConflict = _db.Appointments.Any(a => a.DoctorDocument.Equals(doctorDoc, StringComparison.OrdinalIgnoreCase) && a.AppointmentDate == when);
            if (doctorConflict)
            {
                var existing = _db.Appointments.Find(a => a.DoctorDocument.Equals(doctorDoc, StringComparison.OrdinalIgnoreCase) && a.AppointmentDate == when);
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Doctor already has an appointment at that time (Appointment ID: {existing?.Id}).");
                return;
            }

            // prevent double-booking for patient as well
            var patientConflict = _db.Appointments.Any(a => a.PatientDocument.Equals(patientDoc, StringComparison.OrdinalIgnoreCase) && a.AppointmentDate == when);
            if (patientConflict)
            {
                var existing = _db.Appointments.Find(a => a.PatientDocument.Equals(patientDoc, StringComparison.OrdinalIgnoreCase) && a.AppointmentDate == when);
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Patient already has an appointment at that time (Appointment ID: {existing?.Id}).");
                return;
            }

            var appt = new Appointment
            {
                Id = _db.Appointments.Count > 0 ? _db.Appointments[^1].Id + 1 : 1,
                PatientDocument = patientDoc,
                DoctorDocument = doctorDoc,
                AppointmentDate = when
            };

            _db.Appointments.Add(appt);
            Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Appointment scheduled (ID: {appt.Id}) on {appt.AppointmentDate:yyyy-MM-dd HH:mm}");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error scheduling appointment: {ex.Message}");
            }
        }

        public void ListAppointments()
        {
            try
            {
            if (_db.Appointments.Count == 0)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("No appointments scheduled.");
                return;
            }

            foreach (var a in _db.Appointments)
            {
                var patient = _db.Patients.Find(p => p.DocumentNumber.Equals(a.PatientDocument, StringComparison.OrdinalIgnoreCase));
                var doctor = _db.Doctors.Find(d => d.DocumentNumber.Equals(a.DoctorDocument, StringComparison.OrdinalIgnoreCase));
                Console.WriteLine($"{a.Id}: {a.AppointmentDate:yyyy-MM-dd HH:mm} - Status: {a.Status} - Patient: {(patient != null ? patient.Name : a.PatientDocument)} - Doctor: {(doctor != null ? doctor.Name : a.DoctorDocument)}");
            }
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error listing appointments: {ex.Message}");
            }
        }

        public void MarkAttended(int appointmentId)
        {
            try
            {
            var a = _db.Appointments.Find(x => x.Id == appointmentId);
            if (a == null)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Appointment not found");
                return;
            }
            // Use the model's IAttendable implementation
            if (a is Hospital_Test_Performance.Interface.IAttendable attendable)
            {
                attendable.MarkAttended();
            }
            else
            {
                a.Status = AppointmentStatus.Attended;
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Appointment {appointmentId} marked as Attended.");
            }
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error marking appointment attended: {ex.Message}");
            }
        }

        public void MarkCancelled(int appointmentId)
        {
            try
            {
            var a = _db.Appointments.Find(x => x.Id == appointmentId);
            if (a == null)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Appointment not found");
                return;
            }
            a.Status = AppointmentStatus.Cancelled;
            Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Appointment {appointmentId} marked as Cancelled.");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error marking appointment cancelled: {ex.Message}");
            }
        }

        public void MarkPending(int appointmentId)
        {
            try
            {
            var a = _db.Appointments.Find(x => x.Id == appointmentId);
            if (a == null)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Appointment not found");
                return;
            }
            a.Status = AppointmentStatus.Pending;
            Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Appointment {appointmentId} marked as Pending.");
            }
            catch (Exception ex)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError($"Error marking appointment pending: {ex.Message}");
            }
        }
    }
}
 