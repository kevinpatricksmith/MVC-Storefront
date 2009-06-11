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

namespace Commerce.Pipelines.WindowsWorkflow
{
    partial class AcceptPayPalWorkflow {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent() {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            this.AdminProcessError = new Commerce.Pipelines.Activities.SendEmailActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.EmailAdmin = new Commerce.Pipelines.Activities.SendEmailActivity();
            this.EmailOrderReceived = new Commerce.Pipelines.Activities.SendEmailActivity();
            this.AdjustInventory = new Commerce.Pipelines.Activities.AdjustInventoryActivity();
            this.SaveOrder = new Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity();
            this.CreatePayPalTransaction = new System.Workflow.Activities.CodeActivity();
            this.CheckForPayments = new Commerce.Pipelines.WindowsWorkflow.Activities.CheckForPaymentsActivity();
            this.SetAsCharged = new Commerce.Pipelines.Activities.SetOrderStatusActivity();
            // 
            // AdminProcessError
            // 
            activitybind1.Name = "AcceptPayPalWorkflow";
            activitybind1.Path = "CustomerOrder";
            activitybind2.Name = "AcceptPayPalWorkflow";
            activitybind2.Path = "MailerServiceInterface";
            this.AdminProcessError.MailType = Commerce.Data.MailerType.AdminProcessingError;
            this.AdminProcessError.Name = "AdminProcessError";
            this.AdminProcessError.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.AdminProcessError.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.MailerServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.AdminProcessError);
            this.faultHandlerActivity1.FaultType = typeof(System.Exception);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // EmailAdmin
            // 
            activitybind3.Name = "AcceptPayPalWorkflow";
            activitybind3.Path = "CustomerOrder";
            activitybind4.Name = "AcceptPayPalWorkflow";
            activitybind4.Path = "MailerServiceInterface";
            this.EmailAdmin.MailType = Commerce.Data.MailerType.AdminOrderReceived;
            this.EmailAdmin.Name = "EmailAdmin";
            this.EmailAdmin.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.EmailAdmin.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.MailerServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // EmailOrderReceived
            // 
            activitybind5.Name = "AcceptPayPalWorkflow";
            activitybind5.Path = "CustomerOrder";
            activitybind6.Name = "AcceptPayPalWorkflow";
            activitybind6.Path = "MailerServiceInterface";
            this.EmailOrderReceived.MailType = Commerce.Data.MailerType.CustomerOrderReceived;
            this.EmailOrderReceived.Name = "EmailOrderReceived";
            this.EmailOrderReceived.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.EmailOrderReceived.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.MailerServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // AdjustInventory
            // 
            activitybind7.Name = "AcceptPayPalWorkflow";
            activitybind7.Path = "CustomerOrder";
            activitybind8.Name = "AcceptPayPalWorkflow";
            activitybind8.Path = "InventoryServiceInterface";
            this.AdjustInventory.Name = "AdjustInventory";
            this.AdjustInventory.SetBinding(Commerce.Pipelines.Activities.AdjustInventoryActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.AdjustInventory.SetBinding(Commerce.Pipelines.Activities.AdjustInventoryActivity.InventoryInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // SaveOrder
            // 
            activitybind9.Name = "AcceptPayPalWorkflow";
            activitybind9.Path = "CustomerOrder";
            this.SaveOrder.Name = "SaveOrder";
            activitybind10.Name = "AcceptPayPalWorkflow";
            activitybind10.Path = "OrderServiceInterface";
            this.SaveOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.SaveOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // CreatePayPalTransaction
            // 
            this.CreatePayPalTransaction.Name = "CreatePayPalTransaction";
            this.CreatePayPalTransaction.ExecuteCode += new System.EventHandler(this.CreatePayPalTransaction_ExecuteCode);
            // 
            // CheckForPayments
            // 
            activitybind11.Name = "AcceptPayPalWorkflow";
            activitybind11.Path = "CustomerOrder";
            this.CheckForPayments.Name = "CheckForPayments";
            activitybind12.Name = "AcceptPayPalWorkflow";
            activitybind12.Path = "OrderServiceInterface";
            this.CheckForPayments.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.CheckForPaymentsActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.CheckForPayments.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.CheckForPaymentsActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // SetAsCharged
            // 
            activitybind13.Name = "AcceptPayPalWorkflow";
            activitybind13.Path = "CustomerOrder";
            this.SetAsCharged.CustomerOrderStatus = Commerce.Data.OrderStatus.Charged;
            this.SetAsCharged.Name = "SetAsCharged";
            activitybind14.Name = "AcceptPayPalWorkflow";
            activitybind14.Path = "OrderServiceInterface";
            this.SetAsCharged.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.SetAsCharged.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            // 
            // AcceptPayPalWorkflow
            // 
            this.Activities.Add(this.SetAsCharged);
            this.Activities.Add(this.CheckForPayments);
            this.Activities.Add(this.CreatePayPalTransaction);
            this.Activities.Add(this.SaveOrder);
            this.Activities.Add(this.AdjustInventory);
            this.Activities.Add(this.EmailOrderReceived);
            this.Activities.Add(this.EmailAdmin);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "AcceptPayPalWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Commerce.Pipelines.WindowsWorkflow.Activities.CheckForPaymentsActivity CheckForPayments;
        private CodeActivity CreatePayPalTransaction;
        private Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity SaveOrder;
        private Commerce.Pipelines.Activities.AdjustInventoryActivity AdjustInventory;
        private Commerce.Pipelines.Activities.SendEmailActivity EmailOrderReceived;
        private Commerce.Pipelines.Activities.SendEmailActivity EmailAdmin;
        private Commerce.Pipelines.Activities.SendEmailActivity AdminProcessError;
        private FaultHandlerActivity faultHandlerActivity1;
        private FaultHandlersActivity faultHandlersActivity1;
        private Commerce.Pipelines.Activities.SetOrderStatusActivity SetAsCharged;























    }
}
