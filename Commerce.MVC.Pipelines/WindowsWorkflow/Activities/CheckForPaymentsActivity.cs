using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Commerce.Services;
using Commerce.Data;

namespace Commerce.Pipelines.WindowsWorkflow.Activities
{
    public partial class CheckForPaymentsActivity : System.Workflow.ComponentModel.Activity {
        public CheckForPaymentsActivity() {
            InitializeComponent();
        }

        public static DependencyProperty CustomerOrderProperty =
    DependencyProperty.Register("CustomerOrder", typeof(Order), typeof(CheckForPaymentsActivity));

        public Order CustomerOrder {
            get {
                return (Order)base.GetValue(CustomerOrderProperty);
            }
            set {
                base.SetValue(CustomerOrderProperty, value);
            }
        }


        public static DependencyProperty OrderServiceInterfaceProperty =
            DependencyProperty.Register("OrderServiceInterface", typeof(IOrderService),
            typeof(CheckForPaymentsActivity));

        public IOrderService OrderServiceInterface {
            get {
                return (IOrderService)base.GetValue(OrderServiceInterfaceProperty);
            }
            set {
                base.SetValue(OrderServiceInterfaceProperty, value);
            }
        }


        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext) {

            //pull this order and make sure it's not already transacted/charged
            Order orderCheck = OrderServiceInterface.GetOrder(CustomerOrder.ID);

            decimal amountPaid = 0;

            if (orderCheck.Transactions.Count > 0)
                amountPaid = orderCheck.Transactions.Sum(x => x.Amount);

            if (amountPaid > 0)
                throw new InvalidOperationException("A transaction already exists for this order: " + CustomerOrder.OrderNumber);

            return ActivityExecutionStatus.Closed;
        }
    }


}
