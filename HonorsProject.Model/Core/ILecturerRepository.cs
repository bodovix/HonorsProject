using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface ILecturerRepository : IRepository<Lecturer>
    {
        IEnumerable<Lecturer> GetLecturerWhereX(int id);
        List<Lecturer> GetTopXFromSearch(string searchTxt, int rows);
    }
}