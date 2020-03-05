using HonorsProject.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Entities
{
    public class Comment : BaseEntity
    {
        public string CommentText { get; set; }
        public string PostedByName { get; set; }
        public int PostedById { get; set; }
        public Question Quesetion { get; set; }

        public Comment()
        {
        }

        public Comment(string commentText, string postedByName, int postedById, Question question)
        {
            CommentText = commentText;
            PostedByName = postedByName;
            PostedById = postedById;
            Quesetion = question;
            CreatedOn = DateTime.Now;
        }

        public bool Validate()
        {
            if (String.IsNullOrEmpty(CommentText))
                throw new ArgumentException("Cannot post empty comment.");
            if (PostedById == 0)
                throw new ArgumentException("Cannot be posted by user with id 0.");
            if (String.IsNullOrEmpty(PostedByName))
                throw new ArgumentException("Poster name required.");
            if (Quesetion == null)
                throw new ArgumentException("Question cannot be null.");
            if (Quesetion.Id == 0)
                throw new ArgumentException("Question must be saved first.");
            if (CreatedOn == null)
                throw new ArgumentException("Comment created on date cannot be null.");
            return true;
        }
    }
}