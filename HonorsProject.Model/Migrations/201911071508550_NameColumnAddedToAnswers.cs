namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameColumnAddedToAnswers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "Name");
        }
    }
}
