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

        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return DefaultDate(ref _startTime); }
            set
            {
                _startTime = DefaultDate(ref value);
            }
        }

        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return DefaultDate(ref _endTime); }
            set { _endTime = DefaultDate(ref value); }
        }

        public virtual List<Lecturer> Lecturers { get; set; }
        public virtual Group Group { get; set; }
        public virtual List<Question> Questions { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByLecturerId { get; set; }

        public Session()
        {
            Questions = new List<Question>();
            Lecturers = new List<Lecturer>();
        }

        public Session(string name, DateTime startTime, DateTime endTime, List<Lecturer> lecturers, Group group, List<Question> questions, DateTime createdOn, int createdByLecturerId)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            Lecturers = lecturers;
            Group = group;
            Questions = questions;
            CreatedOn = createdOn;
            CreatedByLecturerId = createdByLecturerId;
        }

        private DateTime DefaultDate(ref DateTime value)
        {
            //if date is default set it to today
            if (DateTime.Compare(value.Date, new DateTime(0001, 01, 01)) == 0)
                value = DateTime.Now.Date;

            return value;
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