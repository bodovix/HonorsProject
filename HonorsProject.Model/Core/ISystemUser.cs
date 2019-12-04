using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        bool AddNewSession(Session selectedSession, string conName);

        List<Session> GetAllMyCurrentSessions(string conName);

        List<Session> GetAllMyPreviousSessions(string conName);

        List<Session> GetAllMyFutureSessions(string conName);
    }
}