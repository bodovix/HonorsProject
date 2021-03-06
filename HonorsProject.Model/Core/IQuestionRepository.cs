﻿using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Core
{
    public interface IQuestionRepository : IRepository<Question>
    {
        List<Question> GetFromSession(Session session);

        List<Question> GetFromSearchForSession(Session session, string questionSearchTxt);

        List<Question> GetPublicAndStudentQsFromSearchForSession(Session session, Student askingStuent, string questionSearchTxt);

        List<Question> GetAllForStudent(Student user, string searchTxt);

        List<Question> GetAllWithAnswersByLecturer(ISystemUser user, string questionSearchTxt);

        bool CheckNameAlreadyExistsForSession(Question question);
    }
}