using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public SessionRepository(LabAssistantContext context) : base(context)
        {
        }

        public List<Session> GetCurrentSessions(Lecturer lecturer, DateTime date)
        {
            //TODO:Add date filter
            List<Session> results = _entities.Where(s => s.Lecturers.Any(l => l.Id == lecturer.Id)).ToList();
            return results;
        }

        public List<Session> GetCurrentSessions(Group studentGroup, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Session> GetFutureSessions(Lecturer lecturer, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Session> GetFutureSessions(Group studentGroup, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Session> GetPreviousSessions(Lecturer lecturer, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Session> GetPreviousSessions(Group studentGroup, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}