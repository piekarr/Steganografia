namespace Steganografia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ConversationAndMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conversation",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 250),
                    CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GetUTCDate()"),
                    CreatedByUserId = c.Int(nullable: false),
                    UpdatedDate = c.DateTime(),
                    UpdatedByUserId = c.Int(),
                    IsActive = c.Boolean(nullable: false, defaultValue: true),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedByUserId)
                .ForeignKey("dbo.User", t => t.UpdatedByUserId);

            CreateTable(
                "dbo.Message",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Content = c.String(nullable: false, maxLength: 4000),
                    ConversationId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GetUTCDate()"),
                    CreatedByUserId = c.Int(nullable: false),
                    UpdatedDate = c.DateTime(),
                    UpdatedByUserId = c.Int(),
                    IsActive = c.Boolean(nullable: false, defaultValue: true),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conversation", t => t.ConversationId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.CreatedByUserId)
                .ForeignKey("dbo.User", t => t.UpdatedByUserId)
                .Index(t => t.ConversationId);

            CreateTable(
                "dbo.UserUnreadMessage",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    MessageId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GetUTCDate()"),
                    CreatedByUserId = c.Int(nullable: false),
                    UpdatedDate = c.DateTime(),
                    UpdatedByUserId = c.Int(),
                    IsActive = c.Boolean(nullable: false, defaultValue: true),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Message", t => t.MessageId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.CreatedByUserId)
                .ForeignKey("dbo.User", t => t.UpdatedByUserId)
                .Index(t => t.UserId)
                .Index(t => t.MessageId);

            CreateTable(
                "dbo.ConversationUser",
                c => new
                {
                    Conversation_Id = c.Int(nullable: false),
                    User_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Conversation_Id, t.User_Id })
                .ForeignKey("dbo.Conversation", t => t.Conversation_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Conversation_Id)
                .Index(t => t.User_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.ConversationUser", "User_Id", "dbo.User");
            DropForeignKey("dbo.ConversationUser", "Conversation_Id", "dbo.Conversation");
            DropForeignKey("dbo.UserUnreadMessage", "UserId", "dbo.User");
            DropForeignKey("dbo.UserUnreadMessage", "MessageId", "dbo.Message");
            DropForeignKey("dbo.Message", "ConversationId", "dbo.Conversation");
            DropForeignKey("dbo.Conversation", "CreatedByUserId", "dbo.User");
            DropForeignKey("dbo.Conversation", "UpdatedByUserId", "dbo.User");
            DropForeignKey("dbo.Message", "CreatedByUserId", "dbo.User");
            DropForeignKey("dbo.Message", "UpdatedByUserId", "dbo.User");
            DropForeignKey("dbo.UserUnreadMessage", "CreatedByUserId", "dbo.User");
            DropForeignKey("dbo.UserUnreadMessage", "UpdatedByUserId", "dbo.User");
            DropIndex("dbo.ConversationUser", new[] { "User_Id" });
            DropIndex("dbo.ConversationUser", new[] { "Conversation_Id" });
            DropIndex("dbo.UserUnreadMessage", new[] { "MessageId" });
            DropIndex("dbo.UserUnreadMessage", new[] { "UserId" });
            DropIndex("dbo.Message", new[] { "ConversationId" });
            DropTable("dbo.ConversationUser");
            DropTable("dbo.UserUnreadMessage");
            DropTable("dbo.Message");
            DropTable("dbo.Conversation");
        }
    }
}
