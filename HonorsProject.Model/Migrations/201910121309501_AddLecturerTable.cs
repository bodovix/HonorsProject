namespace HonorsProject.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddLecturerTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lecturers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Lecturers");
        }
    }
}