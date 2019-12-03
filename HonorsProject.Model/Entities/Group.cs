using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Entities
{
    public class Group : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Student> Students { get; set; }
        public virtual List<Session> Sessions { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByLecturerId { get; set; }

        public Group()
        {
            Students = new List<Student>();
            Sessions = new List<Session>();
        }

        public Group(string name, List<Student> students, List<Session> sessions, DateTime createdOn, int createdByLecturerId)
        {
            Name = name;
            Students = students;
            Sessions = sessions;
            CreatedOn = createdOn;
            CreatedByLecturerId = createdByLecturerId;
        }
    }
}