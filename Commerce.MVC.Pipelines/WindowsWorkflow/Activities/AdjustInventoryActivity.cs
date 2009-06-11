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

namespace Commerce.Pipelines.Activities
{
    public partial class AdjustInventoryActivity : System.Workflow.ComponentModel.Activity {
        public AdjustInventoryActivity() {
            InitializeComponent();
        }




        public static DependencyProperty CustomerOrderProperty =
                DependencyProperty.Register("CustomerOrder", typeof(Order),
                typeof(AdjustInventoryActivity));

        public static DependencyProperty InventoryInterfaceProperty =
                DependencyProperty.Register("InventoryInterface", typeof(IInventoryService),
                typeof(AdjustInventoryActivity));


        public Order CustomerOrder {
            get {
                return (Order)base.GetValue(CustomerOrderProperty);
            }
            set {
                base.SetValue(CustomerOrderProperty, value);
            }
        }
        public IInventoryService InventoryInterface {
            get {
                return (IInventoryService)base.GetValue(InventoryInterfaceProperty);
            }
            set {
                base.SetValue(InventoryInterfaceProperty, value);
            }
        }



        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext) {


            //debit the inventory for each product
            foreach (OrderItem item in CustomerOrder.Items) {

                //increment the inventory
                InventoryInterface.IncrementInventory(item.Product.ID, 
                    -item.Quantity, "Debiting inventory for order " + item.OrderID);

                //update the product inventory status
                InventoryInterface.SetProductInventoryStatus(item.Product);
                

            }


            return ActivityExecutionStatus.Closed;


        }


    }
}
