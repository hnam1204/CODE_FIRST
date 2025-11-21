namespace HV_NIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentMethods", "MethodType", c => c.String());
            AddColumn("dbo.PaymentMethods", "Provider", c => c.String());
            DropColumn("dbo.PaymentMethods", "Type");
            DropColumn("dbo.PaymentMethods", "BankName");
            DropColumn("dbo.PaymentMethods", "OwnerName");
            DropColumn("dbo.PaymentMethods", "MoMoPhone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentMethods", "MoMoPhone", c => c.String());
            AddColumn("dbo.PaymentMethods", "OwnerName", c => c.String());
            AddColumn("dbo.PaymentMethods", "BankName", c => c.String());
            AddColumn("dbo.PaymentMethods", "Type", c => c.String());
            DropColumn("dbo.PaymentMethods", "Provider");
            DropColumn("dbo.PaymentMethods", "MethodType");
        }
    }
}
