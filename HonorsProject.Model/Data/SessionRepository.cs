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
            DateTime dateDate = date.Date;
            //get sessions belonging to this lecturer or was created by
            //where inputted date is between the start and end dates of the session
            List<Session> results = _entities.Where(s => (s.Lecturers.Any(l => l.Id == lecturer.Id) || s.CreatedByLecturerId == lecturer.Id)
                                                && s.StartTime <= dateDate
                                                && s.EndTime >= dateDate)
                                                    .ToList();
            return results;
        }

        public List<Session> GetCurrentSessions(Group studentGroup, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Session> GetFutureSessions(Lecturer lecturer, DateTime date)
        {
            //get sessions belonging to this lecturer or was created by
            //and start date is ahead of inputted date
            DateTime dateDate = date.Date;
            List<Session> results = _entities.Where(s => (s.Lecturers.Any(l => l.Id == lecturer.Id) || s.CreatedByLecturerId == lecturer.Id)
                                                && s.StartTime > dateDate)
                                                    .ToList();
            return results;
        }

        public List<Session> GetFutureSessions(Group studentGroup, DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Session> GetPreviousSessions(Lecturer lecturer, DateTime date)
        {
            //get sessions belonging to this lecturer
            //and end date is behind inputted date
            DateTime dateDate = date.Date;
            List<Session> results = _entities.Where(s => (s.Lecturers.Any(l => l.Id == lecturer.Id) || s.CreatedByLecturerId == lecturer.Id)
                                                && s.EndTime < dateDate)
                                                    .ToList();
            return results;
        }

        public List<Session> GetPreviousSessions(Group studentGroup, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}