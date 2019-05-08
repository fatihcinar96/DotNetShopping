namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productCampaign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CampaignId", c => c.Short(nullable: false));
            AddColumn("dbo.Variants", "CampaignId", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Variants", "CampaignId");
            DropColumn("dbo.Products", "CampaignId");
        }
    }
}
