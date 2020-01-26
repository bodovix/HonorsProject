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

        public List<Session> GetCurrentSessions(Student student, DateTime date)
        {
            DateTime dateDate = date.Date;
            //get sessions belonging to this student from the sessions group
            //where inputted date is between the start and end dates of the session
            List<Session> results = _entities.Where(sesh => sesh.Group.Students.Any(st => st.Id == student.Id)
                                                && sesh.StartTime <= dateDate
                                                && sesh.EndTime >= dateDate)
                                                    .ToList();
            return results;
        }

        public List<Session> GetCurrentSessions(Group group, DateTime date)
        {
            DateTime dateDate = date.Date;
            //get sessions belonging to this group
            //where inputted date is between the start and end dates of the session
            List<Session> results = _entities.Where(sesh => sesh.Group.Id == group.Id
                                                && sesh.StartTime <= dateDate
                                                && sesh.EndTime >= dateDate)
                                                    .ToList();
            return results;
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

        public List<Session> GetFutureSessions(Student student, DateTime date)
        {
            //get sessions belonging to this student from the sessions group
            //and start date is ahead of inputted date
            DateTime dateDate = date.Date;
            List<Session> results = _entities.Where(sesh => sesh.Group.Students.Any(st => st.Id == student.Id)
                                                && sesh.StartTime > dateDate)
                                                    .ToList();
            return results;
        }

        public List<Session> GetFutureSessions(Group group, DateTime date)
        {
            //get sessions belonging to this group
            //and start date is ahead of inputted date
            DateTime dateDate = date.Date;
            List<Session> results = _entities.Where(sesh => sesh.Group.Id == group.Id
                                                && sesh.StartTime > dateDate)
                                                    .ToList();
            return results;
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

        public List<Session> GetPreviousSessions(Student student, DateTime date)
        {
            //get sessions belonging to this student from the sessions group
            //and end date is behind inputted date
            DateTime dateDate = date.Date;
            List<Session> results = _entities.Where(sesh => sesh.Group.Students.Any(st => st.Id == student.Id)
                                                && sesh.EndTime < dateDate)
                                                    .ToList();
            return results;
        }

        public List<Session> GetPreviousSessions(Group group, DateTime date)
        {
            //get sessions belonging to this group
            //and end date is behind inputted date
            DateTime dateDate = date.Date;
            List<Session> results = _entities.Where(sesh => sesh.Group.Id == group.Id
                                                && sesh.EndTime < dateDate)
                                                    .ToList();
            return results;
        }

        public Session GetSessionWithQuestion(Question selectedQuestion)
        {
            return _entities.Where(s => s.Questions.Any(q => q.Id == selectedQuestion.Id)).FirstOrDefault();
        }
    }
}