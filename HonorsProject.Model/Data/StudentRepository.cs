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

        public List<Student> GetStudentsFromGroup(Group group)
        {
            return _entities.Where(s => s.Groups.Any(g => g.Id == group.Id)).ToList();
        }

        public List<Student> GetStudentsNotInGroup(Group group)
        {
            return _entities.Where(s => !s.Groups.Any(g => g.Id == group.Id)).ToList();
        }

        public List<Student> GetTopXFromSearch(string searchStudentTxt, int rows)
        {
            //if no search return all
            if (String.IsNullOrEmpty(searchStudentTxt))
                return _entities.Take(rows).ToList();
            else
                return _entities.Where(s =>
                        s.Name.Contains(searchStudentTxt)
                        || s.Email.Contains(searchStudentTxt)
                        || s.Id.ToString().Contains(searchStudentTxt)).Take(rows).ToList();
        }
    }
}