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
    public partial class SaveOrderActivity : System.Workflow.ComponentModel.Activity
    {
        public SaveOrderActivity()
        {
            InitializeComponent();
        }

        public static DependencyProperty CustomerOrderProperty =
        DependencyProperty.Register("CustomerOrder", typeof(Order),
        typeof(SaveOrderActivity));


        public static DependencyProperty OrderServiceInterfaceProperty =
        DependencyProperty.Register("OrderServiceInterface", typeof(IOrderService),
        typeof(SaveOrderActivity));

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

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            OrderServiceInterface.SaveOrder(CustomerOrder);
            return ActivityExecutionStatus.Closed;
        }

    }
}
