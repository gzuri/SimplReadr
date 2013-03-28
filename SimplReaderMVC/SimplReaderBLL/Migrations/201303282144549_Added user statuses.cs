namespace SimplReaderBLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addeduserstatuses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserFeedItemStatus",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        FeedItemID = c.Long(nullable: false),
                        UserID = c.Int(nullable: false),
                        DateRead = c.DateTime(),
                        DateStared = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FeedItems", t => t.FeedItemID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.FeedItemID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserFeedItemStatus", new[] { "UserID" });
            DropIndex("dbo.UserFeedItemStatus", new[] { "FeedItemID" });
            DropForeignKey("dbo.UserFeedItemStatus", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserFeedItemStatus", "FeedItemID", "dbo.FeedItems");
            DropTable("dbo.UserFeedItemStatus");
        }
    }
}
