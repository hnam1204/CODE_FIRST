namespace HV_NIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Status", c => c.String());
            DropColumn("dbo.OrderDetails", "Qty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "Qty", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "Status");
            DropColumn("dbo.OrderDetails", "Quantity");
        }
    }
}
