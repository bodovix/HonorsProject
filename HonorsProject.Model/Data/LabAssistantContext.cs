using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HonorsProject.Model.Entities;

namespace HonorsProject.Model.Data
{
    public partial class LabAssistantContext : DbContext
    {
        public virtual DbSet<Lecturer> Lecturers { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }

        public LabAssistantContext(string conectionName)
            : base(conectionName)
        {
            //live: "name=LabAssistantContext"
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}