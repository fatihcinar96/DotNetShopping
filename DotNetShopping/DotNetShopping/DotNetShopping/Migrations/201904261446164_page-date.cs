namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pagedate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pages", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Pages", "UpdateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pages", "UpdateDate", c => c.String());
            AlterColumn("dbo.Pages", "CreateDate", c => c.String());
        }
    }
}
