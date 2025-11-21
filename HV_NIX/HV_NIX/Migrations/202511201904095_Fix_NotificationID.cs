namespace HV_NIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Fix_NotificationID : DbMigration
    {
        public override void Up()
        {
            // Xóa primary key cũ
            DropPrimaryKey("dbo.Notifications");

            // Xóa cột identity cũ nếu còn
            DropColumn("dbo.Notifications", "NotiID");

            // Tạo cột mới identity đúng chuẩn
            AddColumn("dbo.Notifications", "NotificationID",
                c => c.Int(nullable: false, identity: true));

            // Set primary key mới
            AddPrimaryKey("dbo.Notifications", "NotificationID");
        }
        public override void Down()
        {
            DropPrimaryKey("dbo.Notifications");
            DropColumn("dbo.Notifications", "NotificationID");
            AddColumn("dbo.Notifications", "NotiID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Notifications", "NotiID");
        }
    }
}