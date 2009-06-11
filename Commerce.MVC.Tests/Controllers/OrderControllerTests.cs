using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Data;
using Commerce.MVC.Web.Controllers;
using System.Web.Mvc;
using Commerce.Services;

namespace Commerce.Tests.Controllers {
    /// <summary>
    /// Summary description for CartControllerTests
    /// </summary>
    [TestClass]
    public class OrderControllerTests : TestBase
    {


        [TestMethod]
        public void OrderController_AddItem_Should_Add_Item_ToCart_ForTestUser()
        {

            Order order = _orderService.GetCurrentOrder("testuser");
            Assert.AreEqual(0,order.Items.Count);

            OrderController controller = new OrderController(_orderService, _catalogService);
            controller.SetFakeControllerContext();
            ActionResult result = controller.AddItem(1);

            //should have an item in our cart
            order = _orderService.GetCurrentOrder("testuser");
            Assert.AreEqual(1, order.Items.Count);

        }


        [TestMethod]
        public void OrderController_AddItem_ShouldRedirect() {

            OrderController controller = new OrderController(_orderService, _catalogService);
            ActionResult result = controller.AddItem(1);

            //we should get a redirect here
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
        }



        [TestMethod]
        public void OrderController_Show_Should_Return_NonNull_TestCart() {

            OrderController controller = new OrderController(_orderService, _catalogService);
            ViewResult result = (ViewResult)controller.Show();
            Order order = (Order)result.ViewData.Model;

            ////we should get a redirect here
            Assert.AreEqual("Show", result.ViewName);
            Assert.IsNotNull(order);
        }


        [TestMethod]
        public void OrderController_Checkout_Should_Return_ViewResult_When_First_Called()
        {

            OrderController controller = new OrderController(_orderService, _catalogService);
            controller.SetFakeControllerContextWithLogin("testuser", "password", "");
            //add two items to the cart
            controller.AddItem(1);
            controller.AddItem(2);

            ActionResult result = (ActionResult)controller.Checkout();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            
            
        }


        Order GetTestOrder() {
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


        [TestMethod]
        public void OrderController_List_Should_Set_20_Orders_To_ViewData() {
            OrderController controller = new OrderController(_orderService, _catalogService);
            ActionResult result = controller.List(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            ViewResult view = (ViewResult)result;
            IList<Order> orders = (IList<Order>)view.ViewData.Model;

            Assert.AreEqual(20, orders.Count);


        }


    }
}
