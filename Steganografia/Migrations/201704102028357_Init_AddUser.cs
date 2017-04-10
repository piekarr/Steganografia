namespace Steganografia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Init_AddUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserName = c.String(nullable: false, maxLength: 50),
                    Password = c.String(nullable: false, maxLength: 50),
                    FirstName = c.String(nullable: false, maxLength: 100),
                    LastName = c.String(nullable: false, maxLength: 100),
                    CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GetUTCDate()"),
                    CreatedByUserId = c.Int(nullable: false, defaultValue: 1000),
                    UpdatedDate = c.DateTime(),
                    UpdatedByUserId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedByUserId)
                .ForeignKey("dbo.User", t => t.UpdatedByUserId)
                .Index(t => t.UserName, unique: true);
            Sql("DBCC CHECKIDENT ('User', RESEED, 2000)");
        }

        public override void Down()
        {
            DropForeignKey("dbo.User", "UpdatedByUserId", "dbo.User");
            DropForeignKey("dbo.User", "CreatedByUserId", "dbo.User");
            DropIndex("dbo.User", new[] { "UserName" });
            DropTable("dbo.User");
        }
    }
}
