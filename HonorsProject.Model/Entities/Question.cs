using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Entities
{
    public class Question : IEntity
    {
        public int Id { get; set; }
        public DateTime TimeAsked { get; set; }
        public string QuestionText { get; set; }
        public bool IsResolved { get; set; }
        public List<Answer> Answers { get; set; }
        public Student AskedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }
    }
}