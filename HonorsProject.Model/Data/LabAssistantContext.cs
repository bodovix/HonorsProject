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

        //Default to live database
        public LabAssistantContext(string conectionName)
            : base(conectionName)
        {
            //live: "name=LabAssistantContext"
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //One Group can have many Sessions
            modelBuilder.Entity<Group>()
                .HasMany<Session>(g => g.Sessions)
                .WithRequired(s => s.Group)
                .WillCascadeOnDelete(true);

            //One Student can have Many Questions
            modelBuilder.Entity<Student>()
                .HasMany<Question>(s => s.Questions)
                .WithRequired(q => q.AskedBy)
                .WillCascadeOnDelete(true);

            //question
            modelBuilder.Entity<Question>()
                .Property(q => q.QuestionText)
                .IsRequired();
            modelBuilder.Entity<Question>()
                .HasMany<Answer>(p => p.Answers)
                .WithRequired(a => a.Question)
                .WillCascadeOnDelete(true);
            //Answer
            modelBuilder.Entity<Answer>()
                .Property(q => q.AnswerTest)
                .IsRequired();
            //Lecturer
            modelBuilder.Entity<Lecturer>()
                .HasMany<Answer>(l => l.Answers)
                .WithRequired(a => a.AnsweredBy)
                .WillCascadeOnDelete(true);
        }
    }
}