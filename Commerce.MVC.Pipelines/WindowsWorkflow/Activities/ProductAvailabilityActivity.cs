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
    public partial class ProductAvailabilityActivity : System.Workflow.ComponentModel.Activity {
        public ProductAvailabilityActivity() {
            InitializeComponent();
        }


        public static DependencyProperty CustomerOrderProperty =
        DependencyProperty.Register("CustomerOrder", typeof(Order),
        typeof(ProductAvailabilityActivity));

        public static DependencyProperty CatalogServiceInterfaceProperty =
                DependencyProperty.Register("CatalogServiceInterface", typeof(ICatalogService),
                typeof(ProductAvailabilityActivity));



        public Order CustomerOrder {
            get {
                return (Order)base.GetValue(CustomerOrderProperty);
            }
            set {
                base.SetValue(CustomerOrderProperty, value);
            }
        }
        public ICatalogService CatalogServiceInterface {
            get {
                return (ICatalogService)base.GetValue(CatalogServiceInterfaceProperty);
            }
            set {
                base.SetValue(CatalogServiceInterfaceProperty, value);
            }
        }


        public bool ProductsAreAvailable() {
            bool result = true;

            //load up all the products
            foreach (OrderItem item in CustomerOrder.Items) {
                //check the inventory status for each product
                //call up the product and check it's status
                //this is very, VERY simplistic
                Product p = CatalogServiceInterface.GetProduct(item.Product.ID);
                if (p != null) {

                    if (p.Inventory == InventoryStatus.Discontinued ||
                        p.Inventory == InventoryStatus.CurrentlyUnavailable) {
                        result = false;
                        break;
                    }
                        

                }
            }

            return result;
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext) {

            if (!ProductsAreAvailable())
                throw new InvalidOperationException("One of the products in this order are not available");

            return ActivityExecutionStatus.Closed;

        }
    }
}
