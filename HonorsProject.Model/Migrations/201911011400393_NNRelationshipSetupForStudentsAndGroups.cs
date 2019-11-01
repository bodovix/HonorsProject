namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NNRelationshipSetupForStudentsAndGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupStudents",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        Student_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.Student_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.Student_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupStudents", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.GroupStudents", "Group_Id", "dbo.Groups");
            DropIndex("dbo.GroupStudents", new[] { "Student_Id" });
            DropIndex("dbo.GroupStudents", new[] { "Group_Id" });
            DropTable("dbo.GroupStudents");
        }
    }
}
