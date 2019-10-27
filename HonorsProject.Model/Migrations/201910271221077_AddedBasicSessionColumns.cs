namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBasicSessionColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "Name", c => c.String());
            AddColumn("dbo.Sessions", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "CreatedByLecturerID", c => c.Int(nullable: false));
            AddColumn("dbo.Sessions", "Group_Id", c => c.Int());
            CreateIndex("dbo.Sessions", "Group_Id");
            AddForeignKey("dbo.Sessions", "Group_Id", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sessions", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Sessions", new[] { "Group_Id" });
            DropColumn("dbo.Sessions", "Group_Id");
            DropColumn("dbo.Sessions", "CreatedByLecturerID");
            DropColumn("dbo.Sessions", "CreatedOn");
            DropColumn("dbo.Sessions", "EndTime");
            DropColumn("dbo.Sessions", "StartTime");
            DropColumn("dbo.Sessions", "Name");
        }
    }
}
