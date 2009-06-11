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

namespace Commerce.Pipelines
{
    public sealed partial class CancelOrderWorkflow : SequentialWorkflowActivity
    {


        public Commerce.Data.Order CustomerOrder { get; set; }
        public Commerce.Services.IPaymentService PaymentServiceInterface { get; set; }
        public Commerce.Services.IOrderService OrderServiceInterface { get; set; }
        public Commerce.Services.IMailerService MailerServiceInterface { get; set; }
        public Commerce.Services.IInventoryService InventoryServiceInterface { get; set; }
        public Commerce.Services.ICatalogService CatalogServiceInterface { get; set; }


        public CancelOrderWorkflow()
        {
            InitializeComponent();
        }

        private void CancelValidation_ExecuteCode(object sender, EventArgs e)
        {
            OrderServiceInterface.ValidateOrderForCancel(CustomerOrder);
        }
    }

}
