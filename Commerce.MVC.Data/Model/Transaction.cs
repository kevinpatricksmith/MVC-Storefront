using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data
{

    public enum TransactionProcessor
    {
        FakePaymentProcessor=1,
        PayPal=2
    }
    
    
    public class Transaction
    {
        public Guid ID { get; set; }
        public Guid OrderID { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateExecuted { get; set; }
        public string AuthorizationCode { get; set; }
        public string Notes { get; set; }

        public TransactionProcessor Processor { get; set; }

        public bool IsRefund
        {
            get
            {
                return Amount <= 0;
            }
        }
        public Transaction() { }
        public Transaction(Guid orderID, decimal amount, string authCode, TransactionProcessor processor) : this(Guid.NewGuid(), orderID, amount, DateTime.Now, authCode, "",processor) { }

        public Transaction(Guid id, Guid orderID, decimal amount, DateTime executed, string authCode, string notes, TransactionProcessor processor)
        {
            ID = id;
            OrderID = orderID;
            Amount = amount;
            DateExecuted = executed;
            AuthorizationCode = authCode;
            Notes = notes;
            Processor = processor;
        }

    }
}
