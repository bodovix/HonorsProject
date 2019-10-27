namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBasicStudentColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Name", c => c.String());
            AddColumn("dbo.Students", "Email", c => c.String());
            AddColumn("dbo.Students", "Password", c => c.String());
            AddColumn("dbo.Students", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Students", "CreatedBy_Id", c => c.Int());
            CreateIndex("dbo.Students", "CreatedBy_Id");
            AddForeignKey("dbo.Students", "CreatedBy_Id", "dbo.Lecturers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "CreatedBy_Id", "dbo.Lecturers");
            DropIndex("dbo.Students", new[] { "CreatedBy_Id" });
            DropColumn("dbo.Students", "CreatedBy_Id");
            DropColumn("dbo.Students", "CreatedOn");
            DropColumn("dbo.Students", "Password");
            DropColumn("dbo.Students", "Email");
            DropColumn("dbo.Students", "Name");
        }
    }
}
