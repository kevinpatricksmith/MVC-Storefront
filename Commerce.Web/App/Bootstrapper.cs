using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Configuration;
using Commerce.Services;
using Commerce.Data;
using StructureMap.Attributes;
using Commerce.Data.SqlRepository;
using Commerce.Pipelines;
using StructureMap.Pipeline;

namespace Commerce.MVC.Web {
    public static class Bootstrapper {

        public static void ConfigureStructureMap() {
            StructureMapConfiguration.AddRegistry(new DBServiceRegistry());
            StructureMapConfiguration.AddRegistry(new StorefrontRegistry());
        }
    }

    public class StorefrontRegistry : Registry {
        protected override void configure() {
            
            ForRequestedType<IOrderService>()
                .TheDefaultIsConcreteType<OrderService>();
            
            ForRequestedType<ICatalogService>()
                .TheDefaultIsConcreteType<CatalogService>();

            ForRequestedType<IOrderRepository>()
                .TheDefaultIsConcreteType<SqlOrderRepository>();

            ForRequestedType<ICatalogRepository>()
                .TheDefaultIsConcreteType<SqlCatalogRepository>();

            ForRequestedType<IPersonalizationRepository>()
                .TheDefaultIsConcreteType<SqlPersonalizationRepository>();

            ForRequestedType<IPersonalizationService>()
               .TheDefaultIsConcreteType<PersonalizationService>();

            ForRequestedType<ITaxRepository>()
               .TheDefaultIsConcreteType<SqlTaxRepository>();

            ForRequestedType<ISalesTaxService>()
               .TheDefaultIsConcreteType<RegionalSalesTaxService>();

            ForRequestedType<IShippingService>()
               .TheDefaultIsConcreteType<SimpleShippingService>();

            ForRequestedType<IShippingRepository>()
               .TheDefaultIsConcreteType<SqlShippingRepository>();

            ForRequestedType<IPipelineEngine>()
             .TheDefaultIsConcreteType<WindowsWorkflowPipeline>();
            
            ForRequestedType<IInventoryRepository>()
              .TheDefaultIsConcreteType<SqlInventoryRepository>();
            
            ForRequestedType<IInventoryService>()
             .TheDefaultIsConcreteType<InventoryService>();

            ForRequestedType<IMailerRepository>()
             .TheDefaultIsConcreteType<SqlMailerRepository>();

            ForRequestedType<ILogger>()
              .TheDefaultIsConcreteType<NLogLogger>();

            ForRequestedType<IIncentiveService>()
             .TheDefaultIsConcreteType<IncentiveService>();

            ForRequestedType<IIncentiveRepository>()
             .TheDefaultIsConcreteType<SqlIncentiveRepository>();

            //CHANGE THIS
            ForRequestedType<IAddressValidationService>()
               .TheDefaultIsConcreteType<NullAddressValidationService>();

            //CHANGE THIS
            ForRequestedType<IPaymentService>()
               .TheDefaultIsConcreteType<FakePaymentService>();
            
            //CHANGE THIS
            ForRequestedType<IMailerService>()
              .TheDefaultIsConcreteType<NullMailerService>();
           
        }
    }
}
