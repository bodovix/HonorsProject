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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Group> Groups { get; set; }
        public List<Question> Questions { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByLecturerID { get; set; }

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
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
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
    }
}