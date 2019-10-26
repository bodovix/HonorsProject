using HonorsProject.Model.Core;
using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface IUnitOfWork
    {
        LabAssistantContext _context { get; }
        ILecturerRepository LecturerRepo { get; }
        IStudentRepository StudentRepo { get; }
        ISessionRepository SessionRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        IGroupRepository GroupRepository { get; }

        int Complete();

        void Dispose();
    }
}