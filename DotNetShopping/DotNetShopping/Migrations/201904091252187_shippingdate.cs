namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shippingdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "ShippingDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "ShippingDate", c => c.DateTime(nullable: false));
        }
    }
}
