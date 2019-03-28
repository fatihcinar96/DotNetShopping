namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderProducts",
                c => new
                    {
                        OrderId = c.Long(nullable: false),
                        VariantId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.OrderId, t.VariantId });
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Long(nullable: false, identity: true),
                        UserId = c.String(),
                        BillingFirstName = c.String(),
                        BillingLastName = c.String(),
                        BillingCompany = c.String(),
                        BillingEmail = c.String(),
                        BillingStreet1 = c.String(),
                        BillingStreet2 = c.String(),
                        BillingCityId = c.Short(nullable: false),
                        BillingStateId = c.Short(nullable: false),
                        BillingCountryId = c.Short(nullable: false),
                        BillingZip = c.String(),
                        BillingTelephone = c.String(),
                        ShippingFirstName = c.String(),
                        ShippingLastName = c.String(),
                        ShippingCompany = c.String(),
                        ShippingStreet1 = c.String(),
                        ShippingStreet2 = c.String(),
                        ShippingCityId = c.Short(nullable: false),
                        ShippingStateId = c.Short(nullable: false),
                        ShippingCountryId = c.Short(nullable: false),
                        ShippingZip = c.String(),
                        ShippingTelephone = c.String(),
                        ShippingMethodId = c.Short(nullable: false),
                        ShippingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ShippingDate = c.DateTime(nullable: false),
                        PaymentMethodId = c.Short(nullable: false),
                        CardHolderName = c.String(),
                        CardAccount = c.String(),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalProfit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderDate = c.DateTime(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        Paid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
            DropTable("dbo.OrderProducts");
        }
    }
}
