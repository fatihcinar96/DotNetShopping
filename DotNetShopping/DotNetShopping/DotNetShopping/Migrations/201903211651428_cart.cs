namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        VariantId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.VariantId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Carts");
        }
    }
}
