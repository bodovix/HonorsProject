namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WasHelpfullColumnAddedToAnswerTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "WasHelpfull", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "WasHelpfull");
        }
    }
}
