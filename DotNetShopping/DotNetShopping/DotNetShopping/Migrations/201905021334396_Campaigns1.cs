namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Campaigns1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "CodeCampaign", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "CodeCampaign");
        }
    }
}
