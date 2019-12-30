namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnsertypoFixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "AnswerText", c => c.String(nullable: false));
            DropColumn("dbo.Answers", "AnswerTest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answers", "AnswerTest", c => c.String(nullable: false));
            DropColumn("dbo.Answers", "AnswerText");
        }
    }
}
