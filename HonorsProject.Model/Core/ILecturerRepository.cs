using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public interface ILecturerRepository
    {
        IEnumerable<Lecturer> GetLecturerWhereX(int id);
    }
}