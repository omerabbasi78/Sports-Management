using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApp.Models;

namespace WebApp
{
    //Update-Database -configuration WebApp.Migrations.Configuration -Verbose -force
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : Repository.Pattern.Ef6.DataContext
    {
        //Update-Database -configuration WebApp.Migrations.Configuration -Verbose -force
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Users> User { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<MenuItems> MenuItems { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<PagePermissions> PagePermissions { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
    }
}