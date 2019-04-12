namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredorder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "BillingEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "BillingStreet1", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "BillingZip", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "BillingTelephone", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "ShippingFirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "ShippingLastName", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "ShippingStreet1", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "ShippingZip", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "ShippingTelephone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "ShippingTelephone", c => c.String());
            AlterColumn("dbo.Orders", "ShippingZip", c => c.String());
            AlterColumn("dbo.Orders", "ShippingStreet1", c => c.String());
            AlterColumn("dbo.Orders", "ShippingLastName", c => c.String());
            AlterColumn("dbo.Orders", "ShippingFirstName", c => c.String());
            AlterColumn("dbo.Orders", "BillingTelephone", c => c.String());
            AlterColumn("dbo.Orders", "BillingZip", c => c.String());
            AlterColumn("dbo.Orders", "BillingStreet1", c => c.String());
            AlterColumn("dbo.Orders", "BillingEmail", c => c.String());
            AlterColumn("dbo.Orders", "UserId", c => c.String());
        }
    }
}
