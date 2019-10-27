namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class N1RelationshipSetupForSessionsAndGroupsCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sessions", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Sessions", new[] { "Group_Id" });
            AlterColumn("dbo.Sessions", "Group_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Sessions", "Group_Id");
            AddForeignKey("dbo.Sessions", "Group_Id", "dbo.Groups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sessions", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Sessions", new[] { "Group_Id" });
            AlterColumn("dbo.Sessions", "Group_Id", c => c.Int());
            CreateIndex("dbo.Sessions", "Group_Id");
            AddForeignKey("dbo.Sessions", "Group_Id", "dbo.Groups", "Id");
        }
    }
}
