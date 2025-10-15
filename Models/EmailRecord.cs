using System;

namespace Hospital_Test_Performance.Models
{
    /// <summary>
    /// Represents a sent (or attempted) email and its status for auditing.
    /// </summary>
    public class EmailRecord
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        // Original intended recipient (before any override)
        public string OriginalTo { get; set; } = string.Empty;

        // Actual recipient used when sending (after override logic)
        public string FinalTo { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool Sent { get; set; }
        // Short error message and detailed exception data for debugging
        public string? ErrorMessage { get; set; }
        public string? ErrorDetail { get; set; }
        // Optional reference to appointment id
        public int? AppointmentId { get; set; }
    }
}
