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
        public virtual List<Student> Students { get; set; }
        public virtual List<Session> Sessions { get; set; }
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

        public bool ValidateGroup()
        {
            //ID auto incremented by EF
            if (String.IsNullOrEmpty(Name))
                throw new ArgumentException("Group name required.");
            if (CreatedOn == null)
                throw new ArgumentException("Group created on date required.");
            if (CreatedByLecturerId == 0)
                throw new ArgumentException("Group created by Id required.");

            return true;
        }
    }
}