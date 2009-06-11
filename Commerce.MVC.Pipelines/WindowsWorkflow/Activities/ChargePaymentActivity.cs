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
    public partial class ChargePaymentActivity : System.Workflow.ComponentModel.Activity
    {
        public ChargePaymentActivity()
        {
            InitializeComponent();
        }


        public static DependencyProperty CustomerOrderProperty =
            DependencyProperty.Register("CustomerOrder", typeof(Order), typeof(ChargePaymentActivity));

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
            DependencyProperty.Register("PaymentGateway", typeof(IPaymentService), typeof(ChargePaymentActivity));

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
            typeof(ChargePaymentActivity));

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
            //Capture the payment
            if (CustomerOrder.Transactions == null)
                CustomerOrder.Transactions = new LazyList<Transaction>();

            //pull this order and make sure it's not already transacted/charged
            Order orderCheck = OrderServiceInterface.GetOrder(CustomerOrder.ID);

            decimal amountPaid = 0;

            if (orderCheck.Transactions.Count > 0)
                amountPaid = orderCheck.Transactions.Sum(x => x.Amount);

            if (amountPaid > 0)
                throw new InvalidOperationException("A transaction already exists for this order: " + CustomerOrder.OrderNumber);

            CustomerOrder.Transactions.Add(PaymentGateway.Capture(CustomerOrder));

            return ActivityExecutionStatus.Closed;
        }
    }
}
