using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Get(int id);

        IEnumerable<T> GetAll();

        //IEnumerable<IEntity> Find(Expression<Func<IEntity, bool>> predicaIEntitye);
        //IEnumerable<IEntity> SingleOrDefault(Expression<Func<IEntity, bool>> predicaIEntitye);

        void Add(T enIEntityiIEntityy);

        void AddRange(IEnumerable<T> eniIEntityies);

        void Remove(T enIEntityiIEntityy);

        void RemoveRange(IEnumerable<T> enIEntityiIEntityies);
    }
}