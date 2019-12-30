namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BackToAsnwerTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "AnswerTest", c => c.String(nullable: false));
            DropColumn("dbo.Answers", "AnswerText");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answers", "AnswerText", c => c.String(nullable: false));
            DropColumn("dbo.Answers", "AnswerTest");
        }
    }
}
