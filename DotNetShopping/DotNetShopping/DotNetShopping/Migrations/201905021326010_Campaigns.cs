namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Campaigns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        CampaignId = c.Short(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Code = c.String(),
                        DiscountPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductCampaign = c.Boolean(nullable: false),
                        VariantCampaign = c.Boolean(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        OneTimeUse = c.Boolean(nullable: false),
                        FreeShipping = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        UseCount = c.Long(nullable: false),
                        TotalDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalProfit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CampaignId);
            
            CreateTable(
                "dbo.PromoCodes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        CampaignId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
            CreateTable(
                "dbo.PromoCodeUsages",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        UseDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Code, t.UserId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoCodes", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.PromoCodes", new[] { "CampaignId" });
            DropTable("dbo.PromoCodeUsages");
            DropTable("dbo.PromoCodes");
            DropTable("dbo.Campaigns");
        }
    }
}
