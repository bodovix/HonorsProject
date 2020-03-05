using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(LabAssistantContext context) : base(context)
        {
        }
    }
}