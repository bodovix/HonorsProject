namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialColumnsSetForAnswerTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "AnswerTest", c => c.String(nullable: false));
            AddColumn("dbo.Answers", "DateAnswered", c => c.DateTime(nullable: false));
            AddColumn("dbo.Answers", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Answers", "AnsweredBy_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Answers", "Question_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Answers", "AnsweredBy_Id");
            CreateIndex("dbo.Answers", "Question_Id");
            AddForeignKey("dbo.Answers", "AnsweredBy_Id", "dbo.Lecturers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Answers", "Question_Id", "dbo.Questions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Answers", "AnsweredBy_Id", "dbo.Lecturers");
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.Answers", new[] { "AnsweredBy_Id" });
            DropColumn("dbo.Answers", "Question_Id");
            DropColumn("dbo.Answers", "AnsweredBy_Id");
            DropColumn("dbo.Answers", "CreatedOn");
            DropColumn("dbo.Answers", "DateAnswered");
            DropColumn("dbo.Answers", "AnswerTest");
        }
    }
}
