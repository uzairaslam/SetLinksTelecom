using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.Repositories;
using Unity;
using Unity.Mvc5;

namespace SetLinksTelecom
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IDesignationRepo, DesignationRepo>();
            container.RegisterType<IPersonRepo, PersonRepo>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}