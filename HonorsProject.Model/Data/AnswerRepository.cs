using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        public AnswerRepository(LabAssistantContext context) : base(context)
        {
        }

        public List<Answer> GetFromSession(Session selectedSession)
        {
            return _entities.Where(a => a.Question.Id == selectedSession.Id).ToList();
        }
    }
}