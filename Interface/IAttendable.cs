namespace Hospital_Test_Performance.Interface
{
    /// <summary>
    /// Interface for entities that can be marked as attended (e.g., appointments).
    /// </summary>
    public interface IAttendable
    {
        /// <summary>Mark the entity as attended.</summary>
        void MarkAttended();
    }
}