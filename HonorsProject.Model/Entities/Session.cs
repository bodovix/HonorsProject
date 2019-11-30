using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Entities
{
    public class Session : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Lecturer> Lecturers { get; set; }
        public Group Group { get; set; }
        public List<Question> Questions { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByLecturerId { get; set; }

        public Session()
        {
            Questions = new List<Question>();
            Lecturers = new List<Lecturer>();
        }

        public Session(Session sessionToCopy)
        {
            Id = sessionToCopy.Id;
            Name = sessionToCopy.Name;
            StartTime = sessionToCopy.StartTime;
            EndTime = sessionToCopy.EndTime;
            Lecturers = sessionToCopy.Lecturers;
            Group = sessionToCopy.Group;
            Questions = sessionToCopy.Questions;
            CreatedOn = DateTime.Now;
            CreatedByLecturerId = sessionToCopy.CreatedByLecturerId;
            Questions = new List<Question>();
            Lecturers = new List<Lecturer>();
        }

        public bool ValidateSession()
        {
            if (String.IsNullOrEmpty(this.Name))
                throw new ArgumentException("Name required.");
            if (StartTime == null)
                throw new ArgumentException("Start time required.");
            if (EndTime == null)
                throw new ArgumentException("End time required.");
            if (Group == null)
                throw new ArgumentException("Sessions must belong to a group.");
            if (CreatedByLecturerId == 0)
                throw new ArgumentException("Session created by Id required.");
            return true;
        }
    }
}