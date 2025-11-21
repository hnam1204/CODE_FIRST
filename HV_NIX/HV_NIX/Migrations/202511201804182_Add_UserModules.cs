namespace HV_NIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_UserModules : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        LogID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Action = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LogID);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotiID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Message = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NotiID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notifications");
            DropTable("dbo.ActivityLogs");
        }
    }
}
