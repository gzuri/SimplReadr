namespace SimplReaderBLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            AddColumn("dbo.UserSubscriptions", "CategoryID", c => c.Long(nullable:true));
            AddForeignKey("dbo.UserSubscriptions", "CategoryID", "dbo.Categories", "CategoryID");
            CreateIndex("dbo.UserSubscriptions", "CategoryID");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Categories", new[] { "User_UserID" });
            DropIndex("dbo.UserSubscriptions", new[] { "CategoryID" });
            DropForeignKey("dbo.Categories", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.UserSubscriptions", "CategoryID", "dbo.Categories");
            DropColumn("dbo.UserSubscriptions", "CategoryID");
            DropTable("dbo.Categories");
        }
    }
}
