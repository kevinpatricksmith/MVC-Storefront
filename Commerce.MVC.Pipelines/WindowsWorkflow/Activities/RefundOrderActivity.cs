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
using Commerce.Data;
using Commerce.Services;

namespace Commerce.Pipelines.WindowsWorkflow.Activities
{
    public partial class RefundOrderActivity : System.Workflow.ComponentModel.Activity
    {
        public RefundOrderActivity()
        {
            InitializeComponent();
        }

        public static DependencyProperty CustomerOrderProperty =
    DependencyProperty.Register("CustomerOrder", typeof(Order), typeof(RefundOrderActivity));

        public Order CustomerOrder
        {
            get
            {
                return (Order)base.GetValue(CustomerOrderProperty);
            }
            set
            {
                base.SetValue(CustomerOrderProperty, value);
            }
        }

        public static DependencyProperty PaymentGatewayProperty =
            DependencyProperty.Register("PaymentGateway", typeof(IPaymentService),
            typeof(RefundOrderActivity));

        public IPaymentService PaymentGateway
        {
            get
            {
                return (IPaymentService)base.GetValue(PaymentGatewayProperty);
            }
            set
            {
                base.SetValue(PaymentGatewayProperty, value);
            }
        }

        public static DependencyProperty OrderServiceInterfaceProperty =
            DependencyProperty.Register("OrderServiceInterface", typeof(IOrderService),
            typeof(RefundOrderActivity));

        public IOrderService OrderServiceInterface
        {
            get
            {
                return (IOrderService)base.GetValue(OrderServiceInterfaceProperty);
            }
            set
            {
                base.SetValue(OrderServiceInterfaceProperty, value);
            }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {

            CustomerOrder.Transactions = new LazyList<Transaction>();
            CustomerOrder.Transactions.Add(PaymentGateway.Refund(CustomerOrder));
            return ActivityExecutionStatus.Closed;

        }
    }
}
