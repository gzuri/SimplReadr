namespace SimplReaderBLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedHtmlUrltofeed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssFeeds", "HtmlURL", c => c.String());
        }
        
        public override void Down()
        {
            DropIndex("dbo.Categories", new[] { "UserID" });
            DropForeignKey("dbo.Categories", "UserID", "dbo.Users");
            AlterColumn("dbo.Categories", "UserID", c => c.Long(nullable: false));
            DropColumn("dbo.RssFeeds", "HtmlURL");
        }
    }
}
