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
    partial class SubmitOrderWorkflow
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
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind18 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind19 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind20 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind21 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind22 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind23 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind24 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.WorkflowServiceAttributes workflowserviceattributes1 = new System.Workflow.Activities.WorkflowServiceAttributes();
            this.terminateActivity2 = new System.Workflow.ComponentModel.TerminateActivity();
            this.SendAuthFailEmail = new Commerce.Pipelines.Activities.SendEmailActivity();
            this.terminateActivity1 = new System.Workflow.ComponentModel.TerminateActivity();
            this.SendInvalidShippingToCustomer = new Commerce.Pipelines.Activities.SendEmailActivity();
            this.terminateActivity3 = new System.Workflow.ComponentModel.TerminateActivity();
            this.SendAdminErrorEmail = new Commerce.Pipelines.Activities.SendEmailActivity();
            this.faultHandlerActivity2 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.CatchAddressFail = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.faultHandlerActivity3 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.faultHandlersActivity2 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.AuthPayment = new Commerce.Pipelines.Activities.AuthorizePaymentActivity();
            this.HandleBadAddress = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.ValidateShipping = new Commerce.Pipelines.Activities.ValidateAddressActivity();
            this.cancellationHandlerActivity1 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.PipeFailureHandler = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.AdminNewOrderEmail = new Commerce.Pipelines.Activities.SendEmailActivity();
            this.AdjustInventory = new Commerce.Pipelines.Activities.AdjustInventoryActivity();
            this.SaveOrder = new Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity();
            this.SetAsVerified = new Commerce.Pipelines.Activities.SetOrderStatusActivity();
            this.AuthPaymentSequence = new System.Workflow.Activities.SequenceActivity();
            this.ShippingSequence = new System.Workflow.Activities.SequenceActivity();
            this.SendThankYouEmail = new Commerce.Pipelines.Activities.SendEmailActivity();
            this.SaveInitialOrder = new Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity();
            this.SetAsSubmitted = new Commerce.Pipelines.Activities.SetOrderStatusActivity();
            // 
            // terminateActivity2
            // 
            this.terminateActivity2.Name = "terminateActivity2";
            // 
            // SendAuthFailEmail
            // 
            activitybind1.Name = "SubmitOrderWorkflow";
            activitybind1.Path = "CustomerOrder";
            activitybind2.Name = "SubmitOrderWorkflow";
            activitybind2.Path = "MailerServiceInterface";
            this.SendAuthFailEmail.MailType = Commerce.Data.MailerType.CustomerPaymentAuthFailed;
            this.SendAuthFailEmail.Name = "SendAuthFailEmail";
            this.SendAuthFailEmail.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.MailerServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.SendAuthFailEmail.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // terminateActivity1
            // 
            this.terminateActivity1.Name = "terminateActivity1";
            // 
            // SendInvalidShippingToCustomer
            // 
            activitybind3.Name = "SubmitOrderWorkflow";
            activitybind3.Path = "CustomerOrder";
            activitybind4.Name = "SubmitOrderWorkflow";
            activitybind4.Path = "MailerServiceInterface";
            this.SendInvalidShippingToCustomer.MailType = Commerce.Data.MailerType.CustomerAddressValidationFailed;
            this.SendInvalidShippingToCustomer.Name = "SendInvalidShippingToCustomer";
            this.SendInvalidShippingToCustomer.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.MailerServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.SendInvalidShippingToCustomer.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // terminateActivity3
            // 
            this.terminateActivity3.Name = "terminateActivity3";
            // 
            // SendAdminErrorEmail
            // 
            activitybind5.Name = "SubmitOrderWorkflow";
            activitybind5.Path = "CustomerOrder";
            activitybind6.Name = "SubmitOrderWorkflow";
            activitybind6.Path = "MailerServiceInterface";
            this.SendAdminErrorEmail.MailType = Commerce.Data.MailerType.AdminProcessingError;
            this.SendAdminErrorEmail.Name = "SendAdminErrorEmail";
            this.SendAdminErrorEmail.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.SendAdminErrorEmail.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.MailerServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // faultHandlerActivity2
            // 
            this.faultHandlerActivity2.Activities.Add(this.SendAuthFailEmail);
            this.faultHandlerActivity2.Activities.Add(this.terminateActivity2);
            this.faultHandlerActivity2.FaultType = typeof(System.Exception);
            this.faultHandlerActivity2.Name = "faultHandlerActivity2";
            // 
            // CatchAddressFail
            // 
            this.CatchAddressFail.Activities.Add(this.SendInvalidShippingToCustomer);
            this.CatchAddressFail.Activities.Add(this.terminateActivity1);
            this.CatchAddressFail.FaultType = typeof(System.Exception);
            this.CatchAddressFail.Name = "CatchAddressFail";
            // 
            // faultHandlerActivity3
            // 
            this.faultHandlerActivity3.Activities.Add(this.SendAdminErrorEmail);
            this.faultHandlerActivity3.Activities.Add(this.terminateActivity3);
            this.faultHandlerActivity3.FaultType = typeof(System.ApplicationException);
            this.faultHandlerActivity3.Name = "faultHandlerActivity3";
            // 
            // faultHandlersActivity2
            // 
            this.faultHandlersActivity2.Activities.Add(this.faultHandlerActivity2);
            this.faultHandlersActivity2.Name = "faultHandlersActivity2";
            // 
            // AuthPayment
            // 
            activitybind7.Name = "SubmitOrderWorkflow";
            activitybind7.Path = "CustomerOrder";
            this.AuthPayment.Name = "AuthPayment";
            activitybind8.Name = "SubmitOrderWorkflow";
            activitybind8.Path = "PaymentServiceInterface";
            this.AuthPayment.SetBinding(Commerce.Pipelines.Activities.AuthorizePaymentActivity.PaymentGatewayProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.AuthPayment.SetBinding(Commerce.Pipelines.Activities.AuthorizePaymentActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // HandleBadAddress
            // 
            this.HandleBadAddress.Activities.Add(this.CatchAddressFail);
            this.HandleBadAddress.Name = "HandleBadAddress";
            // 
            // ValidateShipping
            // 
            activitybind9.Name = "SubmitOrderWorkflow";
            activitybind9.Path = "AddressValidationInterface";
            activitybind10.Name = "SubmitOrderWorkflow";
            activitybind10.Path = "CustomerOrder.ShippingAddress";
            this.ValidateShipping.Name = "ValidateShipping";
            this.ValidateShipping.SetBinding(Commerce.Pipelines.Activities.ValidateAddressActivity.AddressServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.ValidateShipping.SetBinding(Commerce.Pipelines.Activities.ValidateAddressActivity.AddressToVerifyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // cancellationHandlerActivity1
            // 
            this.cancellationHandlerActivity1.Name = "cancellationHandlerActivity1";
            // 
            // PipeFailureHandler
            // 
            this.PipeFailureHandler.Activities.Add(this.faultHandlerActivity3);
            this.PipeFailureHandler.Name = "PipeFailureHandler";
            // 
            // AdminNewOrderEmail
            // 
            activitybind11.Name = "SubmitOrderWorkflow";
            activitybind11.Path = "CustomerOrder";
            activitybind12.Name = "SubmitOrderWorkflow";
            activitybind12.Path = "MailerServiceInterface";
            this.AdminNewOrderEmail.MailType = Commerce.Data.MailerType.AdminOrderReceived;
            this.AdminNewOrderEmail.Name = "AdminNewOrderEmail";
            this.AdminNewOrderEmail.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.AdminNewOrderEmail.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.MailerServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // AdjustInventory
            // 
            activitybind13.Name = "SubmitOrderWorkflow";
            activitybind13.Path = "CustomerOrder";
            activitybind14.Name = "SubmitOrderWorkflow";
            activitybind14.Path = "InventoryServiceInterface";
            this.AdjustInventory.Name = "AdjustInventory";
            this.AdjustInventory.SetBinding(Commerce.Pipelines.Activities.AdjustInventoryActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.AdjustInventory.SetBinding(Commerce.Pipelines.Activities.AdjustInventoryActivity.InventoryInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            // 
            // SaveOrder
            // 
            activitybind15.Name = "SubmitOrderWorkflow";
            activitybind15.Path = "CustomerOrder";
            this.SaveOrder.Name = "SaveOrder";
            activitybind16.Name = "SubmitOrderWorkflow";
            activitybind16.Path = "OrderServiceInterface";
            this.SaveOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            this.SaveOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            // 
            // SetAsVerified
            // 
            activitybind17.Name = "SubmitOrderWorkflow";
            activitybind17.Path = "CustomerOrder";
            this.SetAsVerified.CustomerOrderStatus = Commerce.Data.OrderStatus.Verified;
            this.SetAsVerified.Name = "SetAsVerified";
            activitybind18.Name = "SubmitOrderWorkflow";
            activitybind18.Path = "OrderServiceInterface";
            this.SetAsVerified.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            this.SetAsVerified.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
            // 
            // AuthPaymentSequence
            // 
            this.AuthPaymentSequence.Activities.Add(this.AuthPayment);
            this.AuthPaymentSequence.Activities.Add(this.faultHandlersActivity2);
            this.AuthPaymentSequence.Name = "AuthPaymentSequence";
            // 
            // ShippingSequence
            // 
            this.ShippingSequence.Activities.Add(this.ValidateShipping);
            this.ShippingSequence.Activities.Add(this.HandleBadAddress);
            this.ShippingSequence.Name = "ShippingSequence";
            // 
            // SendThankYouEmail
            // 
            activitybind19.Name = "SubmitOrderWorkflow";
            activitybind19.Path = "CustomerOrder";
            activitybind20.Name = "SubmitOrderWorkflow";
            activitybind20.Path = "MailerServiceInterface";
            this.SendThankYouEmail.MailType = Commerce.Data.MailerType.CustomerOrderReceived;
            this.SendThankYouEmail.Name = "SendThankYouEmail";
            this.SendThankYouEmail.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind19)));
            this.SendThankYouEmail.SetBinding(Commerce.Pipelines.Activities.SendEmailActivity.MailerServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind20)));
            // 
            // SaveInitialOrder
            // 
            activitybind21.Name = "SubmitOrderWorkflow";
            activitybind21.Path = "CustomerOrder";
            this.SaveInitialOrder.Name = "SaveInitialOrder";
            activitybind22.Name = "SubmitOrderWorkflow";
            activitybind22.Path = "OrderServiceInterface";
            this.SaveInitialOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind21)));
            this.SaveInitialOrder.SetBinding(Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind22)));
            // 
            // SetAsSubmitted
            // 
            activitybind23.Name = "SubmitOrderWorkflow";
            activitybind23.Path = "CustomerOrder";
            this.SetAsSubmitted.CustomerOrderStatus = Commerce.Data.OrderStatus.Submitted;
            this.SetAsSubmitted.Name = "SetAsSubmitted";
            activitybind24.Name = "SubmitOrderWorkflow";
            activitybind24.Path = "OrderServiceInterface";
            this.SetAsSubmitted.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind23)));
            this.SetAsSubmitted.SetBinding(Commerce.Pipelines.Activities.SetOrderStatusActivity.OrderServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind24)));
            workflowserviceattributes1.ConfigurationName = "Commerce.Pipelines.Checkout";
            workflowserviceattributes1.Name = "Checkout";
            // 
            // SubmitOrderWorkflow
            // 
            this.Activities.Add(this.SetAsSubmitted);
            this.Activities.Add(this.SaveInitialOrder);
            this.Activities.Add(this.SendThankYouEmail);
            this.Activities.Add(this.ShippingSequence);
            this.Activities.Add(this.AuthPaymentSequence);
            this.Activities.Add(this.SetAsVerified);
            this.Activities.Add(this.SaveOrder);
            this.Activities.Add(this.AdjustInventory);
            this.Activities.Add(this.AdminNewOrderEmail);
            this.Activities.Add(this.PipeFailureHandler);
            this.Activities.Add(this.cancellationHandlerActivity1);
            this.Name = "SubmitOrderWorkflow";
            this.SetValue(System.Workflow.Activities.ReceiveActivity.WorkflowServiceAttributesProperty, workflowserviceattributes1);
            this.CanModifyActivities = false;

        }

        #endregion

        private CancellationHandlerActivity cancellationHandlerActivity1;
        private Commerce.Pipelines.Activities.AuthorizePaymentActivity AuthPayment;
        private Commerce.Pipelines.Activities.AdjustInventoryActivity AdjustInventory;
        private Commerce.Pipelines.Activities.ValidateAddressActivity ValidateShipping;
        private Commerce.Pipelines.Activities.SendEmailActivity AdminNewOrderEmail;
        private Commerce.Pipelines.Activities.SendEmailActivity SendInvalidShippingToCustomer;
        private FaultHandlerActivity CatchAddressFail;
        private FaultHandlersActivity HandleBadAddress;
        private SequenceActivity ShippingSequence;
        private Commerce.Pipelines.Activities.SendEmailActivity SendAuthFailEmail;
        private FaultHandlerActivity faultHandlerActivity2;
        private FaultHandlersActivity faultHandlersActivity2;
        private SequenceActivity AuthPaymentSequence;
        private Commerce.Pipelines.Activities.SendEmailActivity SendAdminErrorEmail;
        private FaultHandlerActivity faultHandlerActivity3;
        private TerminateActivity terminateActivity1;
        private TerminateActivity terminateActivity2;
        private TerminateActivity terminateActivity3;
        private Commerce.Pipelines.Activities.SendEmailActivity SendThankYouEmail;
        private Commerce.Pipelines.Activities.SetOrderStatusActivity SetAsVerified;
        private Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity SaveOrder;
        private Commerce.Pipelines.Activities.SetOrderStatusActivity SetAsSubmitted;
        private Commerce.Pipelines.WindowsWorkflow.Activities.SaveOrderActivity SaveInitialOrder;
        private FaultHandlersActivity PipeFailureHandler;






















































































































































































    }
}
