using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(LabAssistantContext context) : base(context)
        {
        }

        public Student FindById(int id)
        {
            return _entities.FirstOrDefault(s => s.Id.Equals(id));
        }
    }
}