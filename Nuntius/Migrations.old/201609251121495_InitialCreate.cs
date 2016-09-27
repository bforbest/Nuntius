namespace Nuntius.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleId = c.Int(nullable: false, identity: true),
                        Author = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        UrlToImage = c.String(),
                        PublishedAt = c.String(nullable: false),
                        SourceId = c.String(nullable: false, maxLength: 128),
                        Favourite_FavouriteId = c.Int(),
                    })
                .PrimaryKey(t => t.ArticleId)
                .ForeignKey("dbo.Favourites", t => t.Favourite_FavouriteId)
                .ForeignKey("dbo.Sources", t => t.SourceId, cascadeDelete: true)
                .Index(t => t.SourceId)
                .Index(t => t.Favourite_FavouriteId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        CommentText = c.String(),
                        DatePublished = c.DateTime(nullable: false),
                        Id = c.String(maxLength: 128),
                        ArticleId = c.Int(nullable: false),
                        Comment_CommentId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.Comment_CommentId)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ArticleId)
                .Index(t => t.Comment_CommentId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProfilePicture = c.String(),
                        FavouriteId = c.Int(nullable: false),
                        SubscriptionId = c.Int(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Favourites", t => t.FavouriteId, cascadeDelete: true)
                .ForeignKey("dbo.Subscriptions", t => t.SubscriptionId)
                .Index(t => t.FavouriteId)
                .Index(t => t.SubscriptionId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Favourites",
                c => new
                    {
                        FavouriteId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.FavouriteId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        SubscriptionId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.SubscriptionId);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SourceId = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Url = c.String(),
                        Category = c.String(),
                        Language = c.String(),
                        Country = c.String(),
                        UrlsToLogos_Small = c.String(),
                        UrlsToLogos_Medium = c.String(),
                        UrlsToLogos_Large = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VotingArticles",
                c => new
                    {
                        VotingArticleId = c.Int(nullable: false, identity: true),
                        VoteValue = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.VotingArticleId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SourceSubscriptions",
                c => new
                    {
                        Source_Id = c.String(nullable: false, maxLength: 128),
                        Subscription_SubscriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Source_Id, t.Subscription_SubscriptionId })
                .ForeignKey("dbo.Sources", t => t.Source_Id, cascadeDelete: true)
                .ForeignKey("dbo.Subscriptions", t => t.Subscription_SubscriptionId, cascadeDelete: true)
                .Index(t => t.Source_Id)
                .Index(t => t.Subscription_SubscriptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.VotingArticles", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.VotingArticles", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Articles", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.Comments", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "SubscriptionId", "dbo.Subscriptions");
            DropForeignKey("dbo.SourceSubscriptions", "Subscription_SubscriptionId", "dbo.Subscriptions");
            DropForeignKey("dbo.SourceSubscriptions", "Source_Id", "dbo.Sources");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "FavouriteId", "dbo.Favourites");
            DropForeignKey("dbo.Articles", "Favourite_FavouriteId", "dbo.Favourites");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "Comment_CommentId", "dbo.Comments");
            DropForeignKey("dbo.Comments", "ArticleId", "dbo.Articles");
            DropIndex("dbo.SourceSubscriptions", new[] { "Subscription_SubscriptionId" });
            DropIndex("dbo.SourceSubscriptions", new[] { "Source_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.VotingArticles", new[] { "ApplicationUserID" });
            DropIndex("dbo.VotingArticles", new[] { "ArticleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "SubscriptionId" });
            DropIndex("dbo.AspNetUsers", new[] { "FavouriteId" });
            DropIndex("dbo.Comments", new[] { "Comment_CommentId" });
            DropIndex("dbo.Comments", new[] { "ArticleId" });
            DropIndex("dbo.Comments", new[] { "Id" });
            DropIndex("dbo.Articles", new[] { "Favourite_FavouriteId" });
            DropIndex("dbo.Articles", new[] { "SourceId" });
            DropTable("dbo.SourceSubscriptions");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.VotingArticles");
            DropTable("dbo.Sources");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Favourites");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
            DropTable("dbo.Articles");
        }
    }
}
