using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.Tests {
    /// <summary>
    /// Summary description for OrderTests
    /// </summary>
    [TestClass]
    public class OrderTests : TestBase
    {
        #region Basic Tests
        [TestMethod]
        public void Order_ShouldHave_ID_Number_Date_UserName_DiscountAmount_Items() {

            var o = new Order("1234", "rob");
            Assert.IsNotNull(o.ID);
            Assert.AreEqual("1234", o.OrderNumber);
            Assert.AreEqual("rob", o.UserName);
            Assert.IsNotNull(o.Items);

        }

        [TestMethod]
        public void Order_TestRepository_Should_Return_100_Orders() {
            Assert.AreEqual(100, _orderRepository.GetOrders().Count());
        }

        [TestMethod]
        public void Order_TestRepository_Should_Return_495_OrderItems() {
            Assert.AreEqual(495, _orderRepository.GetOrderItems().Count());
        }

        [TestMethod]
        public void Order_Service_Should_Return_100_Orders() {
            Assert.AreEqual(100, _orderService.GetOrders().Count());

        }


        [TestMethod]
        public void Order_Should_Have_UserName_CreateDate_AndItemList() {
            Order order = new Order("testuser");
            Assert.AreEqual("testuser", order.UserName);
        }

        [TestMethod]
        public void Order_Should_Have_OrderStatus_SetTo_NotCheckedOut_OnCreation() {
            Order order = new Order("testuser");
            Assert.AreEqual(OrderStatus.NotCheckoutOut, order.Status);
        }

        [TestMethod]
        public void Order_Should_Have_Billing_And_Shipping_Addresses_SetTo_Null_OnCreation() {
            Order order = new Order("testuser");
            Assert.IsNull(order.ShippingAddress);
        }


        [TestMethod]
        public void Order_Should_Have_SubTotal()
        {
            Order order = _orderService.GetCurrentOrder("testuser");

            //each product is $10
            order.AddItem(_catalogService.GetProduct(1), 1);
            order.AddItem(_catalogService.GetProduct(2), 1);
            order.AddItem(_catalogService.GetProduct(5), 3);

            Assert.AreEqual(50, order.SubTotal);

        }

        [TestMethod]
        public void Order_Should_Total_Correctly_With_Tax_And_Shipping_Set()
        {
            Order order = _orderService.GetCurrentOrder("testuser");

            //each product is $10
            order.AddItem(_catalogService.GetProduct(1), 1);
            order.AddItem(_catalogService.GetProduct(2), 1);
            order.AddItem(_catalogService.GetProduct(3), 1);
            Assert.AreEqual(30, order.SubTotal);
            
            //set the shipping address to HI, US
            //the test repo has this at 5% 
            order.ShippingAddress = new Address("testuser", "first", "last", "email",
                "street1", "street2", "city", "HI", "94510", "US");
            
            IShippingRepository rep = new TestShippingRepository();
            ISalesTaxService tax = new RegionalSalesTaxService(new TestTaxRepository());

            //each item in testrepo is 5 pounds
            var methods = rep.GetRates(order).ToList();

            //the first method is 10/pound
            //15 pounds at 10/pound == $150
            order.ShippingMethod = methods[0];
            Assert.AreEqual(150, order.ShippingMethod.Cost);

            //tax is 5% of taxable goods
            //30 * .05 is 1.5
            order.TaxAmount = tax.CalculateTaxAmount(order);
            Assert.AreEqual(1.5M, order.TaxAmount);

            //the total should be 30 + 150 + 1.5 = 181.5
            Assert.AreEqual(181.5M, order.Total);

        }

        [TestMethod]
        public void Order_With_No_Items_Should_Have_Zero_TaxAmount()
        {
            Order o=new Order();
            Assert.AreEqual(0,o.TaxAmount);
        }


        [TestMethod]
        public void OrderService_ShouldReturn_20_Orders_AsPagedList_From_GetOrdersPaged_With_100_Total_And_5_PageCount() {
            PagedList<Order> orders = _orderService.GetPagedOrders(20, 0);
            Assert.AreEqual(20, orders.Count);
            Assert.AreEqual(100, orders.TotalCount);
            Assert.AreEqual(5, orders.TotalPages);
        }

        [TestMethod]
        public void OrderService_ShouldReturn_20_Orders_AsPagedList_From_GetOrdersPaged_With_100_Total_And_5_PageCount_FilteredByStatus()
        {
            PagedList<Order> orders = _orderService.GetPagedOrders(20, 0,OrderStatus.NotCheckoutOut);
            Assert.AreEqual(20, orders.Count);
            Assert.AreEqual(100, orders.TotalCount);
            Assert.AreEqual(5, orders.TotalPages);
        }

        #endregion

        #region Order Items

        [TestMethod]
        public void OrderItem_Should_Have_OrderID_Product_Quantity_DiscountAmount() {
            Product p = _catalogService.GetProduct(1);
            OrderItem item=new OrderItem(Guid.Empty,p);
            Assert.AreEqual(Guid.Empty,item.OrderID);
            Assert.AreEqual(p, item.Product);
            Assert.AreEqual(1, item.Quantity);
        }

        [TestMethod]
        public void OrderItem_Should_Have_LineItem_Summary_EqualTo_50_For_5_TestProduct() {
            Product p = _catalogService.GetProduct(1);
            OrderItem item = new OrderItem(Guid.Empty, p);
            item.Quantity = 5; 
            Assert.AreEqual(50,item.LineTotal);
        }
        
        [TestMethod]
        public void OrderItem_Should_Have_LineItem_Summary_EqualTo_25_For_5_TestProduct_WithDiscounts_Of_5_Each() {
            Product p = _catalogService.GetProduct(1);
            p.DiscountPercent = .50M;
            OrderItem item = new OrderItem(Guid.Empty, p, 5);
            Assert.AreEqual(25, item.LineTotal);
        }

        [TestMethod]
        public void OrderItem_Should_Be_Equal_With_Same_Product_And_OrderID() {
            Guid orderID = Guid.NewGuid();
            Product p = _catalogService.GetProduct(1);
            OrderItem item = new OrderItem(orderID, p, 5);
            OrderItem item2 = new OrderItem(orderID, p, 5);

            Assert.AreEqual(item,item2);
            Assert.IsTrue(item.Equals(item2));
        }
        [TestMethod]
        public void OrderItem_Should_Return_ProductName_ForToString() {
            Product p = _catalogService.GetProduct(1);
            OrderItem item = new OrderItem(Guid.NewGuid(), p, 5);

            Assert.AreEqual("Product1",item.ToString());

        }


        #endregion

        #region Cart Tests

        [TestMethod]
        public void OrderItem_Should_Have_Product_And_Quantity() {
            Product p = _catalogService.GetProduct(1);
            OrderItem item = new OrderItem(p, 5);
            Assert.AreEqual(1, item.Product.ID);
            Assert.AreEqual(5, item.Quantity);
        }

        [TestMethod]
        public void OrderItem_Should_Have_Category_SubCategory_From_Where_ProductWas_Selected() {
            Product p = _catalogService.GetProduct(1);
            OrderItem item = new OrderItem(p, 5);

        }

        [TestMethod]
        public void OrderItem_With_ProductID_1_ShouldEqual_Item2_With_ProductID_1_AndDifferent_Quantity() {
            Product p = _catalogService.GetProduct(1);
            
            OrderItem item1 = new OrderItem(p, 5);
            OrderItem item2 = new OrderItem(p, 2);

            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        public void OrderItem_With_ProductID_1_Should_Return_Product1_ForToString() {
            Product p = _catalogService.GetProduct(1);
            OrderItem item1 = new OrderItem(p, 5);

            Assert.AreEqual("Product1", item1.ToString());
        }

        [TestMethod]
        public void OrderItem_With_ProductID_1_Should_Be_Returned_When_Using_Contains() {
            Product p = _catalogService.GetProduct(1);

            Order order = new Order("testuser");

            OrderItem item1 = new OrderItem(p, 5);
            OrderItem item2 = new OrderItem(p, 2);

            order.Items.Add(item1);

            Assert.IsTrue(order.Items.Contains(item2));
        }

        [TestMethod]
        public void OrderRepository_Should_Return_orders() {
            IOrderRepository rep = new TestOrderRepository();
            IList<Order> orders = rep.GetOrders().ToList();
            Assert.IsNotNull(orders);
        }
        [TestMethod]
        public void OrderRepository_Should_Return_orderItems() {
            IOrderRepository rep = new TestOrderRepository();
            IList<OrderItem> orders = rep.GetOrderItems().ToList();
            Assert.IsNotNull(orders);
        }


        [TestMethod]
        public void OrderRepository_Should_Accept_Product_1() {

            Product p = _catalogService.GetProduct(1);
            IOrderRepository rep = new TestOrderRepository();

            Order order = new Order("testuser");
            OrderItem item1 = new OrderItem(p, 1);

            order.Items.Add(item1);

            Assert.AreEqual(1, order.Items.Count);

        }

        [TestMethod]
        public void Order_Can_Find_Product1_In_TestUserorder() {
            Product p = _catalogService.GetProduct(1);
            Order order = new Order("testuser");
            order.Items.Add(new OrderItem(p, 1));

            OrderItem item = order.FindItem(p);

            Assert.IsNotNull(item);

        }

        [TestMethod]
        public void Order_Should_Add_Product_ToTestorder() {
            Product p = _catalogService.GetProduct(1);
            Order order = new Order("testuser");
            order.Items.Add(new OrderItem(p, 1));

            Assert.AreEqual(1, order.Items.Count);
        }

        [TestMethod]
        public void Order_Should_UpdateQuantity_Of_Product1_If_Added_Twice() {
            Product p = _catalogService.GetProduct(1);
           
            Order order = new Order("testuser");
            order.AddItem(p, 1);
            order.AddItem(p, 1);

            _orderService.SaveItems(order);

            Assert.AreEqual(1, order.Items.Count);
            Assert.AreEqual(2, order.Items[0].Quantity);
        }

        [TestMethod]
        public void Order_Should_Update_Product_InTestorder_ToQuantity100() {
            Product p = _catalogService.GetProduct(1);
           
            Order order = new Order("testuser");

            order.AddItem(p, 1);
            order.AdjustQuantity(p, 100);
            _orderService.SaveItems(order);

            Assert.IsNotNull(order);
        }



        [TestMethod]
        public void Order_Should_Remove_Product1_From_TestUser_order() {
            Product p = _catalogService.GetProduct(1);
           
            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(p, 1);
            Assert.AreEqual(1, order.Items.Count);
            order.RemoveItem(p);

            Assert.AreEqual(0, order.Items.Count);

        }

        [TestMethod]
        public void Order_Should_Remove_Item_When_Updated_To_0() {

            Product p = _catalogService.GetProduct(1);

           
            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(p, 1);
            order.AddItem(p, 1);
            order.AdjustQuantity(p, 0);
            _orderService.SaveItems(order);

            Assert.AreEqual(0, order.Items.Count);

        }


        [TestMethod]
        public void Order_Should_Do_Nothing_When_Zero_Quantity_Passed_ToAdd() {

            Product p = _catalogService.GetProduct(1);

           
            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(p, 1);
            order.AddItem(p, 1);
            order.AddItem(p, 0);
            _orderService.SaveItems(order);

            Assert.AreEqual(1, order.Items.Count);
            Assert.AreEqual(2, order.Items[0].Quantity);

        }


        [TestMethod]
        public void Order_Should_Decrement_When_Negative_Quantity_Passed() {

            Product p = _catalogService.GetProduct(1);

           
            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(p, 1);
            order.AddItem(p, 1);
            order.AddItem(p, -1);
            _orderService.SaveItems(order);

            Assert.AreEqual(1, order.Items.Count);
            Assert.AreEqual(1, order.Items[0].Quantity);

        }

        [TestMethod]
        public void Order_Can_Clear_TestUser_order() {
            Product p = _catalogService.GetProduct(1);
           
            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(p, 1);
            order.AddItem(p, 1);
            order.AddItem(p, 1);

            order.ClearItems();
            _orderService.SaveItems(order);

            Assert.AreEqual(0, order.Items.Count);

        }
        [TestMethod]
        public void Order_Can_Find_Item_By_ProductID() {
            Product p = _catalogService.GetProduct(1);
           
            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(p, 1);
            OrderItem item = order.FindItem(p.ID);

            Assert.AreEqual(item.Product, p);


        }
        [TestMethod]
        public void Order_Can_Remove_Item_By_ProductID() {
            Product p = _catalogService.GetProduct(1);
           
            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(p, 1);
            order.RemoveItem(p.ID);

            Assert.AreEqual(0, order.Items.Count);


        }


        [TestMethod]
        public void Order_Should_Reprice_Cart_When_Product_Price_IsChanged()
        {
            Product p = _catalogService.GetProduct(1);

            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(p, 1);
            p.Price = p.Price + 2.0M;

            Assert.IsTrue(order.RepriceItems(), "RepriceItems returned false"); 
        }

        [TestMethod]
        public void Order_SubTotal_Should_Reflect_Repriced_Items()
        {
            Product p = _catalogService.GetProduct(1);

            Order order = _orderService.GetCurrentOrder("testuser");
            
            order.AddItem(p, 1);
            decimal startingSubTotal = order.SubTotal;
            decimal expectedSubTotal = startingSubTotal + 2.0M;
            p.Price = p.Price + 2.0M;
            
            order.RepriceItems();
            Assert.AreEqual(order.SubTotal, expectedSubTotal, "New subtotal was not repriced correctly"); 

        }

        [TestMethod]
        public void Order_LineItem_LineItemPrice_Should_Reprice_Correctly()
        {
            Product p = _catalogService.GetProduct(1);

            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(p, 1);
            Decimal originalLineTotal = order.FindItem(1).LineItemPrice;
            Decimal expectedLineTotal = originalLineTotal + 2.0M; 
            p.Price = p.Price + 2.0M;
            order.RepriceItems();
            
            Assert.AreEqual(order.FindItem(1).LineItemPrice, expectedLineTotal, "New LineItemPrice was not repriced correctly"); 

        }

        [TestMethod]
        public void Order_Total_Should_Reflect_Repriced_Items()
        {
            Product p = _catalogService.GetProduct(1);

            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(p, 1);
            decimal startingTotal = order.Total;
            decimal expectedTotal = startingTotal + 2.0M;
            p.Price = p.Price + 2.0M;
            order.RepriceItems();
            Assert.AreEqual(order.Total, expectedTotal, 
                "New total was not repriced correctly"); 
        }

    #endregion

        #region Fulfillment Tests
        [TestMethod]
        public void Order_Should_Return_True_If_All_Products_Have_Shipped_DeliveryMethod()
        {

            Product p = _catalogService.GetProduct(1);

            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(_catalogService.GetProduct(1), 1);
            order.AddItem(_catalogService.GetProduct(2), 1);
            order.AddItem(_catalogService.GetProduct(3), 1);

            Assert.IsTrue(order.HasShippableGoods);

        }

        [TestMethod]
        public void Order_Should_Return_False_If_All_Products_Have_Download_DeliveryMethod()
        {

            Product p = _catalogService.GetProduct(1);

            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(_catalogService.GetProduct(4), 1);
            order.AddItem(_catalogService.GetProduct(5), 1);

            Assert.IsFalse(order.HasShippableGoods);

        }


        [TestMethod]
        public void Order_Should_Return_True_If_MoreThanZero_Products_Have_Shipped_DeliveryMethod()
        {

            Product p = _catalogService.GetProduct(1);

            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(_catalogService.GetProduct(1), 1);
            order.AddItem(_catalogService.GetProduct(4), 1);

            Assert.IsTrue(order.HasShippableGoods);

        }

        [TestMethod]
        public void Order_Should_Calculate_ItemWeight()
        {

            Product p = _catalogService.GetProduct(1);

            Order order = _orderService.GetCurrentOrder("testuser");

            order.AddItem(_catalogService.GetProduct(1), 1);
            order.AddItem(_catalogService.GetProduct(2), 1);
            order.AddItem(_catalogService.GetProduct(3), 3);
            order.AddItem(_catalogService.GetProduct(4), 2);

            //each item in the test repo is 5 pounds.

            Assert.AreEqual(35, order.TotalWeightInPounds);

        }
        #endregion

        #region CreditCard Tests
        [TestMethod]
        public void CreditCard_Should_Have_Name_Account_Expiration_Number()
        {
            CreditCard cc = new CreditCard("Visa","testuser", "41111111111111", 10, 2010, "123");
            Assert.AreEqual(cc.Name, "testuser");
            Assert.AreEqual(cc.AccountNumber, "41111111111111");
            Assert.AreEqual(cc.ExpirationYear, 2010);
            Assert.AreEqual(cc.ExpirationMonth, 10);
            Assert.AreEqual(cc.Expiration, new DateTime(2010,10,31));

        }
        [TestMethod]
        public void CreditCard_TestNumber_ShouldValidate()
        {
            //card number generated by Brad Conte's card generator
            //http://bradconte.com/projects/cc_generator.php
            CreditCard cc = new CreditCard("Visa", "testuser", "4586 9748 7358 4049", 10, 2010, "123");
            Assert.IsTrue(cc.IsValid());

        }
        [TestMethod]
        public void CreditCard_TestNumber_ShouldBe_Invalid()
        {
            CreditCard cc = new CreditCard("Visa", "testuser", "4111 1111 1111 1111", 10, 2010, "123");
            Assert.IsFalse(cc.IsValid());

        }
        [TestMethod]
        public void CreditCard_Should_Mask_CardNumber()
        {
            CreditCard cc = new CreditCard("Visa", "testuser", "4111 1111 1111 1111", 10, 2010, "123");
            Assert.AreEqual("**** **** **** 1111", cc.MaskedNumber);

        }
        #endregion

        #region Transaction Tests
        [TestMethod]
        public void Transaction_Should_Have_ID_Amount_OrderID_Authorization_Date_Notes_Processor()
        {
            
            Guid orderID=Guid.NewGuid();
            Guid ID=Guid.NewGuid();

            Transaction p = new Transaction(ID, orderID, 100, DateTime.Now, "1234", "notes", TransactionProcessor.FakePaymentProcessor);
            Assert.AreEqual(ID, p.ID);
            Assert.AreEqual(orderID, p.OrderID);
            Assert.AreEqual(100, p.Amount);
            Assert.AreEqual("1234", p.AuthorizationCode);
            Assert.AreEqual("notes", p.Notes);
            Assert.AreEqual(TransactionProcessor.FakePaymentProcessor, p.Processor);

        }
        [TestMethod]
        public void OrderRepository_CanReturn_Transactions()
        {
            TestOrderRepository rep = new TestOrderRepository();
            Assert.IsNotNull(rep.GetTransactions());

        }

        [TestMethod]
        public void Transaction_ShouldBe_Refund_When_Negative()
        {
            Transaction t = new Transaction(Guid.NewGuid(), -100, "124", TransactionProcessor.FakePaymentProcessor);
            Assert.IsTrue(t.IsRefund);

        }


        #endregion

        #region Processing Tests


        [TestMethod]
        public void OrderProcessing_Should_Fail_Without_ShipAddress()
        {
            bool hasFailed = false;
            Order o = GetTestOrder();
            o.ShippingAddress=null;
            try
            {
                o.ValidateForCheckout();
            }
            catch
            {
                hasFailed = true;
            }


            Assert.IsTrue(hasFailed);

        }

        [TestMethod]
        public void OrderProcessing_Should_Fail_Without_ShipMethod()
        {
            bool hasFailed = false;
            Order o = GetTestOrder();

            o.ShippingMethod = null;
            try
            {
                o.ValidateForCheckout();
            }
            catch
            {
                hasFailed = true;
            }


            Assert.IsTrue(hasFailed);

        }

        [TestMethod]
        public void OrderProcessing_Should_Fail_Without_PaymentMethod()
        {
            bool hasFailed = false;
            Order o = GetTestOrder();
            o.PaymentMethod = null;

            try
            {
                o.ValidateForCheckout();
            }
            catch
            {
                hasFailed = true;
            }
            Assert.IsTrue(hasFailed);

        }

        [TestMethod]
        public void OrderProcessing_Should_Succeed_With_Valid_Addresses()
        {
            bool hasFailed = false;
            Order o = GetTestOrder();

            _orderService = new OrderService(_orderRepository,_catalogRepository,_shippingRepository,_shippingService);
            _orderService.SaveOrder(o);

        }

        #endregion

        #region Stats
        [TestMethod]
        public void OrderService_Should_Return_4_Products_For_Recommended_Product1_WihoutID_1()
        {
            IList<Product> result = _orderService.GetRecommended(1);
            
            Assert.AreEqual(4, result.Count);
            Assert.IsNull(result.Where(x => x.ID==1).SingleOrDefault());

        }
        [TestMethod]
        public void OrderService_Should_Return_4_Products_For_Recommended_Sorted_By_Sum()
        {
            IList<Product> result = _orderService.GetRecommended(1);

            Assert.AreEqual(5, result[0].ID);
            Assert.AreEqual(4, result[1].ID);

        }

        #endregion
    }
}
