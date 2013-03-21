namespace SimplReaderBLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        IsActivated = c.Boolean(nullable: false),
                        IsBlocked = c.Boolean(nullable: false),
                        BlockedReason = c.String(),
                        Password = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastLogin = c.DateTime(),
                        DefaultLangID = c.Int(),
                        Name = c.String(),
                        UserType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.UserKeys",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        UserID = c.Int(nullable: false),
                        KeyType = c.Int(nullable: false),
                        Key = c.Guid(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserKeys", new[] { "UserID" });
            DropForeignKey("dbo.UserKeys", "UserID", "dbo.Users");
            DropTable("dbo.UserKeys");
            DropTable("dbo.Users");
            DropTable("dbo.DBSettings");
        }

		 
    }
}
