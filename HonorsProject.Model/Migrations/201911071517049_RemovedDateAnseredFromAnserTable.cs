namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedDateAnseredFromAnserTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Answers", "DateAnswered");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answers", "DateAnswered", c => c.DateTime(nullable: false));
        }
    }
}
