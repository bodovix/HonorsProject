namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsLectureOnlyQuesitonColumnAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "IsLectureOnlyQuestion", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "IsLectureOnlyQuestion");
        }
    }
}
