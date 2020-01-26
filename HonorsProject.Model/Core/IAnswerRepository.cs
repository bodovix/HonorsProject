using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        List<Answer> GetFromSession(Question question);

        List<Answer> GetFromSearchForQuestion(Question question, string answerSearchTxt);

        bool CheckNameAlreadyExistsForQuestion(Answer answer);
    }
}