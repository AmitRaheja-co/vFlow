using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using vFlow.Data;
using vFlow.Repository.IRepository;
using System.Reflection;
using System;
using System.Linq.Expressions;
namespace vFlow.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            //Console.WriteLine("I am here to add in repository.cs file check here");
            //Console.WriteLine(entity.ToString());
            //Console.WriteLine();

            //// Using reflection to print properties and values
            //Console.WriteLine("Using reflection:");
            //Type type = entity.GetType();
            //PropertyInfo[] properties = type.GetProperties();
            //foreach (PropertyInfo property in properties)
            //{
            //    object value = property.GetValue(entity);
            //    Console.WriteLine($"{property.Name}: {value}");
            //}
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            T query = (T)dbSet.Where(filter).FirstOrDefault();
            return query;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.ToList();
        }
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

    }
}