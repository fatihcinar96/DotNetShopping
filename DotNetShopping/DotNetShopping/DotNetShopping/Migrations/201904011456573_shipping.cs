namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shipping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentMethods",
                c => new
                    {
                        PaymentMethodId = c.Short(nullable: false, identity: true),
                        Name = c.String(),
                        Domestic = c.Boolean(nullable: false),
                        International = c.Boolean(nullable: false),
                        StaticCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentInfo = c.String(),
                        PaymentDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PaymentMethodId);
            
            CreateTable(
                "dbo.ShippingCosts",
                c => new
                    {
                        ShippingMethodId = c.Short(nullable: false),
                        CountryId = c.Short(nullable: false),
                        CostHalf = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostOne = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostOneHalf = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostTwo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostTwoHalf = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.ShippingMethodId, t.CountryId });
            
            CreateTable(
                "dbo.ShippingMethods",
                c => new
                    {
                        ShippingMethodId = c.Short(nullable: false, identity: true),
                        Name = c.String(),
                        Domestic = c.Boolean(nullable: false),
                        International = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ShippingMethodId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShippingMethods");
            DropTable("dbo.ShippingCosts");
            DropTable("dbo.PaymentMethods");
        }
    }
}
