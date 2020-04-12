namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessionBlacklistAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "Blacklist", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "Blacklist");
        }
    }
}
