namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Campaigns2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "RequiredAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "RequiredAmount");
        }
    }
}
