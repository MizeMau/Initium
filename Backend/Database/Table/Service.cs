using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public virtual IQueryable<T> GetQuery(HttpRequest? request, bool withDeleted = false)
        {
            var query = GetQuery(withDeleted);

            if (request == null)
                return query;

            foreach (var (key, value) in request.Query)
            {
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                string modelKey = $"{key[0].ToString().ToUpper()}{key.Substring(1)}";
                var property = typeof(T).GetProperty(modelKey);
                if (property == null)
                    continue;

                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyAccess = Expression.Property(parameter, property);

                var convertedValue = Convert.ChangeType(value.ToString(), property.PropertyType);

                var constant = Expression.Constant(convertedValue);
                var equality = Expression.Equal(propertyAccess, constant);
                var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

                query = query.Where(lambda);
            }

            return query;
        }

        public virtual List<T> GetAll(bool withDeleted = false)
        {
            return GetQuery(withDeleted)
                .ToList();
        }

        public virtual List<T> GetAll(HttpRequest request, bool withDeleted = false)
        {
            return GetQuery(request, withDeleted)
                .ToList();
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

        public virtual bool UpdateProperty<TProperty>(object key, Expression<Func<T, TProperty>> propertyExpression, TProperty value)
        {
            using var context = CreateContext();

            var entityType = context.Model.FindEntityType(typeof(T));
            var primaryKey = entityType!.FindPrimaryKey()!;
            var keyProperty = primaryKey.Properties.First();

            var parameter = Expression.Parameter(typeof(T), "e");

            var propertyAccess = Expression.Call(
                typeof(EF),
                nameof(EF.Property),
                new[] { keyProperty.ClrType },
                parameter,
                Expression.Constant(keyProperty.Name)
            );

            var equals = Expression.Equal(
                propertyAccess,
                Expression.Constant(key)
            );

            var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

            var affected = context.Set<T>()
                .Where(lambda)
                .ExecuteUpdate(setters =>
                    setters.SetProperty(propertyExpression, value));

            return affected == 1;
        }

        public virtual bool Delete(long id, bool hard = false)
        {
            using var context = CreateContext();
            if (!hard)
            {
                return UpdateProperty(id, u => u.Deleted, DateTime.Now);
            }
            var entity = context.Set<T>().Find(id);
            if (entity == null) return false;
            context.Set<T>().Remove(entity);
            context.SaveChanges();
            return true;
        }
    }
}
