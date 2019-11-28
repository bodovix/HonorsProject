using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using HonorsProject.Model.HelperClasses;
using System.Security.Cryptography;
using System.Security.Authentication;

namespace HonorsProject.Model.Entities
{
    public class Student : BaseEntity, ISystemUser<Student>
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Group> Groups { get; set; }
        public List<Question> Questions { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByLecturerID { get; set; }

        #endregion Properties

        public Student()
        {
            Groups = new List<Group>();
            Questions = new List<Question>();
        }

        public Student(int id, string name, string email, string password, DateTime createdOn, int createdByLecturerID)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            CreatedOn = createdOn;
            CreatedByLecturerID = createdByLecturerID;
            Groups = new List<Group>();
            Questions = new List<Question>();
        }

        public ISystemUser<Student> Login(int userId, string password, string conName)
        {
            //attempt student login
            using (UnitOfWork UoW = new UnitOfWork(new LabAssistantContext(conName)))
            {
                string hashedPassword = Cryptography.Hash(password);
                Student student = UoW.StudentRepo.FindById(userId);
                if (student != null)
                {
                    //verify
                    if (Cryptography.Verify(password, student.Password))
                    {
                        return student;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public bool Register(Student student, string conName)
        {
            using (UnitOfWork UoW = new UnitOfWork(new LabAssistantContext(conName)))
            {
                //hash password
                student.Password = Cryptography.Hash(student.Password);
                //save to DB
                UoW.StudentRepo.Add(student);
                int result = UoW.Complete();
                if (result != 0)
                    return true;
                else
                    return false;
            }
        }
    }
}