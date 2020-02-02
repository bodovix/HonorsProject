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

        public bool CheckSessionNameAlreadyExistsForGroup(Session session)
        {
            //count all sessions in the selected session group where the name matches but ignore this one.
            //return true if > 0. false if <= 0
            return (_entities.Where(s =>
                                (s.Name.Equals(session.Name)
                                && s.Id != session.Id
                                && s.Group.Id == session.Group.Id)
                                ).Count() > 0) ? true : false;
        }

        public List<Session> GetCurrentSessions(Lecturer lecturer, DateTime dateTime)
        {
            //get sessions belonging to this lecturer or was created by
            //where inputted date is between the start and end dates of the session
            List<Session> results = _entities.Where(s => (s.Lecturers.Any(l => l.Id == lecturer.Id) || s.CreatedByLecturerId == lecturer.Id)
                                                && s.StartTime <= dateTime
                                                && s.EndTime >= dateTime)
                                                    .ToList();
            return results;
        }

        public List<Session> GetCurrentSessions(Student student, DateTime dateTime)
        {
            //get sessions belonging to this student from the sessions group
            //where inputted date is between the start and end dates of the session
            List<Session> results = _entities.Where(sesh => sesh.Group.Students.Any(st => st.Id == student.Id)
                                                && sesh.StartTime <= dateTime
                                                && sesh.EndTime >= dateTime)
                                                    .ToList();
            return results;
        }

        public List<Session> GetCurrentSessions(Group group, DateTime dateTime)
        {
            //get sessions belonging to this group
            //where inputted date is between the start and end dates of the session
            List<Session> results = _entities.Where(sesh => sesh.Group.Id == group.Id
                                                && sesh.StartTime <= dateTime
                                                && sesh.EndTime >= dateTime)
                                                    .ToList();
            return results;
        }

        public List<Session> GetFutureSessions(Lecturer lecturer, DateTime dateTime)
        {
            //get sessions belonging to this lecturer or was created by
            //and start date is ahead of inputted date
            List<Session> results = _entities.Where(s => (s.Lecturers.Any(l => l.Id == lecturer.Id) || s.CreatedByLecturerId == lecturer.Id)
                                                && s.StartTime > dateTime)
                                                    .ToList();
            return results;
        }

        public List<Session> GetFutureSessions(Student student, DateTime dateTime)
        {
            //get sessions belonging to this student from the sessions group
            //and start date is ahead of inputted date
            List<Session> results = _entities.Where(sesh => sesh.Group.Students.Any(st => st.Id == student.Id)
                                                && sesh.StartTime > dateTime)
                                                    .ToList();
            return results;
        }

        public List<Session> GetFutureSessions(Group group, DateTime dateTime)
        {
            //get sessions belonging to this group
            //and start date is ahead of inputted date
            List<Session> results = _entities.Where(sesh => sesh.Group.Id == group.Id
                                                && sesh.StartTime > dateTime)
                                                    .ToList();
            return results;
        }

        public List<Session> GetPreviousSessions(Lecturer lecturer, DateTime dateTime)
        {
            //get sessions belonging to this lecturer
            //and end date is behind inputted date
            List<Session> results = _entities.Where(s => (s.Lecturers.Any(l => l.Id == lecturer.Id) || s.CreatedByLecturerId == lecturer.Id)
                                                && s.EndTime < dateTime)
                                                    .ToList();
            return results;
        }

        public List<Session> GetPreviousSessions(Student student, DateTime dateTime)
        {
            //get sessions belonging to this student from the sessions group
            //and end date is behind inputted date
            List<Session> results = _entities.Where(sesh => sesh.Group.Students.Any(st => st.Id == student.Id)
                                                && sesh.EndTime < dateTime)
                                                    .ToList();
            return results;
        }

        public List<Session> GetPreviousSessions(Group group, DateTime dateTime)
        {
            //get sessions belonging to this group
            //and end date is behind inputted date
            List<Session> results = _entities.Where(sesh => sesh.Group.Id == group.Id
                                                && sesh.EndTime < dateTime)
                                                    .ToList();
            return results;
        }

        public Session GetSessionWithQuestion(Question selectedQuestion)
        {
            return _entities.Where(s => s.Questions.Any(q => q.Id == selectedQuestion.Id)).FirstOrDefault();
        }

        public List<Session> GetTopXWithSearchForGroup(Group group, string searchTxt, int rowLimit)
        {
            if (group == null)
                return null;
            if (group.Id == 0)
                return null;
            if (String.IsNullOrEmpty(searchTxt))
            {
                //no search string
                if (rowLimit == 0)
                    return _entities.Where(s => s.Group.Id == group.Id).ToList();//no row limit
                else
                    return _entities.Where(s => s.Group.Id == group.Id).Take(rowLimit).ToList();//include row limit
            }
            else
            {
                //include search string
                //no search
                if (rowLimit == 0)
                    return _entities.Where(s => s.Group.Id == group.Id && (s.Id.ToString().Contains(searchTxt)
                                                    || s.Name.Contains(searchTxt))).ToList();//no row limit
                else
                    return _entities.Where(s => s.Group.Id == group.Id && (s.Id.ToString().Contains(searchTxt)
                                                    || s.Name.Contains(searchTxt))).Take(rowLimit).ToList();//include row limit
            }
        }
    }
}