namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageLocatonColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "ImageLocation", c => c.String());
            AddColumn("dbo.Questions", "ImageLocation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "ImageLocation");
            DropColumn("dbo.Answers", "ImageLocation");
        }
    }
}
