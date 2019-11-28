namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LectureIDNowManual : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "AnsweredBy_Id", "dbo.Lecturers");
            DropForeignKey("dbo.SessionLecturers", "Lecturer_Id", "dbo.Lecturers");
            DropPrimaryKey("dbo.Lecturers");
            AlterColumn("dbo.Lecturers", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Lecturers", "Id");
            AddForeignKey("dbo.Answers", "AnsweredBy_Id", "dbo.Lecturers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SessionLecturers", "Lecturer_Id", "dbo.Lecturers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SessionLecturers", "Lecturer_Id", "dbo.Lecturers");
            DropForeignKey("dbo.Answers", "AnsweredBy_Id", "dbo.Lecturers");
            DropPrimaryKey("dbo.Lecturers");
            AlterColumn("dbo.Lecturers", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Lecturers", "Id");
            AddForeignKey("dbo.SessionLecturers", "Lecturer_Id", "dbo.Lecturers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Answers", "AnsweredBy_Id", "dbo.Lecturers", "Id", cascadeDelete: true);
        }
    }
}
