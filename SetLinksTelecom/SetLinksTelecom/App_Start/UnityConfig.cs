using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
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
            container.RegisterType<IItemsRepo, ItemsRepo>();
            container.RegisterType<IPurchaseRepo, PurchaseRepo>();
            container.RegisterType<ISaleRepo, SaleRepo>();
            container.RegisterType<ILineRepo, LineRepo>();
            container.RegisterType<IInventoryType, InventoryTypeRepo>();
            container.RegisterType<IProductCategoryRepo, ProductCategoryRepo>();
            container.RegisterType<IAccAccount, AccAccountRepo>();
            container.RegisterType<IReports, ReportsRepo>();
            container.RegisterType<IAccVoucher, AccVoucherRepo>();
            container.RegisterType<IBvsRepo, BvsRepo>();
            container.RegisterType<IUserRepo, UserRepo>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}