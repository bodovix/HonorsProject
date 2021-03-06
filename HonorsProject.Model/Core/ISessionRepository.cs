﻿using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface ISessionRepository : IRepository<Session>
    {
        List<Session> GetCurrentSessions(Group group, DateTime date);

        List<Session> GetFutureSessions(Group group, DateTime date);

        List<Session> GetPreviousSessions(Group group, DateTime date);

        List<Session> GetCurrentSessions(Lecturer lecturer, DateTime date);

        List<Session> GetFutureSessions(Lecturer lecturer, DateTime date);

        List<Session> GetPreviousSessions(Lecturer lecturer, DateTime date);

        List<Session> GetCurrentSessions(Student studentGroup, DateTime date);

        List<Session> GetFutureSessions(Student studentGroup, DateTime date);

        List<Session> GetPreviousSessions(Student studentGroup, DateTime date);

        List<Session> GetTopXWithSearchForGroup(Group group, string searchTxt, int rowLimit);

        Session GetSessionWithQuestion(Question selectedQuestion);

        bool CheckSessionNameAlreadyExistsForGroup(Session session);
    }
}