using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface IRepository<IEntity>
    {
        IEntity Get(int id);

        IEnumerable<IEntity> GetAll();

        //IEnumerable<IEntity> Find(Expression<Func<IEntity, bool>> predicaIEntitye);
        //IEnumerable<IEntity> SingleOrDefault(Expression<Func<IEntity, bool>> predicaIEntitye);

        void Add(IEntity enIEntityiIEntityy);

        void AddRange(IEnumerable<IEntity> eniIEntityies);

        void Remove(IEntity enIEntityiIEntityy);

        void RemoveRange(IEnumerable<IEntity> enIEntityiIEntityies);
    }
}