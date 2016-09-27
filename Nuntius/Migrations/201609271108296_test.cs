namespace Nuntius.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Comments", name: "Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.Comments", name: "IX_Id", newName: "IX_ApplicationUserId");
            CreateTable(
                "dbo.VotingComments",
                c => new
                    {
                        VotingCommentId = c.Int(nullable: false, identity: true),
                        CommentId = c.Int(nullable: false),
                        VoteValue = c.Boolean(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.VotingCommentId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.CommentId)
                .Index(t => t.ApplicationUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VotingComments", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.VotingComments", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.VotingComments", new[] { "ApplicationUserID" });
            DropIndex("dbo.VotingComments", new[] { "CommentId" });
            DropTable("dbo.VotingComments");
            RenameIndex(table: "dbo.Comments", name: "IX_ApplicationUserId", newName: "IX_Id");
            RenameColumn(table: "dbo.Comments", name: "ApplicationUserId", newName: "Id");
        }
    }
}
