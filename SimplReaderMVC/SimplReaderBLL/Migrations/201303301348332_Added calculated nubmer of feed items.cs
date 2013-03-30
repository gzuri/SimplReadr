namespace SimplReaderBLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedcalculatednubmeroffeeditems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserFeedItemStatus", "UserID", "dbo.Users");
            DropIndex("dbo.UserFeedItemStatus", new[] { "UserID" });
            DropPrimaryKey("dbo.UserSubscriptions", new[] { "ID" });
            DropColumn("dbo.UserSubscriptions", "ID");
            AddColumn("dbo.UserSubscriptions", "UserSubscriptionID", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.RssFeeds", "CalculatedFeedItemsCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserFeedItemStatus", "UserSubscriptionID", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.UserSubscriptions", "UserSubscriptionID");
            AddForeignKey("dbo.UserFeedItemStatus", "UserSubscriptionID", "dbo.UserSubscriptions", "UserSubscriptionID", cascadeDelete: true);
            CreateIndex("dbo.UserFeedItemStatus", "UserSubscriptionID");
            DropColumn("dbo.UserFeedItemStatus", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserFeedItemStatus", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.UserSubscriptions", "ID", c => c.Long(nullable: false, identity: true));
            DropIndex("dbo.UserFeedItemStatus", new[] { "UserSubscriptionID" });
            DropForeignKey("dbo.UserFeedItemStatus", "UserSubscriptionID", "dbo.UserSubscriptions");
            DropPrimaryKey("dbo.UserSubscriptions", new[] { "UserSubscriptionID" });
            AddPrimaryKey("dbo.UserSubscriptions", "ID");
            DropColumn("dbo.UserFeedItemStatus", "UserSubscriptionID");
            DropColumn("dbo.RssFeeds", "CalculatedFeedItemsCount");
            DropColumn("dbo.UserSubscriptions", "UserSubscriptionID");
            CreateIndex("dbo.UserFeedItemStatus", "UserID");
            AddForeignKey("dbo.UserFeedItemStatus", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
    }
}
