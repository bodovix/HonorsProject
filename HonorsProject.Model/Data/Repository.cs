using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext Context;
        protected DbSet<T> _entities;

        public Repository(DbContext context)
        {
            Context = context;
            _entities = Context.Set<T>();
        }

        public T Get(int id)
        {
            return _entities.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public IEnumerable<T> GetTop(int rowCount)
        {
            return _entities.Take(rowCount).ToList();
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _entities.AddRange(entities);
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }

        public int Count()
        {
            return _entities.Count();
        }
    }
}