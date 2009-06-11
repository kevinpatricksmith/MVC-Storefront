using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Data;
using Commerce.Services;
using Commerce.Pipelines;

namespace Commerce.Tests
{
    /// <summary>
    /// Summary description for TestBase
    /// </summary>
    [TestClass]
    public class TestBase
    {
        protected IOrderRepository _orderRepository;
        protected ICatalogRepository _catalogRepository;
        protected IAddressValidationService _addressValidation;
        protected IShippingRepository _shippingRepository;
        protected IShippingService _shippingService;
        protected ISalesTaxService _taxService;
        protected ITaxRepository _taxRepository;
        protected IOrderService _orderService;
        protected ICatalogService _catalogService;
        protected IPersonalizationRepository _personalizationRepository;
        protected IPersonalizationService _personalizationService;
        protected IPaymentService _paymentService;
        protected IPipelineEngine _pipeline;
        protected IInventoryRepository _inventoryRepository;
        protected IInventoryService _inventoryService;
        protected IMailerRepository _mailerRepository;
        protected IMailerService _mailerService;
        protected IIncentiveRepository _incentiveRepository;
        protected IIncentiveService _incentiveService;

        [TestInitialize]
        public void Startup()
        {
            _orderRepository = new TestOrderRepository();
            _catalogRepository = new TestCatalogRepository();
            _addressValidation = new TestAddressValidator();
            _shippingRepository = new TestShippingRepository();
            _shippingService = new SimpleShippingService(_shippingRepository);
            _taxRepository = new TestTaxRepository();
            _taxService = new RegionalSalesTaxService(_taxRepository);
            _orderService = new OrderService(_orderRepository,_catalogRepository,_shippingRepository,_shippingService);
            _personalizationRepository = new TestPersonalizationRepository();
            _personalizationService = new PersonalizationService(_personalizationRepository,_orderRepository, _orderService,_catalogRepository);
            _catalogService = new CatalogService(_catalogRepository,_orderService);
            _paymentService = new FakePaymentService();
            _incentiveRepository = new TestIncentiveRepository();
            _incentiveService = new IncentiveService(_incentiveRepository);

            //this service throws the sent mailers into a collection
            //and does not use SMTP
            _mailerService = new TestMailerService();
            _inventoryRepository = new TestInventoryRepository();
            _inventoryService = new InventoryService(_inventoryRepository,_catalogService);
            _mailerRepository = new TestMailerRepository();
            _pipeline=new DefaultPipeline(
                _addressValidation,_paymentService,
                _orderService,_mailerService,
                _inventoryService
                );


        }

        protected Order GetTestOrder() {
            Order o = _orderService.GetCurrentOrder("testuser");
            o.AddItem(_catalogService.GetProduct(1), 1);
            o.AddItem(_catalogService.GetProduct(2), 1);
            o.AddItem(_catalogService.GetProduct(3), 1);
            o.AddItem(_catalogService.GetProduct(4), 1);

            o.ShippingMethod = _shippingService.CalculateRates(o)[0];
            o.PaymentMethod = new CreditCard("Visa", "testuser", "4586 9748 7358 4049", 10, 2010, "123");

            //add some addresses
            Address add = new Address("Joe", "Joe", "Tonks", "joe@joe.com",
                "1099 Alakea St", "", "Honolulu", "HI", "96813", "US");

            o.ShippingAddress = add;
            o.BillingAddress = add;


            return o;
        }

    }
}
