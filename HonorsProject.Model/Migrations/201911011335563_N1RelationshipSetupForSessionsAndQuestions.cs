namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class N1RelationshipSetupForSessionsAndQuestions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Session_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Questions", "Session_Id");
            AddForeignKey("dbo.Questions", "Session_Id", "dbo.Sessions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Session_Id", "dbo.Sessions");
            DropIndex("dbo.Questions", new[] { "Session_Id" });
            DropColumn("dbo.Questions", "Session_Id");
        }
    }
}
