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
    public class Lecturer : BaseSystemUser, ISystemUser<Lecturer>
    {
        public List<Session> Sessions { get; set; }
        public List<Answer> Answers { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByLecturerId { get; set; }

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

        public Lecturer Login(int userId, string password, string conName)
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

        public bool Register(Lecturer lecturer, string conName)
        {
            using (UnitOfWork UoW = new UnitOfWork(new LabAssistantContext(conName)))
            {
                //check id free
                if (UoW.LecturerRepo.Get(lecturer.Id) != null)
                    throw new Exception("ID already owned by Lecturer");
                if (UoW.StudentRepo.Get(lecturer.Id) != null)
                    throw new Exception("ID already owned by Student");
                //hash password
                lecturer.Password = Cryptography.Hash(lecturer.Password);
                //save to DB
                UoW.LecturerRepo.Add(lecturer);
                int result = UoW.Complete();
                if (result != 0)
                    return true;
                else
                    return false;
            }
        }
    }
}