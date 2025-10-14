using Hospital_Test_Performance.Database;

namespace Hospital_Test_Performance.Interface
{
    /// <summary>
    /// Simple interface for registrable entities.
    /// </summary>
    public interface IRegistable
    {
        /// <summary>
        /// Register the entity into the provided in-memory database.
        /// </summary>
        void Registrar(DatabaseContent db);
    }
}
