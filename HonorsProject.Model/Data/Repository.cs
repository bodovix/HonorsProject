using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class Repository<T> : IRepository<IEntity>
    {
        protected readonly DbContext Context;
        private DbSet<IEntity> _entities;

        public Repository(DbContext context)
        {
            Context = context;
            _entities = Context.Set<IEntity>();
        }

        public IEntity Get(int id)
        {
            return _entities.Find(id);
        }

        public IEnumerable<IEntity> GetAll()
        {
            return _entities.ToList();
        }

        public IEnumerable<IEntity> Find(Expression<Func<IEntity, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public IEntity SingleOrDefault(Expression<Func<IEntity, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
        }

        public void Add(IEntity entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(IEnumerable<IEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public void Remove(IEntity entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<IEntity> entities)
        {
            _entities.RemoveRange(entities);
        }
    }
}