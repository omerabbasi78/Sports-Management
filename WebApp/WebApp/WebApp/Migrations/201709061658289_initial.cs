namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sports", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Venues", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "City", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "City", c => c.Int(nullable: false));
            DropColumn("dbo.Venues", "IsActive");
            DropColumn("dbo.Sports", "IsActive");
            DropColumn("dbo.Events", "IsActive");
        }
    }
}
