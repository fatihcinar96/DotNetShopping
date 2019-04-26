namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        PageId = c.String(nullable: false, maxLength: 128),
                        PageTitle = c.String(),
                        PageBody = c.String(),
                        Keywords = c.String(),
                        Description = c.String(),
                        CreateUser = c.String(),
                        CreateDate = c.String(),
                        UpdateUser = c.String(),
                        UpdateDate = c.String(),
                    })
                .PrimaryKey(t => t.PageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pages");
        }
    }
}
