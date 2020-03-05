namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsEntityAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentText = c.String(),
                        PostedByName = c.String(),
                        PostedById = c.Int(nullable: false),
                        Name = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        Quesetion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.Quesetion_Id)
                .Index(t => t.Quesetion_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Quesetion_Id", "dbo.Questions");
            DropIndex("dbo.Comments", new[] { "Quesetion_Id" });
            DropTable("dbo.Comments");
        }
    }
}
