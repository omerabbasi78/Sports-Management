namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserName = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        City = c.Int(nullable: false),
                        Phone = c.String(nullable: false),
                        Address = c.String(),
                        IsAccountVerified = c.Boolean(nullable: false),
                        ShopId = c.Long(),
                        TempPassword = c.String(),
                        IsPasswordResetRequested = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsTrustedUser = c.Boolean(nullable: false),
                        InvalidAttempts = c.Int(nullable: false),
                        ProfilePic = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
        }
    }
}
