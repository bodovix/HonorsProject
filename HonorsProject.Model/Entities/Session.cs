using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Entities
{
    public class Session : IEntity
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
    }
}