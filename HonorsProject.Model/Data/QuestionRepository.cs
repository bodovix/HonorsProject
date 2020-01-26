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

        public List<Question> GetFromSearchForSession(Session session, string searchTxt)
        {
            if (String.IsNullOrEmpty(searchTxt))
                return GetFromSession(session);
            else
                return _entities.Where(q => (q.Id.ToString().Contains(searchTxt)
                                        || q.Name.Contains(searchTxt)
                                        || q.QuestionText.Contains(searchTxt)
                                        || q.Id.ToString().Contains(searchTxt))
                                        && q.Session.Id == session.Id).ToList();
        }

        public List<Question> GetAllForStudent(Student user, string searchTxt)
        {
            if (String.IsNullOrEmpty(searchTxt))
                return _entities.Where(q => q.AskedBy.Id == user.Id).ToList();
            else
                return _entities.Where(q => q.AskedBy.Id == user.Id
                                        && (q.Name.Contains(searchTxt)
                                        || q.QuestionText.Contains(searchTxt)
                                        || q.Id.ToString().Contains(searchTxt))).ToList();
        }

        public List<Question> GetAllWithAnswersByLecturer(ISystemUser user, string questionSearchTxt)
        {
            if (String.IsNullOrEmpty(questionSearchTxt))
                return _entities.Where(q => q.Answers.Any(a => a.AnsweredBy.Id == user.Id)).ToList();
            else
                return _entities.Where(q => q.Answers.Any(a => a.AnsweredBy.Id == user.Id)
                                        && (q.Id.ToString().Contains(questionSearchTxt)
                                        || q.Name.Contains(questionSearchTxt)
                                        || q.QuestionText.Contains(questionSearchTxt))).ToList();
        }

        public bool CheckNameAlreadyExistsForSession(Question question)
        {
            return (_entities.Where(q =>
                               (q.Session.Id == question.Session.Id && q.Id != question.Id))
                               .Count(a => a.Name.Equals(question.Name)) > 0) ? true : false;
        }
    }
}