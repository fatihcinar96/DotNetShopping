namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sequence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductImages", "Sequence", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductImages", "Sequence");
        }
    }
}
