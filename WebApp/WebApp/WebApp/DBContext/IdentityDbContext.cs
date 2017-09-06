using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
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

    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        //Update-Database -configuration WebApp.Migrations.Configuration -Verbose -force
        public IdentityDbContext()
            : base("DefaultConnection")
        {
        }

        public static IdentityDbContext Create()
        {
            return new IdentityDbContext();
        }

        public DbSet<Users> User { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Sports> Sports { get; set; }
        public DbSet<Venues> Venues { get; set; }
        public DbSet<Team> Team { get; set; }
    }
}