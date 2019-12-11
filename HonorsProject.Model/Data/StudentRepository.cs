using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public List<Student> GetTopXFromSearch(string searchStudentTxt, int rows)
        {
            if (String.IsNullOrEmpty(searchStudentTxt))
                return _entities.ToList();
            else
                return _entities.Where(s =>
                        s.Name.Contains(searchStudentTxt)
                        || s.Email.Contains(searchStudentTxt)
                        || s.Id.ToString() == searchStudentTxt).ToList();
        }
    }
}