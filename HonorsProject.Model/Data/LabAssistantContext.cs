namespace HonorsProject.Model.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using HonorsProject.Model.Entities;

    public partial class LabAssistantContext : DbContext
    {
        public virtual DbSet<Lecturer> Lecturers { get; set; }

        public LabAssistantContext()
            : base("name=LabAssistantContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}