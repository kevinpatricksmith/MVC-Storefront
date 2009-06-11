using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.Tests
{
    /// <summary>
    /// Summary description for ShippingTests
    /// </summary>
    [TestClass]
    public class ShippingTests : TestBase
    {

        [TestMethod]
        public void Shipping_Method_ShouldHave_ID_Carrier_ServiceName_RatePerPound_EstimatedDelivery_DaysToDeliver()
        {
            ShippingMethod s = new ShippingMethod("Fedex", "Overnite", 10M, "Next Day",1);
            Assert.AreEqual("Fedex", s.Carrier);
            Assert.AreEqual("Overnite", s.ServiceName);
            Assert.AreEqual(10M, s.RatePerPound);
            Assert.AreEqual("Next Day", s.EstimatedDelivery);
        }

        [TestMethod]
        public void ShippingRepository_ShouldReturn_ShippingMethods()
        {
            Assert.IsNotNull(_shippingRepository.GetRates(new Order()));
        }

        [TestMethod]
        public void ShippingRepository_ShouldCalculate_Rates_Based_On_Weight_Of_Order()
        {
            Product p = _catalogService.GetProduct(1);

            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(_catalogService.GetProduct(1), 1);
            order.AddItem(_catalogService.GetProduct(2), 1);
            order.AddItem(_catalogService.GetProduct(3), 3);
            order.AddItem(_catalogService.GetProduct(4), 2);

            //each item in the test repo is 5 pounds.
            //cart's weight will be 35 pounds
            
            //the rates in the TestShippingRepo are
            //10
            //5
            //1
            //2
            var shippingMethods = _shippingRepository.GetRates(order).ToList();
            //5*10
            Assert.AreEqual(350, shippingMethods[0].Cost);
            //5*5
            Assert.AreEqual(175, shippingMethods[1].Cost);
            //5*3*1
            Assert.AreEqual(35, shippingMethods[2].Cost);
            //5*2*2
            Assert.AreEqual(70, shippingMethods[3].Cost);
        }


    }
}
