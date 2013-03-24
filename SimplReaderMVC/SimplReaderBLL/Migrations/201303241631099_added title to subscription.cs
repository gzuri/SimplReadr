namespace SimplReaderBLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedtitletosubscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSubscriptions", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSubscriptions", "Title");
        }
    }
}
