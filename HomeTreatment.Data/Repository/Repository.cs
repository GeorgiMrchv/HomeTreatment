using System.Collections.Generic;
using System.Linq;

namespace HomeTreatment.Data.Repository
{
    public class Repository : IRepository
    {
        protected readonly HomeTreatmentDbContext _context;
        public Repository(HomeTreatmentDbContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void AddRange<T>(IEnumerable<T> entities)
        {
           
        }

        public void Remove<T>(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange<T>(IEnumerable<T> entities)
        {
           
        }

        public void Update<T>(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return _context.Set<T>();
        }

    }
}
