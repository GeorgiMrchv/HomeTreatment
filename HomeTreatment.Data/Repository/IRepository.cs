using System.Collections.Generic;
using System.Linq;

namespace HomeTreatment.Data.Repository
{
    public interface IRepository
    {
        void Add<T>(T entity);

        void AddRange<T>(IEnumerable<T> entities);

        void Remove<T>(T entity);

        void RemoveRange<T>(IEnumerable<T> entities);

        void Update<T>(T entity);

        IQueryable<T> Set<T>() where T : class;
        
    }
}
