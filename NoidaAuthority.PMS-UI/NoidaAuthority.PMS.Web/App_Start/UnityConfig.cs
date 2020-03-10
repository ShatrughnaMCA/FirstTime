using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using NoidaAuthority.PMS.Common.Logger;
using NoidaAuthority.PMS.Service.Admin;
using NoidaAuthority.PMS.Service.Login;
using NoidaAuthority.PMS.Service.ManageUsers;
using Unity.Mvc5;
using NoidaAuthority.PMS.Service;

namespace NoidaAuthority.PMS.Web
{
    public static class UnityConfig
    {
        public static HttpConfiguration RegisterComponents(HttpConfiguration config)

        {
			var container = new UnityContainer();
            container.RegisterType<ILogin, LoginService>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityResolver(container);

            return config;
        }
    }
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<ILogin, LoginService>();

            container.RegisterType<IMenuMappingService, MenuMappingService>();
            container.RegisterType<IManageUserService, ManageUserService>();
            container.RegisterType<IManageRoleService, ManageRoleService>();

            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<ICommonService, CommonService>();
            container.RegisterType<IRequestService, RequestService>();
            container.RegisterType<IPropertyService, PropertyService>();

            container.RegisterType<ILogService>(new InjectionFactory(c => LoggingManager.GetLogInstance()));

            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}