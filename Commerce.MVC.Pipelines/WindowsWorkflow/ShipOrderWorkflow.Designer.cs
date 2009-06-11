using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Commerce.Pipelines
{
    partial class ShipOrderWorkflow
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            this.NotifyCustomer = new Commerce.Pipelines.Activities.SendEmailActivity();
            this.SaveOrder = new Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity();
            this.SetAsShipped = new Commerce.Pipelines.Activities.SetOrderStatusActivity();
            this.SetTrackingAndDelivery = new System.Workflow.Activities.CodeActivity();
            // 
            // NotifyCustomer
            // 
            activitybind1.Name = "ShipOrderWorkflow";
            activitybind1.Path = "CustomerOrder";
            activitybind2.Name = "ShipOrderWorkflow";
            activitybind2.Path = "MailerServiceInterface";
            this.NotifyCustomer.MailType = Commerce.Data.MailerType.CustomerOrderShipped;
            this.NotifyCustomer.Name = "NotifyCustomer";
            this.NotifyCustomer.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.NotifyCustomer.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.MailerServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // SaveOrder
            // 
            activitybind3.Name = "ShipOrderWorkflow";
            activitybind3.Path = "CustomerOrder";
            this.SaveOrder.Name = "SaveOrder";
            activitybind4.Name = "ShipOrderWorkflow";
            activitybind4.Path = "OrderServiceInterface";
            this.SaveOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.SaveOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // SetAsShipped
            // 
            activitybind5.Name = "ShipOrderWorkflow";
            activitybind5.Path = "CustomerOrder";
            this.SetAsShipped.CustomerOrderStatus = Commerce.Data.OrderStatus.Shipped;
            this.SetAsShipped.Name = "SetAsShipped";
            activitybind6.Name = "ShipOrderWorkflow";
            activitybind6.Path = "OrderServiceInterface";
            this.SetAsShipped.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.SetAsShipped.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // SetTrackingAndDelivery
            // 
            this.SetTrackingAndDelivery.Name = "SetTrackingAndDelivery";
            this.SetTrackingAndDelivery.ExecuteCode += new System.EventHandler(this.SetTrackingAndDelivery_ExecuteCode);
            // 
            // ShipOrderWorkflow
            // 
            this.Activities.Add(this.SetTrackingAndDelivery);
            this.Activities.Add(this.SetAsShipped);
            this.Activities.Add(this.SaveOrder);
            this.Activities.Add(this.NotifyCustomer);
            this.Name = "ShipOrderWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Commerce.Pipelines.Activities.SetOrderStatusActivity SetAsShipped;
        private Commerce.Pipelines.Activities.SendEmailActivity NotifyCustomer;
        private Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity SaveOrder;
        private CodeActivity SetTrackingAndDelivery;









    }
}
