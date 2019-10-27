namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialColumnsSetForQuestionTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "TimeAsked", c => c.DateTime(nullable: false));
            AddColumn("dbo.Questions", "QuestionText", c => c.String(nullable: false));
            AddColumn("dbo.Questions", "IsResolved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Questions", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Questions", "AskedBy_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Questions", "AskedBy_Id");
            AddForeignKey("dbo.Questions", "AskedBy_Id", "dbo.Students", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "AskedBy_Id", "dbo.Students");
            DropIndex("dbo.Questions", new[] { "AskedBy_Id" });
            DropColumn("dbo.Questions", "AskedBy_Id");
            DropColumn("dbo.Questions", "CreatedOn");
            DropColumn("dbo.Questions", "IsResolved");
            DropColumn("dbo.Questions", "QuestionText");
            DropColumn("dbo.Questions", "TimeAsked");
        }
    }
}
