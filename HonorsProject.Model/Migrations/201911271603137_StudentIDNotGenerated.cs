namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentIDNotGenerated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "AskedBy_Id", "dbo.Students");
            DropForeignKey("dbo.GroupStudents", "Student_Id", "dbo.Students");
            DropPrimaryKey("dbo.Students");
            AlterColumn("dbo.Students", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Students", "Id");
            AddForeignKey("dbo.Questions", "AskedBy_Id", "dbo.Students", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GroupStudents", "Student_Id", "dbo.Students", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupStudents", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Questions", "AskedBy_Id", "dbo.Students");
            DropPrimaryKey("dbo.Students");
            AlterColumn("dbo.Students", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Students", "Id");
            AddForeignKey("dbo.GroupStudents", "Student_Id", "dbo.Students", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Questions", "AskedBy_Id", "dbo.Students", "Id", cascadeDelete: true);
        }
    }
}
