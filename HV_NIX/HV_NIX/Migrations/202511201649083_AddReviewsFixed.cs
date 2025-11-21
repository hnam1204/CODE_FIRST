namespace HV_NIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReviewsFixed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                        ReviewDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewID)
                .ForeignKey("dbo.Orders", t => t.OrderID)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.ProductID)
                .Index(t => t.UserID)
                .Index(t => t.OrderID);
            
            AddColumn("dbo.UserInfoes", "City", c => c.String());
            AddColumn("dbo.UserInfoes", "PaymentMethod", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "UserID", "dbo.Users");
            DropForeignKey("dbo.Reviews", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Reviews", "OrderID", "dbo.Orders");
            DropIndex("dbo.Reviews", new[] { "OrderID" });
            DropIndex("dbo.Reviews", new[] { "UserID" });
            DropIndex("dbo.Reviews", new[] { "ProductID" });
            DropColumn("dbo.UserInfoes", "PaymentMethod");
            DropColumn("dbo.UserInfoes", "City");
            DropTable("dbo.Reviews");
        }
    }
}
