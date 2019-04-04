namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredfields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "BillingFirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "BillingLastName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "BillingLastName", c => c.String());
            AlterColumn("dbo.Orders", "BillingFirstName", c => c.String());
        }
    }
}
