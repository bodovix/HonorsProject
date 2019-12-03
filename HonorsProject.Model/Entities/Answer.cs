using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Entities
{
    public class Answer : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AnswerTest { get; set; }
        public bool WasHelpfull { get; set; }
        public virtual Lecturer AnsweredBy { get; set; }
        public virtual Question Question { get; set; }

        public DateTime CreatedOn { get; set; }

        public Answer()
        {
        }
    }
}