using HonorsProject.Model.Data;
using HonorsProject.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                //clear data
                context.Answers.RemoveRange(context.Answers);
                context.Questions.RemoveRange(context.Questions);
                context.Groups.RemoveRange(context.Groups);
                context.Lecturers.RemoveRange(context.Lecturers);
                context.Sessions.RemoveRange(context.Sessions);
                context.Students.RemoveRange(context.Students);
                //reset auto increment ids
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Answers', RESEED, 0)");
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Groups', RESEED, 0)");
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Questions', RESEED, 0)");
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Sessions', RESEED, 0)");

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
                Lecturer l2 = new Lecturer(555, "Gavin Hales", "gav@test.com", "password", DateTime.Now.AddYears(-2), 444);
                Student s = new Student(1701267, "Gwydion", "1701267@uad.ac.uk", "password", DateTime.Now.AddYears(-1), 444);
                l.Register(u);
                l2.Register(u);
                s.Register(u);
                ObservableCollection<Lecturer> lecL = new ObservableCollection<Lecturer>();
                lecL.Add(l);
                lecL.Add(l2);
                List<Student> stL = new List<Student>();
                stL.Add(s);
                //group added before session
                Group g = new Group("Computing 19/20", stL, null, DateTime.Now.AddMonths(-6), 444);
                Group g2 = new Group("Ethical Hacking 19/20", stL, null, DateTime.Now.AddMonths(-5), 555);
                u.GroupRepository.Add(g);
                u.GroupRepository.Add(g2);
                //session added with group
                //active session
                Session todaysSesh = new Session("Todays Sesh", DateTime.Now.Date, DateTime.Now.Date, lecL, g, null, DateTime.Now, 444);
                Session longTermSesh = new Session("Long term Sesh", DateTime.Now.AddDays(-1).Date, DateTime.Now.AddDays(1).Date, lecL, g, null, DateTime.Now, 444);
                Session previousSesh = new Session("Previous Sesh", DateTime.Now.AddMonths(-1).Date, DateTime.Now.AddMonths(-1).Date, lecL, g, null, DateTime.Now.AddMonths(-2), 444);
                Session futureSesh = new Session("Future Sesh", DateTime.Now.AddMonths(1).Date, DateTime.Now.AddMonths(1).Date, lecL, g, null, DateTime.Now, 444);
                u.SessionRepository.Add(todaysSesh);
                u.SessionRepository.Add(longTermSesh);
                u.SessionRepository.Add(previousSesh);
                u.SessionRepository.Add(futureSesh);
                u.Complete();
            }
        }
    }
}