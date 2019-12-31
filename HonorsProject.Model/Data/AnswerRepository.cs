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

        public List<Answer> GetFromSession(Question question)
        {
            return _entities.Where(a => a.Question.Id == question.Id).ToList();
        }

        public List<Answer> GetFromSearchForQuestion(Question question, string searchTxt)
        {
            if (String.IsNullOrEmpty(searchTxt))
                return GetFromSession(question);
            else
                return _entities.Where(a => (a.Id.ToString().Contains(searchTxt)
                            || a.Name.Contains(searchTxt)
                            || a.AnswerTest.Contains(searchTxt))
                            && a.Question.Id == question.Id).ToList();
        }
    }
}