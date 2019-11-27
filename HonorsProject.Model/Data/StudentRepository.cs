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

        public Student Login(int id, string password)
        {
            string passwordString = password.ToString();

            Student newS = new Student(id, "test", "e@a.cp", password, DateTime.Now, 1234);
            _entities.Add(newS);
            return newS;
            // return _entities.FirstOrDefault(s => s.Id == id && s.Password == passwordString);
        }
    }
}