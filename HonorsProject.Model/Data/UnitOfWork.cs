﻿using HonorsProject.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties

        private LabAssistantContext _context;

        LabAssistantContext IUnitOfWork.Context
        {
            get { return _context; }
            set { _context = value; }
        }

        public ILecturerRepository LecturerRepo { get; private set; }
        public IStudentRepository StudentRepo { get; private set; }
        public ISessionRepository SessionRepository { get; private set; }
        public IQuestionRepository QuestionRepository { get; private set; }
        public IAnswerRepository AnswerRepository { get; private set; }
        public IGroupRepository GroupRepository { get; private set; }

        #endregion Properties

        public UnitOfWork(LabAssistantContext context)
        {
            _context = context;
            LecturerRepo = new LecturerRepository(_context);
            StudentRepo = new StudentRepository(_context);
            SessionRepository = new SessionRepository(_context);
            QuestionRepository = new QuestionRepository(_context);
            AnswerRepository = new AnswerRepository(_context);
            GroupRepository = new GroupRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}