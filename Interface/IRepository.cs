using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Test_Performance.Interface
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);

        // Deletes by document number; returns true if an entity was removed
        bool DeleteByDocument(string documentNumber);

        // Returns the entity or null if not found
        T? GetByDocument(string documentNumber);

        // Returns all entities
        System.Collections.Generic.IEnumerable<T> GetAll();
    }
}