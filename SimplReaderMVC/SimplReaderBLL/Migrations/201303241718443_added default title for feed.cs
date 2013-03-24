namespace SimplReaderBLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeddefaulttitleforfeed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssFeeds", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RssFeeds", "Title");
        }
    }
}
