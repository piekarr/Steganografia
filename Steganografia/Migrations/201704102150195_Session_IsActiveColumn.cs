namespace Steganografia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Session_IsActiveColumn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SesssionId = c.Guid(nullable: false),
                        UserId = c.Int(nullable: false),
                        Expires = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GetUTCDate()"),
                        CreatedByUserId = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedByUserId = c.Int(),
                        IsActive = c.Boolean(nullable: false, defaultValue: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedByUserId)
                .ForeignKey("dbo.User", t => t.UpdatedByUserId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.SesssionId);

            AddColumn("dbo.User", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
        }

        public override void Down()
        {
            DropForeignKey("dbo.Session", "UserId", "dbo.User");
            DropForeignKey("dbo.Session", "UpdatedByUserId", "dbo.User");
            DropForeignKey("dbo.Session", "CreatedByUserId", "dbo.User");
            DropIndex("dbo.Session", new[] { "SesssionId" });
            DropColumn("dbo.User", "IsActive");
            DropTable("dbo.Session");
        }
    }
}
