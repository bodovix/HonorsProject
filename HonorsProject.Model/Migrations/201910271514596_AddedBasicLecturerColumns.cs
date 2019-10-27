namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBasicLecturerColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lecturers", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Lecturers", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Lecturers", "Password", c => c.String(nullable: false));
            AddColumn("dbo.Lecturers", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Lecturers", "CreatedByLecturerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lecturers", "CreatedByLecturerId");
            DropColumn("dbo.Lecturers", "CreatedOn");
            DropColumn("dbo.Lecturers", "Password");
            DropColumn("dbo.Lecturers", "Email");
            DropColumn("dbo.Lecturers", "Name");
        }
    }
}
