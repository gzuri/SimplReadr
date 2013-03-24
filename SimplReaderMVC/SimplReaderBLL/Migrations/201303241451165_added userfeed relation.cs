namespace SimplReaderBLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeduserfeedrelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSubscriptions",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        RssFeedID = c.Long(nullable: false),
                        SubscribeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.RssFeeds", t => t.RssFeedID)
                .Index(t => t.UserID)
                .Index(t => t.RssFeedID);
            AddColumn("dbo.RssFeeds", "LastSync", c => c.DateTime(nullable: false));
            AddColumn("dbo.FeedItems", "Author", c => c.String());
            AddColumn("dbo.FeedItems", "DatePublished", c => c.DateTime(nullable: false));
            DropColumn("dbo.FeedItems", "OriginalFeedItemCreated");
            DropTable("dbo.DBSettings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DBSettings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SettingsKey = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.FeedItems", "OriginalFeedItemCreated", c => c.DateTime(nullable: false));
            DropIndex("dbo.UserSubscriptions", new[] { "UserID" });
            DropForeignKey("dbo.UserSubscriptions", "UserID", "dbo.Users");
            DropColumn("dbo.FeedItems", "DatePublished");
            DropColumn("dbo.FeedItems", "Author");
            DropColumn("dbo.RssFeeds", "LastSync");
            DropTable("dbo.UserSubscriptions");
        }
    }
}
