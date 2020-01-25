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
        public string AnswerTest { get; set; }
        public bool WasHelpfull { get; set; }
        public virtual Lecturer AnsweredBy { get; set; }
        public virtual Question Question { get; set; }
        public string ImageLocation { get; set; }

        public Answer()
        {
        }

        public Answer(Lecturer lecturer)
        {
            AnsweredBy = lecturer;
        }

        public Answer(string name, string answerTest, bool wasHelpfull, Lecturer answeredBy, Question question, DateTime createdOn)
        {
            //ID handled by EF
            Name = name;
            AnswerTest = answerTest;
            WasHelpfull = wasHelpfull;
            AnsweredBy = answeredBy;
            Question = question;
            CreatedOn = createdOn;
        }

        public bool ValidateAnswer()
        {
            if (String.IsNullOrEmpty(Name))
                throw new ArgumentException("Answer name required.");
            if (Name.Length > 50)
                throw new ArgumentException("Name cannot exceed 20 characters.");
            if (String.IsNullOrEmpty(AnswerTest))
                throw new ArgumentException("Answer text required.");
            if (Question == null)
                throw new ArgumentException("Answers must belong to a question.");
            if (Question.Id == 0)
                throw new ArgumentException("Answers must belong to a question.");
            if (AnsweredBy == null)
                throw new ArgumentException("Answered by required.");
            if (CreatedOn == null)
                throw new ArgumentException("Created on required.");
            return true;
        }
    }
}