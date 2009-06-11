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
    partial class ChargeWorkflow
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
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            this.SaveOrder = new Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity();
            this.SetAsCharged = new Commerce.Pipelines.Activities.SetOrderStatusActivity();
            this.ChargePayment = new Commerce.Pipelines.WindowsWorkflow.Activities.ChargePaymentActivity();
            // 
            // SaveOrder
            // 
            activitybind1.Name = "ChargeWorkflow";
            activitybind1.Path = "CustomerOrder";
            this.SaveOrder.Name = "SaveOrder";
            activitybind2.Name = "ChargeWorkflow";
            activitybind2.Path = "OrderServiceInterface";
            this.SaveOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.SaveOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // SetAsCharged
            // 
            activitybind3.Name = "ChargeWorkflow";
            activitybind3.Path = "CustomerOrder";
            this.SetAsCharged.CustomerOrderStatus = Commerce.Data.OrderStatus.Charged;
            this.SetAsCharged.Name = "SetAsCharged";
            activitybind4.Name = "ChargeWorkflow";
            activitybind4.Path = "OrderServiceInterface";
            this.SetAsCharged.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.SetAsCharged.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // ChargePayment
            // 
            activitybind5.Name = "ChargeWorkflow";
            activitybind5.Path = "CustomerOrder";
            this.ChargePayment.Name = "ChargePayment";
            activitybind6.Name = "ChargeWorkflow";
            activitybind6.Path = "OrderServiceInterface";
            activitybind7.Name = "ChargeWorkflow";
            activitybind7.Path = "PaymentServiceInterface";
            this.ChargePayment.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.ChargePaymentActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.ChargePayment.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.ChargePaymentActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.ChargePayment.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.ChargePaymentActivity.PaymentGatewayProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // ChargeWorkflow
            // 
            this.Activities.Add(this.ChargePayment);
            this.Activities.Add(this.SetAsCharged);
            this.Activities.Add(this.SaveOrder);
            this.Name = "ChargeWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Commerce.Pipelines.WindowsWorkflow.Activities.ChargePaymentActivity ChargePayment;
        private Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity SaveOrder;
        private Commerce.Pipelines.Activities.SetOrderStatusActivity SetAsCharged;











    }
}
