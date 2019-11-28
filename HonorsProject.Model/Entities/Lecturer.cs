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
    public class Lecturer : BaseEntity, ISystemUser<Lecturer>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Session> Sessions { get; set; }
        public List<Answer> Answers { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByLecturerId { get; set; }

        public Lecturer()
        {
            Answers = new List<Answer>();
            Sessions = new List<Session>();
        }

        public ISystemUser<Lecturer> Login(int userId, string password, string conName)
        {
            //attempt student login
            using (UnitOfWork UoW = new UnitOfWork(new LabAssistantContext(conName)))
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
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
    }
}