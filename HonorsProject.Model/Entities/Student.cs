using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Entities
{
    public class Student : IEntity, ISystemUser
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

        public void Login()
        {
            throw new NotImplementedException();
        }
    }
}