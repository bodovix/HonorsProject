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
        public string ImageLocation { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }

        public Question(Student student)
        {
            AskedBy = student;
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

        public bool Validate(UnitOfWork u)
        {
            if (TimeAsked == null)
                throw new ArgumentException("Time asked required.");
            if (String.IsNullOrEmpty(Name))
                throw new ArgumentException("Name required.");
            if (u.QuestionRepository.CheckNameAlreadyExistsForSession(this))
                throw new ArgumentException("Name already exists for this session.");

            if (Name.Length > nameSizeLimit)
                throw new ArgumentException($"Name cannot exceed {nameSizeLimit} characters.");
            if (String.IsNullOrEmpty(QuestionText))
                throw new ArgumentException("Question text required.");
            if (Session == null)
                throw new ArgumentException("Session required.");
            if (Session.Id == 0)
                throw new ArgumentException("Session required.");
            if (AskedBy == null)
                throw new ArgumentException("Asked by required.");
            if (AskedBy.Id == 0)
                throw new ArgumentException("Asked by required.");
            if (CreatedOn == null)
                throw new ArgumentException("Created on required.");
            return true;
        }
    }
}