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
    /// Summary description for TaxTests
    /// </summary>
    [TestClass]
    public class TaxTests:TestBase
    {


        [TestMethod]
        public void TaxRate_Should_Have_ID_Rate_Region_Country()
        {
            TaxRate t = new TaxRate(0.05M, "HI", "US","");
            Assert.AreEqual(0.05M, t.Rate);
            Assert.AreEqual("HI", t.Region);
            Assert.AreEqual("US", t.Country);

        }
        [TestMethod]
        public void TaxRepository_Should_Return_Rates()
        {
            Assert.IsNotNull(_taxRepository.GetTaxRates());
        }


        [TestMethod]
        public void Order_Should_Sum_Taxable_Goods_With_Quantity_Of_One()
        {
            Order order = _orderService.GetCurrentOrder("testuser");
            
            //items 1 through 3 are all taxable
            order.AddItem(_catalogService.GetProduct(1), 1);
            order.AddItem(_catalogService.GetProduct(2), 1);
            order.AddItem(_catalogService.GetProduct(3), 1);

            Assert.AreEqual(30, order.TaxableGoodsSubtotal);

        }

        [TestMethod]
        public void Order_Should_Sum_Taxable_Goods_With_Multiple_Quanities()
        {
            Order order = _orderService.GetCurrentOrder("testuser");

            //items 1 through 3 are all taxable
            order.AddItem(_catalogService.GetProduct(1), 1);
            order.AddItem(_catalogService.GetProduct(2), 1);
            order.AddItem(_catalogService.GetProduct(3), 3);

            Assert.AreEqual(50, order.TaxableGoodsSubtotal);

        }

        [TestMethod]
        public void Order_Should_Sum_Taxable_Goods_When_Mixed_With_Non_Taxable()
        {
            Order order = _orderService.GetCurrentOrder("testuser");

            //items 1 through 3 are all taxable
            order.AddItem(_catalogService.GetProduct(1), 1);
            order.AddItem(_catalogService.GetProduct(2), 1);
            order.AddItem(_catalogService.GetProduct(5), 3);

            Assert.AreEqual(20, order.TaxableGoodsSubtotal);

        }
    }
}
