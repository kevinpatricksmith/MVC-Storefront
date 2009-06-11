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
    public partial class SetOrderStatusActivity : System.Workflow.ComponentModel.Activity {
        
        
        public SetOrderStatusActivity() {
            InitializeComponent();
        }


        public static DependencyProperty CustomerOrderProperty =
                DependencyProperty.Register("CustomerOrder", typeof(Order),
                typeof(SetOrderStatusActivity));


        public static DependencyProperty CustomerOrderStatusProperty =
        DependencyProperty.Register("CustomerOrderStatus", typeof(OrderStatus),
        typeof(SetOrderStatusActivity));
        
        public static DependencyProperty OrderServiceInterfaceProperty =
        DependencyProperty.Register("OrderServiceInterface", typeof(IOrderService),
        typeof(SetOrderStatusActivity));
        
        public IOrderService OrderServiceInterface {
            get {
                return (IOrderService)base.GetValue(OrderServiceInterfaceProperty);
            }
            set {
                base.SetValue(OrderServiceInterfaceProperty, value);
            }
        }


        public OrderStatus CustomerOrderStatus {
            get {
                return (OrderStatus)base.GetValue(CustomerOrderStatusProperty);
            }
            set {
                base.SetValue(CustomerOrderStatusProperty, value);
            }
        }

        public Order CustomerOrder {
            get {
                return (Order)base.GetValue(CustomerOrderProperty);
            }
            set {
                base.SetValue(CustomerOrderProperty, value);
            }
        }


        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext) {

            CustomerOrder.Status = CustomerOrderStatus;
            return ActivityExecutionStatus.Closed;
        }
    }
}
