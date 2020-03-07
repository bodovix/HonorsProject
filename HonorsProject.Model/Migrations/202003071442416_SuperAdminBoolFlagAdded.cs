namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuperAdminBoolFlagAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lecturers", "IsSuperAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lecturers", "IsSuperAdmin");
        }
    }
}
