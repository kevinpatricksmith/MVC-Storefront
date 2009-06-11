using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.Pipelines.WindowsWorkflow
{
    public sealed partial class AcceptPayPalWorkflow : SequentialWorkflowActivity {

        public Commerce.Data.Order CustomerOrder { get; set; }
        public Commerce.Services.IPaymentService PaymentServiceInterface { get; set; }
        public Commerce.Services.IOrderService OrderServiceInterface { get; set; }
        public Commerce.Services.IMailerService MailerServiceInterface { get; set; }
        public Commerce.Services.IAddressValidationService AddressValidationInterface { get; set; }
        public Commerce.Services.IInventoryService InventoryServiceInterface { get; set; }
        public Commerce.Services.ICatalogService CatalogServiceInterface { get; set; }

        public string PayPalTransactionID { get; set; }
        public decimal AmountPaid { get; set; }



        public AcceptPayPalWorkflow() {
            InitializeComponent();
        }

        private void CreatePayPalTransaction_ExecuteCode(object sender, EventArgs e) {

            //create a transaction
            if (CustomerOrder.Transactions == null)
                CustomerOrder.Transactions = new LazyList<Transaction>();

            CustomerOrder.Transactions.Add(new Transaction(CustomerOrder.ID, AmountPaid, PayPalTransactionID,
                TransactionProcessor.PayPal));


        }
    }

}
