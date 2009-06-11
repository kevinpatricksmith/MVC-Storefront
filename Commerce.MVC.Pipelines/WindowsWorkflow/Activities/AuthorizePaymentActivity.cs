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

namespace Commerce.Pipelines.Activities
{
    public partial class AuthorizePaymentActivity : System.Workflow.ComponentModel.Activity {
        public AuthorizePaymentActivity() {
            InitializeComponent();
        }

        public static DependencyProperty CustomerOrderProperty =
            DependencyProperty.Register("CustomerOrder", typeof(Order), typeof(AuthorizePaymentActivity));

        public Order CustomerOrder {
            get {
                return (Order)base.GetValue(CustomerOrderProperty);
            }
            set {
                base.SetValue(CustomerOrderProperty, value);
            }
        }

        public static DependencyProperty PaymentGatewayProperty =
            DependencyProperty.Register("PaymentGateway", typeof(IPaymentService), typeof(AuthorizePaymentActivity));

        public IPaymentService PaymentGateway {
            get {
                return (IPaymentService)base.GetValue(PaymentGatewayProperty);
            }
            set {
                base.SetValue(PaymentGatewayProperty, value);
            }
        }


        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext) {
            PaymentGateway.Authorize(CustomerOrder);
            return ActivityExecutionStatus.Closed;
        }
    }
}
