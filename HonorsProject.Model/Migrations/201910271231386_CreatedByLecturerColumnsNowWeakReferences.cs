namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedByLecturerColumnsNowWeakReferences : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "CreatedBy_Id", "dbo.Lecturers");
            DropForeignKey("dbo.Students", "CreatedBy_Id", "dbo.Lecturers");
            DropIndex("dbo.Groups", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Students", new[] { "CreatedBy_Id" });
            AddColumn("dbo.Groups", "CreatedByLecturerId", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "CreatedByLecturerID", c => c.Int(nullable: false));
            DropColumn("dbo.Groups", "CreatedBy_Id");
            DropColumn("dbo.Students", "CreatedBy_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "CreatedBy_Id", c => c.Int());
            AddColumn("dbo.Groups", "CreatedBy_Id", c => c.Int());
            DropColumn("dbo.Students", "CreatedByLecturerID");
            DropColumn("dbo.Groups", "CreatedByLecturerId");
            CreateIndex("dbo.Students", "CreatedBy_Id");
            CreateIndex("dbo.Groups", "CreatedBy_Id");
            AddForeignKey("dbo.Students", "CreatedBy_Id", "dbo.Lecturers", "Id");
            AddForeignKey("dbo.Groups", "CreatedBy_Id", "dbo.Lecturers", "Id");
        }
    }
}
