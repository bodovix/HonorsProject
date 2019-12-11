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

namespace HonorsProject.Model.Entities
{
    public class Lecturer : BaseEntity, ISystemUser
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual List<Session> Sessions { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByLecturerId { get; set; }

        #endregion Properties

        public Lecturer()
        {
            Answers = new List<Answer>();
            Sessions = new List<Session>();
        }

        public Lecturer(int id, string name, string email, string password, DateTime createdOn, int createdByLecturerId)
        {
            Id = id;
            Name = name;
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

        public bool AddNewSession(Session session, IUnitOfWork u)
        {
            session.CreatedByLecturerId = Id;
            session.CreatedOn = DateTime.Now;
            //creating session this way so constructor can validate it
            if (session.ValidateSession())
            {
                u.SessionRepository.Add(session);
                u.Complete();
                return true;
            }
            else
                return false;
        }

        public List<Session> GetAllMyCurrentSessions(DateTime todaysDate, IUnitOfWork unitOfWork)
        {
            return unitOfWork.SessionRepository.GetCurrentSessions(this, todaysDate);
        }

        public List<Session> GetAllMyPreviousSessions(DateTime todaysDate, IUnitOfWork unitOfWork)
        {
            return unitOfWork.SessionRepository.GetPreviousSessions(this, todaysDate);
        }

        public List<Session> GetAllMyFutureSessions(DateTime todaysDate, IUnitOfWork unitOfWork)
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

        public bool GenerateNewPasswordHash(ref string optionalPassword)
        {
            if (String.IsNullOrEmpty(optionalPassword))
            {
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                char[] stringChars = new char[8];
                Random random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                    stringChars[i] = chars[random.Next(chars.Length)];

                string randomPasswordTxt = new string(stringChars);
                Password = Cryptography.Hash(randomPasswordTxt);
                return true;
            }
            else
            {
                Cryptography.Hash(optionalPassword);
                return true;
            }
        }
    }
}