namespace HV_NIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToProducts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Description", c => c.String());
            DropColumn("dbo.Products", "CreatedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "CreatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Products", "Description");
        }
    }
}
