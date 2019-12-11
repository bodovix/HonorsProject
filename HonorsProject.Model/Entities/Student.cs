﻿using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using HonorsProject.Model.HelperClasses;
using System.Security.Cryptography;
using System.Security.Authentication;
using System.Collections.ObjectModel;

namespace HonorsProject.Model.Entities
{
    public class Student : BaseEntity, ISystemUser
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ObservableCollection<Group> Groups { get; set; }
        public virtual ObservableCollection<Question> Questions { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByLecturerId { get; set; }

        #endregion Properties

        public Student()
        {
            Groups = new ObservableCollection<Group>();
            Questions = new ObservableCollection<Question>();
        }

        public Student(int id, string name, string email, string password, DateTime createdOn, int createdByLecturerID)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            CreatedOn = createdOn;
            CreatedByLecturerId = createdByLecturerID;
            Groups = new ObservableCollection<Group>();
            Questions = new ObservableCollection<Question>();
        }

        public ISystemUser Login(int userId, string password, string conName)
        {
            //attempt student login
            using (UnitOfWork UoW = new UnitOfWork(new LabAssistantContext(conName)))
            {
                string hashedPassword = Cryptography.Hash(password);
                Student student = UoW.StudentRepo.FindById(userId);
                if (student != null)
                {
                    //verify
                    if (Cryptography.Verify(password, student.Password))
                    {
                        return student;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public bool Register(IUnitOfWork UoW)
        {
            //check id free
            if (UoW.LecturerRepo.Get(Id) != null)
                throw new Exception("ID already owned by Lecturer");
            if (UoW.StudentRepo.Get(Id) != null)
                throw new Exception("ID already owned by Student");
            //hash password
            Password = Cryptography.Hash(Password);
            //save to DB
            UoW.StudentRepo.Add(this);
            int result = UoW.Complete();
            if (result != 0)
                return true;
            else
                return false;
        }

        public bool AddNewSession(Session selectedSession, IUnitOfWork u)
        {
            throw new NotImplementedException("Students Cannot Create New Sessions.");
        }

        public List<Session> GetAllMyCurrentSessions(DateTime todaysDate, IUnitOfWork unitOfWork)
        {
            return unitOfWork.SessionRepository.GetCurrentSessions(this, todaysDate.Date);
        }

        public List<Session> GetAllMyPreviousSessions(DateTime todaysDate, IUnitOfWork unitOfWork)
        {
            return unitOfWork.SessionRepository.GetPreviousSessions(this, todaysDate.Date);
        }

        public List<Session> GetAllMyFutureSessions(DateTime todaysDate, IUnitOfWork unitOfWork)
        {
            return unitOfWork.SessionRepository.GetFutureSessions(this, todaysDate.Date);
        }

        public bool RemoveGroup(Group groupToRemve, UnitOfWork u, ref string feedackMsg)
        {
            if (Groups.Contains(groupToRemve))
            {
                Groups.Remove(groupToRemve);
                u.Complete();
                return true;
            }
            else
            {
                feedackMsg = "Student Not in Selected Group. Refresh and try again.";
                return false;
            }
        }
    }
}