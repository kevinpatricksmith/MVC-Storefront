using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Pipelines;
using Commerce.Services;
using Commerce.Data;
using System.Workflow.Runtime;
using System.Threading;
using System.Diagnostics;

namespace Commerce.Tests {
    /// <summary>
    /// Summary description for PipelineTests
    /// </summary>
    [TestClass]
    public class PipelineTests:TestBase {

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        Dictionary<string, object> GetParams(Order order) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PaymentServiceInterface", _paymentService);
            parameters.Add("OrderServiceInterface", _orderService);
            parameters.Add("MailerServiceInterface", _mailerService);
            parameters.Add("AddressValidationInterface", _addressValidation);
            parameters.Add("InventoryServiceInterface", _inventoryService);
            parameters.Add("CustomerOrder", order);
            return parameters;
        }
        public Dictionary<string, object> GetShippingParams(Order order, string trackingNumber, DateTime estimatedDelivery)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("OrderServiceInterface", _orderService);
            parameters.Add("MailerServiceInterface", _mailerService);
            parameters.Add("CustomerOrder", order);
            parameters.Add("TrackingNumber", trackingNumber);
            parameters.Add("EstimatedDelivery", estimatedDelivery);

            return parameters;

        }
        Dictionary<string, object> GetCancelParams(Order order)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PaymentServiceInterface", _paymentService);
            parameters.Add("OrderServiceInterface", _orderService);
            parameters.Add("MailerServiceInterface", _mailerService);
            parameters.Add("InventoryServiceInterface", _inventoryService);
            parameters.Add("CustomerOrder", order);
            return parameters;
        }

        #region Order Verification with CC - WWF Integration

        [TestMethod]
        public void Pipeline_CC_Workflow_Should_Send_Acknowledgment_Email_To_Customer() {
            
            Order order = GetTestOrder();

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime()) {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e) {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.SubmitOrderWorkflow), GetParams(order));

                instance.Start();

                waitHandle.WaitOne();

                //execution should be finished
                //check the mailers
                TestMailerService mailer = _mailerService as TestMailerService;
                Assert.IsTrue(mailer.SentMail.Count > 0);

                //the first email should be to the customer
                Assert.AreEqual(MailerType.CustomerOrderReceived, mailer.SentMail[0].MailType);

            }
        }


        [TestMethod]
        public void Pipeline_CC_Workflow_Should_Fail_And_Notify_Customer_When_Invalid_Address() {

            Order order = GetTestOrder();

            //this tells the test address service to throw
            order.ShippingAddress.City = "Fail Town";

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime()) {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e) {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.SubmitOrderWorkflow), GetParams(order));

                try {
                    instance.Start();
                    waitHandle.WaitOne();
                } catch {

                }

                //execution should be finished
                //check the mailers
                TestMailerService mailer = _mailerService as TestMailerService;
                Assert.IsTrue(mailer.SentMail.Count > 0);

                //the first email should be to the customer
                Assert.AreEqual(MailerType.CustomerOrderReceived, mailer.SentMail[0].MailType);
                Assert.AreEqual(MailerType.CustomerAddressValidationFailed, mailer.SentMail[1].MailType);

            }
        }

        [TestMethod]
        public void Pipeline_CC_Workflow_Should_Fail_And_Notify_Customer_When_CC_AuthFails() {

            Order order = GetTestOrder();

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime()) {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e) {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                var parameters = GetParams(order);

                //reset the payment service to throw
                parameters["PaymentServiceInterface"] = new ThrowPaymentService();

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.SubmitOrderWorkflow), parameters);

                try {
                    instance.Start();
                    waitHandle.WaitOne();
                } catch {

                }

                //execution should be finished
                //check the mailers
                TestMailerService mailer = _mailerService as TestMailerService;
                Assert.IsTrue(mailer.SentMail.Count > 0);

                //the first email should be to the customer
                Assert.AreEqual(MailerType.CustomerOrderReceived, mailer.SentMail[0].MailType);
                Assert.AreEqual(MailerType.CustomerPaymentAuthFailed, mailer.SentMail[1].MailType);

            }
        }


        [TestMethod]
        public void Pipeline_CC_Workflow_Should_Succeed_With_Valid_Order_And_Card_And_Address_And_Send_User_Thankyou_Admin_Notification() {

            Order order = GetTestOrder();

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime()) {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e) {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                var parameters = GetParams(order);

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.SubmitOrderWorkflow), parameters);

                instance.Start();
                waitHandle.WaitOne();
                //check the Repository and see if the status for the order has been updated
                Order checkOrder = _orderService.GetOrder(order.ID);
                Assert.AreEqual(OrderStatus.Verified, checkOrder.Status);

                //execution should be finished
                //check the mailers
                TestMailerService mailer = _mailerService as TestMailerService;
                Assert.IsTrue(mailer.SentMail.Count > 0);

                //the first email should be to the customer
                Assert.AreEqual(MailerType.CustomerOrderReceived, mailer.SentMail[0].MailType);
                
                //if success, the admin should be second
                Assert.AreEqual(MailerType.AdminOrderReceived, mailer.SentMail[1].MailType);

            }
        }

        [TestMethod]
        public void Pipeline_CC_Workflow_Should_Succeed_With_Valid_Order_And_Change_Product1_Inventory_Status_ToBackOrder() {

            Order order = GetTestOrder();

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime()) {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e) {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                var parameters = GetParams(order);

                //remove the products from the order
                order.Items.Clear();

                //add in a single Product 1
                //there is only one of these in the inventory
                order.AddItem(_catalogService.GetProduct(1), 1);
                

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.SubmitOrderWorkflow), parameters);

                instance.Start();
                waitHandle.WaitOne();
                
                //pull the product out
                Product p = _catalogService.GetProduct(1);

                //this product allows back order
                //since there was only one in inventory, the status should now be set
                //to BackOrder
                Assert.AreEqual(InventoryStatus.BackOrder, p.Inventory);

             }
        }

        [TestMethod]
        public void Pipeline_CC_Workflow_Should_Succeed_With_Valid_Order_And_Add_Inventory_Record_For_Product1() {

            Order order = GetTestOrder();

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime()) {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e) {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                var parameters = GetParams(order);

                //remove the products from the order
                order.Items.Clear();

                //add in a single Product 1
                //there is only one of these in the inventory
                order.AddItem(_catalogService.GetProduct(1), 1);


                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.SubmitOrderWorkflow), parameters);

                instance.Start();
                waitHandle.WaitOne();

                //pull the inventory records
                IList<InventoryRecord> records = _inventoryService.GetInventory(1);
                Assert.AreEqual(2, records.Count);

                //there should be 2 records here
                //the second should be set to -1
                Assert.AreEqual(-1, records[1].Increment);


            }
        }

        #endregion

        #region Charge Process - Default Pipe

        [TestMethod]
        public void Pipeline_Charge_Should_Create_Transaction_With_Amount_Equal_To_OrderTotal()
        {
            Order order = GetTestOrder();
            _pipeline.ChargeOrder(order);

            Assert.AreEqual(1, order.Transactions.Count);
            Assert.AreEqual(order.Transactions[0].Amount, order.Total);
        }

        [TestMethod]
        public void Pipeline_Charge_Should_Set_Order_Status_To_Charged()
        {
            Order order = GetTestOrder();
            _pipeline.ChargeOrder(order);

            Assert.AreEqual(OrderStatus.Charged, order.Status);
        }

        [TestMethod]
        public void Pipeline_Charge_Should_Save_Order_With_Transaction_And_Charged_Status()
        {
            Order order = GetTestOrder();
            _pipeline.ChargeOrder(order);

            order = _orderService.GetOrder(order.ID);

            Assert.AreEqual(OrderStatus.Charged, order.Status);
            Assert.AreEqual(1, order.Transactions.Count);
            Assert.AreEqual(order.Transactions[0].Amount, order.Total);
        }
        #endregion

        #region Charge Process - WWF Integration
        [TestMethod]
        public void Pipeline_WWF_Charge_Should_Create_Transaction_With_Amount_Equal_To_OrderTotal()
        {
            Order order = GetTestOrder();

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.ChargeWorkflow), GetParams(order));

                instance.Start();

                waitHandle.WaitOne();

                //execution should be finished
                //check the order for a transaction

                Assert.AreEqual(1, order.Transactions.Count);
                Assert.AreEqual(order.Transactions[0].Amount, order.Total);

            }
        }

        [TestMethod]
        public void Pipeline_WWF_Charge_Should_Status_To_Charged()
        {
            Order order = GetTestOrder();

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.ChargeWorkflow), GetParams(order));

                instance.Start();

                waitHandle.WaitOne();

                //execution should be finished
                //check the order for status

                Assert.AreEqual(OrderStatus.Charged, order.Status);

            }
        }

        [TestMethod]
        public void Pipeline_WWF_Charge_Should_Status_To_Charged_Create_Transaction_AndSave()
        {
            Order order = GetTestOrder();

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.ChargeWorkflow), GetParams(order));

                instance.Start();

                waitHandle.WaitOne();

                //execution should be finished
                //check the order for status

                order = _orderService.GetOrder(order.ID);

                Assert.AreEqual(OrderStatus.Charged, order.Status);
                Assert.AreEqual(1, order.Transactions.Count);
                Assert.AreEqual(order.Transactions[0].Amount, order.Total);

            }
        }
        #endregion

        #region Shipping Process - Default
        [TestMethod]
        public void Pipeline_Default_Shipping_Should_Set_Tracking_And_Delivery_DateShipped()
        {
            Order order = GetTestOrder();
            DateTime estimatedDelivery = DateTime.Now.AddDays(5);

            _pipeline.ShipOrder(order, "12345", estimatedDelivery);

            Assert.AreEqual("12345", order.TrackingNumber);
            Assert.AreEqual(estimatedDelivery, order.EstimatedDelivery);
            Assert.IsNotNull(order.DateShipped);
        }

        [TestMethod]
        public void Pipeline_Default_Shipping_Should_Set_Status_To_Shipped()
        {
            Order order = GetTestOrder();
            DateTime estimatedDelivery = DateTime.Now.AddDays(5);

            _pipeline.ShipOrder(order, "12345", estimatedDelivery);

            Assert.AreEqual(OrderStatus.Shipped,order.Status);
        }

        [TestMethod]
        public void Pipeline_Default_Shipping_Should_Send_Email_To_Customer()
        {
            Order order = GetTestOrder();
            DateTime estimatedDelivery = DateTime.Now.AddDays(5);

            _pipeline.ShipOrder(order, "12345", estimatedDelivery);

            TestMailerService mailer=_mailerService as TestMailerService;
            
            Assert.AreEqual(1,mailer.SentMail.Count);
            Assert.AreEqual(MailerType.CustomerOrderShipped,mailer.SentMail[0].MailType);

        }

        #endregion

        #region Shipping Process - WWF

       
        [TestMethod]
        public void Pipeline_WWF_Shipping_Should_Set_Tracking_And_Delivery_DateShipped()
        {
            Order order = GetTestOrder();

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };
                DateTime estimatedDelivery = DateTime.Now.AddDays(5);
                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.ShipOrderWorkflow), GetShippingParams(order, "12345", estimatedDelivery));

                instance.Start();

                waitHandle.WaitOne();

                //execution should be finished
                //check the order for status


                Assert.AreEqual("12345", order.TrackingNumber);
                Assert.AreEqual(estimatedDelivery, order.EstimatedDelivery);
                Assert.IsNotNull(order.DateShipped);

            }
        }

        [TestMethod]
        public void Pipeline_WWF_Shipping_Should_Set_Status_To_Shipped()
        {
            Order order = GetTestOrder();
            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };
                DateTime estimatedDelivery = DateTime.Now.AddDays(5);
                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.ShipOrderWorkflow), GetShippingParams(order, "12345", estimatedDelivery));

                instance.Start();

                waitHandle.WaitOne();

                //execution should be finished
                //check the order for status


                Assert.AreEqual(OrderStatus.Shipped, order.Status);

            }

        }

        [TestMethod]
        public void Pipeline_WWF_Shipping_Should_Send_Email_To_Customer()
        {
            Order order = GetTestOrder();
            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };
                DateTime estimatedDelivery = DateTime.Now.AddDays(5);
                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.ShipOrderWorkflow), GetShippingParams(order, "12345", estimatedDelivery));

                instance.Start();

                waitHandle.WaitOne();

                //execution should be finished
                //check the order for status


                TestMailerService mailer = _mailerService as TestMailerService;

                Assert.AreEqual(1, mailer.SentMail.Count);
                Assert.AreEqual(MailerType.CustomerOrderShipped, mailer.SentMail[0].MailType);

            }


        }

        #endregion

        #region Cancel Process - Default
        [TestMethod]
        public void Pipeline_Default_Cancel_Should_Not_Allow_Cancel_With_Status_Of_NotCheckedOut()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.NotCheckoutOut;
            bool threw = false;

            try
            {
                _pipeline.CancelOrder(order);
            }
            catch(InvalidOperationException x)
            {
                threw = true;
            }
            Assert.IsTrue(threw);
        }
        [TestMethod]
        public void Pipeline_Default_Cancel_Should_Set_Status_To_Cancelled_When_StatusWas_Submitted()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.Submitted;
            _pipeline.CancelOrder(order);
            Assert.AreEqual(OrderStatus.Cancelled, order.Status);
        }
        [TestMethod]
        public void Pipeline_Default_Cancel_Should_Set_Status_To_Cancelled_When_StatusWas_Verified()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.Verified;
            _pipeline.CancelOrder(order);
            Assert.AreEqual(OrderStatus.Cancelled, order.Status);
        }


        [TestMethod]
        public void Pipeline_Default_Cancel_Should_Set_Status_To_Refunded_When_StatusWas_Charged()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.Charged;
            _pipeline.CancelOrder(order);
            Assert.AreEqual(OrderStatus.Refunded, order.Status);
        }

        [TestMethod]
        public void Pipeline_Default_Cancel_Should_Create_Refund_Transaction_When_Refunded()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.Charged;
            _pipeline.CancelOrder(order);
            Assert.AreEqual(OrderStatus.Refunded, order.Status);
            Assert.AreEqual(1, order.Transactions.Count);
            Assert.AreEqual(order.Total, order.Transactions[0].Amount);
        }

        [TestMethod]
        public void Pipeline_Default_Cancel_Should_Create_Inventory_Record_For_ReturnedItems()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.Charged;
            _pipeline.CancelOrder(order);

            foreach (OrderItem item in order.Items)
            {
                IList<InventoryRecord> records = _inventoryService.GetInventory(item.Product.ID).ToList();
                //the repository inits with one record per product
                //the return will add a record per order item
                //so this should be 2
                Assert.AreEqual(2, records.Count);
                Assert.AreEqual(item.Quantity, records[1].Increment);
            }
        }


        #endregion

        #region Cancel Process - WWF
        [TestMethod]
        public void Pipeline_WWF_Cancel_Should_Not_Allow_Cancel_With_Status_Of_NotCheckedOut()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.NotCheckoutOut;

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };
                DateTime estimatedDelivery = DateTime.Now.AddDays(5);
                
                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.CancelOrderWorkflow), GetCancelParams(order));

                instance.Start();

                waitHandle.WaitOne();
                Assert.AreEqual(OrderStatus.NotCheckoutOut, order.Status);
            }

        }
        [TestMethod]
        public void Pipeline_WWF_Cancel_Should_Set_Status_To_Cancelled_When_Submitted_Was_Status()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.Submitted;

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.CancelOrderWorkflow), GetCancelParams(order));

                instance.Start();

                waitHandle.WaitOne();
                Assert.AreEqual(OrderStatus.Cancelled, order.Status);
                //execution should be finished
                //check the order for status
            }
        }
        [TestMethod]
        public void Pipeline_WWF_Cancel_Should_Set_Status_To_Cancelled_When_StatusWas_Verified()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.Verified;

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.CancelOrderWorkflow), GetCancelParams(order));

                instance.Start();

                waitHandle.WaitOne();
                Assert.AreEqual(OrderStatus.Cancelled, order.Status);
                //execution should be finished
                //check the order for status
            }
        }


        [TestMethod]
        public void Pipeline_WWF_Cancel_Should_Set_Status_To_Refunded_When_StatusWas_Charged()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.Charged;

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.CancelOrderWorkflow), GetCancelParams(order));

                instance.Start();

                waitHandle.WaitOne();
                Assert.AreEqual(OrderStatus.Refunded, order.Status);
                //execution should be finished
                //check the order for status
            }
        }

        [TestMethod]
        public void Pipeline_WWF_Cancel_Should_Create_Refund_Transaction_When_Refunded()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.Charged;

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.CancelOrderWorkflow), GetCancelParams(order));

                instance.Start();

                waitHandle.WaitOne();
                Assert.AreEqual(OrderStatus.Refunded, order.Status);
                Assert.AreEqual(1, order.Transactions.Count);
                Assert.AreEqual(order.Total, order.Transactions[0].Amount);
                //execution should be finished
                //check the order for status
            }
        }

        [TestMethod]
        public void Pipeline_WWF_Cancel_Should_Create_Inventory_Record_For_ReturnedItems()
        {
            Order order = GetTestOrder();
            order.Status = OrderStatus.Charged;

            using (WorkflowRuntime workflowRuntime = new WorkflowRuntime())
            {
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                workflowRuntime.WorkflowCompleted += delegate(object sender, WorkflowCompletedEventArgs e) { waitHandle.Set(); };
                workflowRuntime.WorkflowTerminated += delegate(object sender, WorkflowTerminatedEventArgs e)
                {
                    Debug.WriteLine(e.Exception.Message);
                    waitHandle.Set();
                };

                WorkflowInstance instance = workflowRuntime.CreateWorkflow(
                    typeof(Commerce.Pipelines.CancelOrderWorkflow), GetCancelParams(order));

                instance.Start();

                waitHandle.WaitOne();
                
                foreach (OrderItem item in order.Items)
                {
                    IList<InventoryRecord> records = _inventoryService.GetInventory(item.Product.ID).ToList();
                    //the repository inits with one record per product
                    //the return will add a record per order item
                    //so this should be 2
                    Assert.AreEqual(2, records.Count);
                    Assert.AreEqual(item.Quantity, records[1].Increment);
                }
            }

        }


        #endregion


    }
}
