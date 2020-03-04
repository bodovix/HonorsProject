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
        public Question Quesetion { get; set; }

        public Comment()
        {
        }

        public Comment(string commentText, string postedByName, Question quesetion)
        {
            CommentText = commentText;
            PostedByName = postedByName;
            Quesetion = quesetion;
        }
    }
}