using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface ICommentRepository : IRepository<Comment>
    {
        List<Comment> GetCommentsForQuestion(Question selectedQuestion);
    }
}