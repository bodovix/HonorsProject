using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Entities
{
    public class Answer : IEntity
    {
        public int Id { get; set; }
        public string AnswerTest { get; set; }
        public Lecturer AnsweredBy { get; set; }
        public Question Question { get; set; }
        public DateTime DateAnswered { get; set; }
        public DateTime CreatedOn { get; set; }

        public Answer()
        {
        }
    }
}