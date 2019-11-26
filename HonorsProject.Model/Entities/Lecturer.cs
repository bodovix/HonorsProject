using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Entities
{
    public class Lecturer : IEntity, ISystemUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Session> Sessions { get; set; }
        public List<Answer> Answers { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByLecturerId { get; set; }

        public Lecturer()
        {
            Answers = new List<Answer>();
            Sessions = new List<Session>();
        }

        public void Login()
        {
            throw new NotImplementedException();
        }
    }
}