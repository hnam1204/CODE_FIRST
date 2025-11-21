namespace HV_NIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ActivityLog_Fixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Title");
        }
    }
}
