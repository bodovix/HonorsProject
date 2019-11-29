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
            using (UnitOfWork Uow = new UnitOfWork(new LabAssistantContext(dbConName)))
            {
                Lecturer l = new Lecturer(444, "Suzy", "lecturer1@uad.ac.uk", "password", new DateTime(2019, 11, 28, 16, 22, 27, 813), 1234);
                Student s = new Student(1701267, "Gwydion", "1701267@uad.ac.uk", "password", new DateTime(2019, 11, 28, 12, 05, 09, 200), 444);
                l.Register(l, dbConName);
                s.Register(s, dbConName);
            }
        }
    }
}