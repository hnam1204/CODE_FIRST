namespace HV_NIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_PaymentMethod : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentMethods",
                c => new
                    {
                        MethodID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Type = c.String(),
                        BankName = c.String(),
                        AccountNumber = c.String(),
                        OwnerName = c.String(),
                        MoMoPhone = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MethodID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.ShippingAddresses",
                c => new
                    {
                        AddressID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ReceiverName = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        AddressLine = c.String(nullable: false),
                        City = c.String(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AddressID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShippingAddresses", "UserID", "dbo.Users");
            DropForeignKey("dbo.PaymentMethods", "UserID", "dbo.Users");
            DropIndex("dbo.ShippingAddresses", new[] { "UserID" });
            DropIndex("dbo.PaymentMethods", new[] { "UserID" });
            DropTable("dbo.ShippingAddresses");
            DropTable("dbo.PaymentMethods");
        }
    }
}
