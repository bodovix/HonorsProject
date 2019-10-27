namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addedStudentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                {
                    Id = c.Int(nullable: false, identity: false),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Students");
        }
    }
}