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
    }
}