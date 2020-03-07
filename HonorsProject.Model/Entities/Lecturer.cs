using HonorsProject.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HonorsProject.Model.Core;
using System.Security.Cryptography;
using HonorsProject.Model.HelperClasses;
using System.Security.Authentication;
using System.Text.RegularExpressions;

namespace HonorsProject.Model.Entities
{
    public class Lecturer : BaseEntity, ISystemUser
    {
        #region Properties

        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsSuperAdmin { get; set; }
        public virtual List<Session> Sessions { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public int CreatedByLecturerId { get; set; }

        #endregion Properties

        public Lecturer()
        {
            Answers = new List<Answer>();
            Sessions = new List<Session>();
        }

        public Lecturer(int id, string name, string email, string password, bool isSuperAdmin, DateTime createdOn, int createdByLecturerId)
        {
            Id = id;
            Name = name;
            IsSuperAdmin = isSuperAdmin;
            Email = email;
            Password = password;
            CreatedOn = createdOn;
            CreatedByLecturerId = createdByLecturerId;
        }

        public ISystemUser Login(int userId, string password, string conName)
        {
            //attempt student login
            using (UnitOfWork UoW = new UnitOfWork(new LabAssistantContext(conName)))
            {
                string hashedPassword = Cryptography.Hash(password);
                Lecturer lecturer = UoW.LecturerRepo.Get(userId);
                if (lecturer != null)
                {
                    //verify
                    if (Cryptography.Verify(password, lecturer.Password))
                    {
                        return lecturer;
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
            UoW.LecturerRepo.Add(this);
            int result = UoW.Complete();
            if (result != 0)
                return true;
            else
                return false;
        }

        public bool AddNewSession(Session session, UnitOfWork u)
        {
            session.CreatedByLecturerId = Id;
            session.CreatedOn = DateTime.Now;
            //creating session this way so constructor can validate it
            if (session.ValidateSession(u))
            {
                u.SessionRepository.Add(session);
                u.Complete();
                return true;
            }
            else
                return false;
        }

        public bool AddNewGroup(Group groupToAdd, UnitOfWork unitOfWork)
        {
            groupToAdd.CreatedByLecturerId = Id;
            groupToAdd.CreatedOn = DateTime.Now;
            //creating session this way so constructor can validate it
            if (groupToAdd.ValidateGroup(unitOfWork))
            {
                unitOfWork.GroupRepository.Add(groupToAdd);
                int rowCount = unitOfWork.Complete();
                if (rowCount > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public List<Session> GetAllMyCurrentSessions(DateTime todaysDate, UnitOfWork unitOfWork)
        {
            return unitOfWork.SessionRepository.GetCurrentSessions(this, todaysDate);
        }

        public List<Session> GetAllMyPreviousSessions(DateTime todaysDate, UnitOfWork unitOfWork)
        {
            return unitOfWork.SessionRepository.GetPreviousSessions(this, todaysDate);
        }

        public List<Session> GetAllMyFutureSessions(DateTime todaysDate, UnitOfWork unitOfWork)
        {
            return unitOfWork.SessionRepository.GetFutureSessions(this, todaysDate);
        }

        public bool AddNewStudent(Student selectedStudent, UnitOfWork unitOfWork)
        {
            selectedStudent.CreatedByLecturerId = Id;
            selectedStudent.CreatedOn = DateTime.Now.Date;
            if (selectedStudent.Validate())
            {
                unitOfWork.StudentRepo.Add(selectedStudent);
                unitOfWork.Complete();
                return true;
            }
            else
                return false;
        }

        public bool AddNewLecturer(Lecturer selectedLecturer, UnitOfWork unitOfWork)
        {
            selectedLecturer.CreatedByLecturerId = Id;
            selectedLecturer.CreatedOn = DateTime.Now.Date;
            if (selectedLecturer.Validate())
            {
                unitOfWork.LecturerRepo.Add(selectedLecturer);
                bool result = (unitOfWork.Complete() > 0) ? true : false;
                if (result)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool AnswerQuestion(Answer selectedAnswer, UnitOfWork unitOfWork)
        {
            bool result = false;
            selectedAnswer.CreatedOn = DateTime.Now;
            selectedAnswer.AnsweredBy = this;
            if (selectedAnswer.ValidateAnswer(unitOfWork))
            {
                unitOfWork.AnswerRepository.Add(selectedAnswer);
                result = (unitOfWork.Complete() > 0) ? true : false;
            }
            return result;
        }

        public bool Validate()
        {
            if (String.IsNullOrEmpty(Name))
                throw new ArgumentException("Name required.");
            if (Name.Length > nameSizeLimit)
                throw new ArgumentException($"Name cannot exceed {nameSizeLimit} characters.");
            if (CreatedOn == null)
                throw new ArgumentException("Date created on required.");
            if (String.IsNullOrEmpty(Email))
                throw new ArgumentException("Email required.");
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(Email);
            if (!match.Success)
                throw new ArgumentException("Email in invalid format.");
            if (String.IsNullOrEmpty(Password))
                throw new ArgumentException("Password required.");
            if (CreatedByLecturerId == 0)
                throw new ArgumentException("Created by id required.");
            return true;
        }

        public bool GenerateNewPasswordHash(ref string optionalPassword, string optionalPassConf)
        {
            if (String.IsNullOrEmpty(optionalPassword))
            {
                char[] stringChars = Cryptography.GenerateRandomString();

                optionalPassword = new string(stringChars);
                Password = Cryptography.Hash(optionalPassword);
                return true;
            }
            else
            {
                ValidatePasswordForManualSet(optionalPassword, optionalPassConf);
                Password = Cryptography.Hash(optionalPassword);
                return true;
            }
        }

        private void ValidatePasswordForManualSet(string optionalPassword, string passwordConf)
        {
            if (String.IsNullOrEmpty(optionalPassword))
                throw new ArgumentException("Password cannot be empty.");
            if (!String.Equals(optionalPassword, passwordConf))
                throw new ArgumentException("Passwords don't match.");
        }

        public bool AskQuestion(Question selectedQuestion, UnitOfWork unitOfWork)
        {
            throw new NotImplementedException("Lecturers cannot ask questions. Please contact support.");
        }
    }
}