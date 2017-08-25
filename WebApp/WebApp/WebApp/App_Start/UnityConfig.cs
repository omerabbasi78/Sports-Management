using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Microsoft.Owin.Security;
using System.Web;
using WebApp.Models;
using WebApp.Services;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Ef6;
using Repository.Pattern.DataContext;
using Repository.Pattern.Repositories;
using WebApp.Identity;

namespace WebApp.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());
            
            
            container.RegisterType<IUserStore<Users, long>, UserStoreService>(new InjectionConstructor(new ApplicationDbContext()));
            container.RegisterType<ApplicationUserManager>(new PerRequestLifetimeManager());

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IDataContextAsync, ApplicationDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IRepositoryAsync<Roles>, Repository<Roles>>();
            container.RegisterType<IRolesService, RolesService>();
            container.RegisterType<IRepositoryAsync<MenuItems>, Repository<MenuItems>>();
            container.RegisterType<IMenuItemsService, MenuItemsService>();
            container.RegisterType<IRepositoryAsync<Permissions>, Repository<Permissions>>();
            container.RegisterType<IPermissionsService, PermissionsService>();
            container.RegisterType<IRepositoryAsync<PagePermissions>, Repository<PagePermissions>>();
            container.RegisterType<IPagePermissionsService, PagePermissionsService>();
            container.RegisterType<IRepositoryAsync<RolePermissions>, Repository<RolePermissions>>();
            container.RegisterType<IRolePermissionsService, RolePermissionsService>();
        }
    }
}
