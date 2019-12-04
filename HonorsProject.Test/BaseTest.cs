using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Test
{
    public class BaseTest
    {
        protected string dbConName;

        public BaseTest()
        {
            dbConName = ConnectionConfigs.TestConfig;
        }

        protected void ClearDatabase()
        {
            using (LabAssistantContext context = new LabAssistantContext(dbConName))
            {
                context.Answers.RemoveRange(context.Answers);
                context.Questions.RemoveRange(context.Questions);
                context.Groups.RemoveRange(context.Groups);
                context.Lecturers.RemoveRange(context.Lecturers);
                context.Sessions.RemoveRange(context.Sessions);
                context.Students.RemoveRange(context.Students);
                context.SaveChanges();
            }
        }

        protected void CreateLoginTestData()
        {
            using (UnitOfWork uow = new UnitOfWork(new LabAssistantContext(dbConName)))
            {
                Lecturer l = new Lecturer(444, "Suzy", "lecturer1@uad.ac.uk", "password", new DateTime(2019, 11, 28, 16, 22, 27, 813), 1234);
                Student s = new Student(1701267, "Gwydion", "1701267@uad.ac.uk", "password", new DateTime(2019, 11, 28, 12, 05, 09, 200), 444);
                l.Register(uow);
                s.Register(uow);
            }
        }

        protected void CreateMySessionTestData(Lecturer lecturer)
        {
            using (UnitOfWork u = new UnitOfWork(new LabAssistantContext(dbConName)))
            {
                //test data cleared each test - need to re  register users
                Lecturer l = lecturer;
                Student s = new Student(1701267, "Gwydion", "1701267@uad.ac.uk", "password", new DateTime(2019, 11, 28, 12, 05, 09, 200), 444);
                l.Register(u);
                s.Register(u);
                List<Lecturer> lecL = new List<Lecturer>();
                lecL.Add(l);
                List<Student> stL = new List<Student>();
                stL.Add(s);
                //group added before session
                Group g = new Group("Computing 19/20", stL, null, new DateTime(2019, 12, 3), 444);
                u.GroupRepository.Add(g);
                //session added with group

                Session sesh = new Session("Week 1", new DateTime(2019, 12, 3), new DateTime(2019, 12, 3), lecL, g, null, new DateTime(2019, 12, 1), 444);
                u.SessionRepository.Add(sesh);
                u.Complete();
            }
        }
    }
}