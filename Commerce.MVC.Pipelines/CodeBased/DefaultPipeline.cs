using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.Pipelines
{
	public class DefaultPipeline:IPipelineEngine
	{


        IAddressValidationService _addressValidation;
        IPaymentService _paymentService;
        IOrderService _orderService;
        IMailerService _mailerService;
        IInventoryService _inventoryService;

        public DefaultPipeline(
            IAddressValidationService addressValidation,
            IPaymentService paymentService,
            IOrderService orderService,
            IMailerService mailerService,
            IInventoryService inventoryService
            ) {
            _addressValidation = addressValidation;
            _paymentService = paymentService;
            _orderService = orderService;
            _mailerService = mailerService;
            _inventoryService = inventoryService;
            
        }


        public void VerifyOrder(Order order) {
            
            
            //set the order as submitted - this will "clear" the basket
            order.Status = OrderStatus.Submitted;
            _orderService.SaveOrder(order);


            //send thank you email
            _mailerService.SendOrderEmail(order, MailerType.CustomerOrderReceived);

            
            //validate shipping address
            _addressValidation.VerifyAddress(order.ShippingAddress);


            //authorize payment
            _paymentService.Authorize(order);


            //set order status to verified
            order.Status = OrderStatus.Verified;

            //save the order
            _orderService.SaveOrder(order);

            //adjust inventory
            foreach (OrderItem item in order.Items) {
                _inventoryService.IncrementInventory(item.Product.ID, -item.Quantity, "Adjustment for order " + order.ID);
            }

            //email admin RE new order
            _mailerService.SendOrderEmail(order, MailerType.AdminOrderReceived);

        }

        public void ChargeOrder(Order order)
        {

            //issue the capture order to the payment service
            //and add the transaction to the order
            //this gets saved on Submit
            if (order.Transactions == null)
                order.Transactions = new LazyList<Transaction>();

            //pull this order and make sure it's not already transacted/charged
            CheckOrderForPayments(order);

            Transaction t = _paymentService.Capture(order);
            CreateTransaction(order, t.AuthorizationCode, t.Amount, t.Processor);

            //set the status of the order to "Charged"
            order.Status = OrderStatus.Charged;

            _orderService.SaveOrder(order);

            
        }

        public void CheckOrderForPayments(Order order) {
            //pull this order and make sure it's not already transacted/charged
            Order orderCheck = _orderService.GetOrder(order.ID);

            decimal amountPaid = 0;

            if (orderCheck.Transactions.Count > 0)
                amountPaid = orderCheck.Transactions.Sum(x => x.Amount);

            if (amountPaid > 0)
                throw new InvalidOperationException("A transaction already exists for this order: " + order.OrderNumber);

        }

        public void ShipOrder(Order order, 
            string trackingNumber, DateTime estimatedDelivery)
        {
            
            //set the tracking number
            order.TrackingNumber = trackingNumber;

            //set the estimated delivery
            order.EstimatedDelivery = estimatedDelivery;

            //set the shipped date
            order.DateShipped = DateTime.Now;

            //set status
            order.Status = OrderStatus.Shipped;
            _orderService.SaveOrder(order);

            //let the customer know
            _mailerService.SendOrderEmail(order, MailerType.CustomerOrderShipped);
        }

        public void CancelOrder(Order order)
        {
            //make sure this can happen!
            _orderService.ValidateOrderForCancel(order);

            //refund the payment
            if (_orderService.CanRefundOrder(order))
            {
                order.Transactions = new LazyList<Transaction>();
                order.Transactions.Add(_paymentService.Refund(order));

                //set the status
                order.Status = OrderStatus.Refunded;
            }
            else
            {
                order.Status = OrderStatus.Cancelled;
            }


            //put the inventory back
            //adjust inventory
            foreach (OrderItem item in order.Items)
            {
                _inventoryService.IncrementInventory(item.Product.ID, item.Quantity, "Adjustment for order " + order.ID + "  cancellation");
            }


            _orderService.SaveOrder(order);
            //save it
        }

        void CreateTransaction(Order order, string authCode, decimal payment, TransactionProcessor processor) {
            
            //create a transaction
            if (order.Transactions == null)
                order.Transactions = new LazyList<Transaction>();

            order.Transactions.Add(new Transaction(order.ID, payment, authCode, processor));

        }

        public void AcceptPalPayment(Order order, string transactionID, decimal payment) {

            //set the status
            order.Status = OrderStatus.Charged;

            //pull this order and make sure it's not already transacted/charged
            CheckOrderForPayments(order);

            CreateTransaction(order, transactionID, payment, TransactionProcessor.PayPal);            

            //save the order
            _orderService.SaveOrder(order);


            //inventory
            foreach (OrderItem item in order.Items) {
                _inventoryService.IncrementInventory(item.Product.ID, -item.Quantity, "Adjustment for order " + order.ID);
            }

            //send thank you email
            _mailerService.SendOrderEmail(order, MailerType.CustomerOrderReceived);

            //email admin RE new order
            _mailerService.SendOrderEmail(order, MailerType.AdminOrderReceived);

        }

    }
}
