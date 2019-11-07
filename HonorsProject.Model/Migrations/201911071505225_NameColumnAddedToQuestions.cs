namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameColumnAddedToQuestions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Name");
        }
    }
}
