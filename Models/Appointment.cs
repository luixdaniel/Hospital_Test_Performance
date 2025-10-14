using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Models
{
    /// <summary>
    /// Represents a scheduled appointment between a patient and a doctor.
    /// Relationships are stored by document number for simplicity in this demo.
    /// </summary>
    public class Appointment : Hospital_Test_Performance.Interface.IAttendable
    {
        /// <summary>Unique identifier for the appointment.</summary>
        public int Id { get; set; }

        /// <summary>Patient document number used as a reference.</summary>
        public string PatientDocument { get; set; } = string.Empty;

        /// <summary>Doctor document number used as a reference.</summary>
        public string DoctorDocument { get; set; } = string.Empty;

        /// <summary>Date and time of the appointment (local time).</summary>
        public DateTime AppointmentDate { get; set; }

        /// <summary>Optional notes for the appointment.</summary>
        public string Notes { get; set; } = string.Empty;
        
        /// <summary>Current status of the appointment.</summary>
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        /// <summary>Mark the appointment as attended (if not cancelled).</summary>
        public void MarkAttended()
        {
            if (Status == AppointmentStatus.Cancelled)
            {
                Hospital_Test_Performance.Utils.ConsoleHelper.WriteError("Cannot mark a cancelled appointment as attended.");
                return;
            }
            Status = AppointmentStatus.Attended;
            Hospital_Test_Performance.Utils.ConsoleHelper.WriteSuccess($"Appointment {Id} marked as Attended.");
        }
    }

    /// <summary>Simple enum representing appointment lifecycle states.</summary>
    public enum AppointmentStatus
    {
        Pending,
        Attended,
        Cancelled
    }
}