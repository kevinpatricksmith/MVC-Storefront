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
    partial class CancelOrderWorkflow
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
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            this.SetAsCancelled = new Commerce.Pipelines.Activities.SetOrderStatusActivity();
            this.SetRefundedStatus = new Commerce.Pipelines.Activities.SetOrderStatusActivity();
            this.RefundOrder = new Commerce.Pipelines.WindowsWorkflow.Activities.RefundOrderActivity();
            this.NoRefund = new System.Workflow.Activities.IfElseBranchActivity();
            this.CanRefund = new System.Workflow.Activities.IfElseBranchActivity();
            this.SaveOrder = new Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity();
            this.ReturnInventory = new Commerce.Pipelines.WindowsWorkflow.Activities.ReturnInventoryActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.CancelValidation = new System.Workflow.Activities.CodeActivity();
            // 
            // SetAsCancelled
            // 
            activitybind1.Name = "CancelOrderWorkflow";
            activitybind1.Path = "CustomerOrder";
            this.SetAsCancelled.CustomerOrderStatus = Commerce.Data.OrderStatus.Cancelled;
            this.SetAsCancelled.Name = "SetAsCancelled";
            activitybind2.Name = "CancelOrderWorkflow";
            activitybind2.Path = "OrderServiceInterface";
            this.SetAsCancelled.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.SetAsCancelled.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // SetRefundedStatus
            // 
            activitybind3.Name = "CancelOrderWorkflow";
            activitybind3.Path = "CustomerOrder";
            this.SetRefundedStatus.CustomerOrderStatus = Commerce.Data.OrderStatus.Refunded;
            this.SetRefundedStatus.Name = "SetRefundedStatus";
            activitybind4.Name = "CancelOrderWorkflow";
            activitybind4.Path = "OrderServiceInterface";
            this.SetRefundedStatus.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.SetRefundedStatus.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // RefundOrder
            // 
            activitybind5.Name = "CancelOrderWorkflow";
            activitybind5.Path = "CustomerOrder";
            this.RefundOrder.Name = "RefundOrder";
            activitybind6.Name = "CancelOrderWorkflow";
            activitybind6.Path = "OrderServiceInterface";
            activitybind7.Name = "CancelOrderWorkflow";
            activitybind7.Path = "PaymentServiceInterface";
            this.RefundOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.RefundOrderActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.RefundOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.RefundOrderActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.RefundOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.RefundOrderActivity.PaymentGatewayProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // NoRefund
            // 
            this.NoRefund.Activities.Add(this.SetAsCancelled);
            this.NoRefund.Name = "NoRefund";
            // 
            // CanRefund
            // 
            this.CanRefund.Activities.Add(this.RefundOrder);
            this.CanRefund.Activities.Add(this.SetRefundedStatus);
            ruleconditionreference1.ConditionName = "IsRefundable";
            this.CanRefund.Condition = ruleconditionreference1;
            this.CanRefund.Name = "CanRefund";
            // 
            // SaveOrder
            // 
            activitybind8.Name = "CancelOrderWorkflow";
            activitybind8.Path = "CustomerOrder";
            this.SaveOrder.Name = "SaveOrder";
            activitybind9.Name = "CancelOrderWorkflow";
            activitybind9.Path = "OrderServiceInterface";
            this.SaveOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.SaveOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // ReturnInventory
            // 
            activitybind10.Name = "CancelOrderWorkflow";
            activitybind10.Path = "CustomerOrder";
            activitybind11.Name = "CancelOrderWorkflow";
            activitybind11.Path = "InventoryServiceInterface";
            this.ReturnInventory.Name = "ReturnInventory";
            this.ReturnInventory.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.ReturnInventoryActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.ReturnInventory.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.ReturnInventoryActivity.InventoryInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.CanRefund);
            this.ifElseActivity1.Activities.Add(this.NoRefund);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // CancelValidation
            // 
            this.CancelValidation.Name = "CancelValidation";
            this.CancelValidation.ExecuteCode += new System.EventHandler(this.CancelValidation_ExecuteCode);
            // 
            // CancelOrderWorkflow
            // 
            this.Activities.Add(this.CancelValidation);
            this.Activities.Add(this.ifElseActivity1);
            this.Activities.Add(this.ReturnInventory);
            this.Activities.Add(this.SaveOrder);
            this.Name = "CancelOrderWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private IfElseBranchActivity NoRefund;
        private IfElseBranchActivity CanRefund;
        private Commerce.Pipelines.Activities.SetOrderStatusActivity SetRefundedStatus;
        private Commerce.Pipelines.WindowsWorkflow.Activities.RefundOrderActivity RefundOrder;
        private Commerce.Pipelines.Activities.SetOrderStatusActivity SetAsCancelled;
        private Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity SaveOrder;
        private Commerce.Pipelines.WindowsWorkflow.Activities.ReturnInventoryActivity ReturnInventory;
        private CodeActivity CancelValidation;
        private IfElseActivity ifElseActivity1;




















    }
}
