namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conversation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ConversationId", c => c.String());
            AddColumn("dbo.Orders", "PaymentError", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PaymentError");
            DropColumn("dbo.Orders", "ConversationId");
        }
    }
}
