namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NNRelationshipSetupForLecturersandSession : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SessionLecturers",
                c => new
                    {
                        Session_Id = c.Int(nullable: false),
                        Lecturer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Session_Id, t.Lecturer_Id })
                .ForeignKey("dbo.Sessions", t => t.Session_Id, cascadeDelete: true)
                .ForeignKey("dbo.Lecturers", t => t.Lecturer_Id, cascadeDelete: true)
                .Index(t => t.Session_Id)
                .Index(t => t.Lecturer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SessionLecturers", "Lecturer_Id", "dbo.Lecturers");
            DropForeignKey("dbo.SessionLecturers", "Session_Id", "dbo.Sessions");
            DropIndex("dbo.SessionLecturers", new[] { "Lecturer_Id" });
            DropIndex("dbo.SessionLecturers", new[] { "Session_Id" });
            DropTable("dbo.SessionLecturers");
        }
    }
}
