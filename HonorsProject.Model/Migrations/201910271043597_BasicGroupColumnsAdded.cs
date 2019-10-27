namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BasicGroupColumnsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Name", c => c.String());
            AddColumn("dbo.Groups", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Groups", "CreatedBy_Id", c => c.Int());
            CreateIndex("dbo.Groups", "CreatedBy_Id");
            AddForeignKey("dbo.Groups", "CreatedBy_Id", "dbo.Lecturers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "CreatedBy_Id", "dbo.Lecturers");
            DropIndex("dbo.Groups", new[] { "CreatedBy_Id" });
            DropColumn("dbo.Groups", "CreatedBy_Id");
            DropColumn("dbo.Groups", "CreatedOn");
            DropColumn("dbo.Groups", "Name");
        }
    }
}
