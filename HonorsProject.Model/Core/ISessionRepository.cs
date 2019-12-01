using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface ISessionRepository : IRepository<Session>
    {
        List<Session> GetCurrentSessions(Lecturer lecturer, DateTime date);

        List<Session> GetFutureSessions(Lecturer lecturer, DateTime date);

        List<Session> GetPreviousSessions(Lecturer lecturer, DateTime date);

        List<Session> GetCurrentSessions(Group studentGroup, DateTime date);

        List<Session> GetFutureSessions(Group studentGroup, DateTime date);

        List<Session> GetPreviousSessions(Group studentGroup, DateTime date);
    }
}