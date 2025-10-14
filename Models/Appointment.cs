using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Models
{
    public class Appointment : Hospital_Test_Performance.Interface.IAttendable
    {
        public int Id { get; set; }

        // store by document numbers to keep relationships simple in this demo
        public string PatientDocument { get; set; } = string.Empty;

        public string DoctorDocument { get; set; } = string.Empty;

        public DateTime AppointmentDate { get; set; }

        public string Notes { get; set; } = string.Empty;
        
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

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

    public enum AppointmentStatus
    {
        Pending,
        Attended,
        Cancelled
    }
}