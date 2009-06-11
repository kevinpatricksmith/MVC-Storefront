using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services
{
    [Serializable]
    public class FakePaymentService : IPaymentService
    {

        public Transaction Authorize(Order order)
        {
            

            //this is where you would structure a call to a payment processor
            //of your choice - like Verisign, PayPal, etc
            //an authorization is just that - not an actual movement of money
            //legally you are only allowed to transact money
            //when the shippable goods have been shipped

            
            //the credit card number and information is on the order
            CreditCard card = order.PaymentMethod as CreditCard;

            //run the auth here, with the data from the order, as needed

            string authCode = "XYZ" + Guid.NewGuid().ToString().Substring(0, 10);


            return new Transaction(order.ID, order.Total, authCode, TransactionProcessor.FakePaymentProcessor);

        }

        public Transaction Refund(Order order)
        {


            //Refunding process - most gateways support this
            string authCode = "XYZ" + Guid.NewGuid().ToString().Substring(0, 10);


            return new Transaction(order.ID, order.Total, authCode, TransactionProcessor.FakePaymentProcessor);

        }

        public Transaction Capture(Order order)
        {

            //Refunding process - most gateways support this
            string authCode = "XYZ" + Guid.NewGuid().ToString().Substring(0, 10);


            return new Transaction(order.ID, order.Total, authCode, TransactionProcessor.FakePaymentProcessor);
        }

    }
}
