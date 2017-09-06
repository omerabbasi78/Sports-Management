namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        EventName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        SportId = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                        VenueId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Sports", t => t.SportId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Venues", t => t.VenueId, cascadeDelete: true)
                .Index(t => t.SportId)
                .Index(t => t.UserId)
                .Index(t => t.VenueId);
            
            CreateTable(
                "dbo.Sports",
                c => new
                    {
                        SportId = c.Int(nullable: false, identity: true),
                        SportName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SportId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        UserName = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Address = c.String(),
                        IsAccountVerified = c.Boolean(nullable: false),
                        TempPassword = c.String(),
                        IsPasswordResetRequested = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        ProfilePic = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        TeamName = c.String(),
                        NoOfMembers = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateTable(
                "dbo.Venues",
                c => new
                    {
                        VenueId = c.Int(nullable: false, identity: true),
                        VenueName = c.String(),
                        IsReserved = c.Boolean(nullable: false),
                        Address = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VenueId);
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        MenuItemId = c.Int(nullable: false, identity: true),
                        MenuItemName = c.String(),
                        IsParent = c.Boolean(nullable: false),
                        GroupId = c.Int(),
                        IconClass = c.String(),
                        SortOrder = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MenuItemId)
                .ForeignKey("dbo.MenuItems", t => t.GroupId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.PagePermissions",
                c => new
                    {
                        PagePermissionId = c.Int(nullable: false, identity: true),
                        PermissionId = c.Int(nullable: false),
                        PermissionText = c.String(),
                        ControllerName = c.String(),
                        ActionName = c.String(),
                        PageUrl = c.String(),
                        MenuItemId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PagePermissionId)
                .ForeignKey("dbo.MenuItems", t => t.MenuItemId, cascadeDelete: true)
                .Index(t => t.MenuItemId);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        RolePermissionId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        PagePermissionId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RolePermissionId)
                .ForeignKey("dbo.PagePermissions", t => t.PagePermissionId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PagePermissionId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        PermissionId = c.Int(nullable: false, identity: true),
                        PermissionName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PermissionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RolePermissions", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "PagePermissionId", "dbo.PagePermissions");
            DropForeignKey("dbo.PagePermissions", "MenuItemId", "dbo.MenuItems");
            DropForeignKey("dbo.MenuItems", "GroupId", "dbo.MenuItems");
            DropForeignKey("dbo.Events", "VenueId", "dbo.Venues");
            DropForeignKey("dbo.Events", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Events", "SportId", "dbo.Sports");
            DropIndex("dbo.RolePermissions", new[] { "PagePermissionId" });
            DropIndex("dbo.RolePermissions", new[] { "RoleId" });
            DropIndex("dbo.PagePermissions", new[] { "MenuItemId" });
            DropIndex("dbo.MenuItems", new[] { "GroupId" });
            DropIndex("dbo.Users", new[] { "TeamId" });
            DropIndex("dbo.Events", new[] { "VenueId" });
            DropIndex("dbo.Events", new[] { "UserId" });
            DropIndex("dbo.Events", new[] { "SportId" });
            DropTable("dbo.Permissions");
            DropTable("dbo.Roles");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.PagePermissions");
            DropTable("dbo.MenuItems");
            DropTable("dbo.Venues");
            DropTable("dbo.Teams");
            DropTable("dbo.Users");
            DropTable("dbo.Sports");
            DropTable("dbo.Events");
        }
    }
}
