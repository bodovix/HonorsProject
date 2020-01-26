using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;

namespace HonorsProject.Model.Core
{
    public interface ISystemUser
    {
        int Id { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        DateTime CreatedOn { get; set; }
        int CreatedByLecturerId { get; set; }

        ISystemUser Login(int userId, string password, string conName);

        bool Register(IUnitOfWork unitOfWork);

        bool AddNewSession(Session selectedSession, UnitOfWork unitOfWork);

        List<Session> GetAllMyCurrentSessions(DateTime todaysDate, UnitOfWork unitOfWork);

        List<Session> GetAllMyPreviousSessions(DateTime todaysDate, UnitOfWork unitOfWork);

        List<Session> GetAllMyFutureSessions(DateTime todaysDate, UnitOfWork unitOfWork);

        bool GenerateNewPasswordHash(ref string optionalPassword, string optionalPasswordConfirmation);

        bool AddNewGroup(Group selectedGroup, UnitOfWork unitOfWork);

        bool AnswerQuestion(Answer selectedAnswer, UnitOfWork unitOfWork);

        bool AskQuestion(Question selectedQuestion, UnitOfWork unitOfWork);
    }
}