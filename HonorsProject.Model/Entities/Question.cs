using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;

namespace HonorsProject.Model.Entities
{
    public class Question : BaseEntity
    {
        public DateTime TimeAsked { get; set; }
        public string QuestionText { get; set; }
        public bool IsResolved { get; set; }
        public virtual Session Session { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public virtual Student AskedBy { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }

        public Question(DateTime timeAsked, string name, string questionText, Session session, Student askedBy, DateTime createdOn)
        {
            //id and Answers list are sorted by EF. created/asked dates injected for testability
            TimeAsked = timeAsked;
            Name = name;
            QuestionText = questionText;
            IsResolved = false;
            Session = session;
            Answers = new List<Answer>();
            AskedBy = askedBy;
            CreatedOn = createdOn;
        }
    }
}