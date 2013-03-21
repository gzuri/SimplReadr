namespace SimplReaderBLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedfeedtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RssFeeds",
                c => new
                    {
                        RssFeedID = c.Long(nullable: false, identity: true),
                        FullURL = c.String(),
                    })
                .PrimaryKey(t => t.RssFeedID);
            
            CreateTable(
                "dbo.FeedItems",
                c => new
                    {
                        FeedItemID = c.Long(nullable: false, identity: true),
                        FullURL = c.String(),
                        Title = c.String(),
                        ShortDescription = c.String(),
                        OriginalFeedItemCreated = c.DateTime(nullable: false),
                        DateCollected = c.DateTime(nullable: false),
                        RssFeedID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.FeedItemID)
                .ForeignKey("dbo.RssFeeds", t => t.RssFeedID, cascadeDelete: true)
                .Index(t => t.RssFeedID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.FeedItems", new[] { "RssFeedID" });
            DropForeignKey("dbo.FeedItems", "RssFeedID", "dbo.RssFeeds");
            DropTable("dbo.FeedItems");
            DropTable("dbo.RssFeeds");
        }
    }
}
