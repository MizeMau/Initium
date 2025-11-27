using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Database.Table
{
    public class Service<T> where T : class, IDeleteable
    {
        protected static DBContext CreateContext()
        {
            // Create a new instance of the DbContext
            // You can configure options globally in DBContext itself (e.g. OnConfiguring)
            return new DBContext();
        }

        protected DbContext _context;
        public virtual IQueryable<T> GetQuery(bool withDeleted = false)
        {
            _context = CreateContext();
            if (withDeleted)
                return _context.Set<T>().AsQueryable();
            return _context.Set<T>().Where(w => w.Deleted == null).AsQueryable();
        }

        public virtual List<T> GetAll(bool withDeleted = false)
        {
            using var context = CreateContext();
            if (withDeleted)
                return _context.Set<T>().ToList();
            return context.Set<T>().Where(w => w.Deleted == null).ToList();
        }

        public virtual T? GetById(long id)
        {
            using var context = CreateContext();
            return context.Set<T>().Find(id);
        }

        public virtual T Create(T entity)
        {
            using var context = CreateContext();
            entity.Created = DateTime.Now;
            context.Set<T>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual T Update(T entity)
        {
            using var context = CreateContext();
            context.Set<T>().Update(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual bool Delete(long id, bool hard)
        {
            using var context = CreateContext();
            var entity = context.Set<T>().Find(id);
            if (entity == null) return false;
            if (hard)
            {
                context.Set<T>().Remove(entity);
            }
            else
            {
                entity.Deleted = DateTime.Now;
                context.Set<T>().Update(entity);
            }
            context.SaveChanges();
            return true;
        }
    }
    public class BaseModel : IDeleteable
    {
        public DateTime Created { get; set; }
        public DateTime? Deleted { get; set; }
    }
    public interface IDeleteable
    {
        public DateTime Created { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
