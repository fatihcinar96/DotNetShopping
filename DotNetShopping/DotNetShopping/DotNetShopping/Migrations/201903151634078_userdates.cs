namespace DotNetShopping.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastLoginTime", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "RegistrationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RegistrationDate");
            DropColumn("dbo.AspNetUsers", "LastLoginTime");
        }
    }
}
