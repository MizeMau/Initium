using Backend.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Backend.Database.Table
{
    public class Service<T> : ServiceBase<T> where T : class, IDeleteable
    {
        private NullabilityInfoContext _nullabilityContext = new NullabilityInfoContext();

        public virtual Dictionary<string, string> Validate(object entity, out T entityOut) => Validate(entity, true, out entityOut);
        public virtual Dictionary<string, string> Validate(object entity, out T entityOut, List<string> propsToIgnore = null) => Validate(entity, true, out entityOut, propsToIgnore);
        public virtual Dictionary<string, string> Validate(object entity, bool isCreate, out T entityOut, List<string> propsToIgnore = null)
        {
            var errorDict = new Dictionary<string, string>();

            entityOut = Activator.CreateInstance<T>();
            var props = entityOut.GetType().GetProperties();

            foreach (var prop in props)
            {
                if (CheckDefaultProp(prop, entity, entityOut, isCreate))
                    continue;

                if (propsToIgnore != null && propsToIgnore.Contains(prop.Name))
                    continue;

                object? value = GetValue(prop, entity);

                if (value == null)
                {
                    var nullabilityInfo = _nullabilityContext.Create(prop);
                    if (nullabilityInfo.WriteState == NullabilityState.NotNull)
                        errorDict.Add(FirstLetterToLower(prop.Name), Error.InputEmpty);
                    
                    continue;
                }

                if (prop.PropertyType == typeof(string))
                {
                    MaxLengthAttribute maxLengthAttribute = prop.GetCustomAttribute(typeof(MaxLengthAttribute)) as MaxLengthAttribute;
                    if (maxLengthAttribute != null && maxLengthAttribute.Length < value.ToString()!.Length)
                    {
                        errorDict.Add(FirstLetterToLower(prop.Name), string.Format(Error.ExceedMaxLength, maxLengthAttribute.Length));
                        continue;
                    }

                    if (value.ToString() == "")
                    {
                        var nullabilityInfo = _nullabilityContext.Create(prop);
                        if (nullabilityInfo.WriteState == NullabilityState.Nullable)
                            value = null;
                    }
                }

                SetValue(prop, value, entityOut);
            }

            if (errorDict.Any())
                errorDict.Add("_containsError", "true");

            return errorDict;
        }
        private bool CheckDefaultProp(PropertyInfo prop, object entity, T entityOut, bool isCreate)
        {
            var keyAttribute = prop.GetCustomAttribute(typeof(KeyAttribute));
            if (keyAttribute != null)
            {
                if (!isCreate) 
                    SetValue(prop, GetValue(prop, entity), entityOut);

                return true;
            }

            if (prop.Name == nameof(entityOut.Created))
            {
                if (!isCreate) 
                    SetValue(prop, GetValue(prop, entity), entityOut);

                return true;
            }
            if (prop.Name == nameof(entityOut.Deleted))
            {
                if (!isCreate) 
                    SetValue(prop, GetValue(prop, entity), entityOut);

                return true;
            }

            return false;
        }
        private object? GetValue(PropertyInfo prop, object entity)
        {
            if (entity is not JsonObject jentity)
                throw new NotImplementedException();

            if (!jentity.TryGetPropertyValue(
                    FirstLetterToLower(prop.Name),
                    out JsonNode? node))
                return null;

            if (node == null)
                return null;

            var targetType = Nullable.GetUnderlyingType(prop.PropertyType)
                             ?? prop.PropertyType;

            var value = node.ToString();

            if (targetType.IsEnum)
            {
                if (Enum.TryParse(targetType, value, true, out var enumValue))
                    return enumValue;

                return Enum.ToObject(targetType, Convert.ToInt32(value));
            }

            return Convert.ChangeType(value, targetType);
        }
        private void SetValue(PropertyInfo prop, object? value, T entityOut)
        {
            if (!prop.CanWrite)
                return;

            prop.SetValue(entityOut, value);
        }
        private string FirstLetterToLower(string input)
        {
            return string.Concat(input[0].ToString().ToLower(), input.AsSpan(1));
        }


        public override IQueryable<T> GetQuery(bool withDeleted = false)
        {
            if (withDeleted)
                return base.GetQuery();
            return base.GetQuery().Where(w => !w.Deleted.HasValue);
        }
        public virtual T Create(T entity)
        {
            using var context = CreateContext();
            entity.Created = DateTime.Now;
            context.Set<T>().Add(entity);
            context.SaveChanges();
            return entity;
        }
        public virtual object ValidateAndCreate(object entity)
        {
            var error = Validate(entity, true, out T entityOut);
            if (error.Any())
                return error;
            return Create(entityOut);
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

    public class ViewService<T> : ServiceBase<T> where T : class
    {

    }

    public class ServiceBase<T> where T : class
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
            return _context.Set<T>().AsQueryable();
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
    }
}
