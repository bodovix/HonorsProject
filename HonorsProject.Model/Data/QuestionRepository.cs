using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(LabAssistantContext context) : base(context)
        {
        }

        public List<Question> GetFromSession(Session session)
        {
            return _entities.Where(q => q.Session.Id == session.Id).ToList();
        }
    }
}