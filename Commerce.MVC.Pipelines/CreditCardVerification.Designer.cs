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

namespace Commerce.MVC.Pipelines
{
    partial class CreditCardVerification {
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
            this.SendThankYouEmail = new Commerce.MVC.Pipelines.Activities.SendEmailActivity();
            // 
            // SendThankYouEmail
            // 
            activitybind1.Name = "CreditCardVerification";
            activitybind1.Path = "CustomerOrder";
            activitybind2.Name = "CreditCardVerification";
            activitybind2.Path = "MailerServiceInterface";
            this.SendThankYouEmail.MailType = Commerce.MVC.Data.MailerType.CustomerOrderReceived;
            this.SendThankYouEmail.Name = "SendThankYouEmail";
            this.SendThankYouEmail.SetBinding(Commerce.MVC.Pipelines.Activities.SendEmailActivity.MailerServiceInterfaceProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.SendThankYouEmail.SetBinding(Commerce.MVC.Pipelines.Activities.SendEmailActivity.CustomerOrderProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // CreditCardVerification
            // 
            this.Activities.Add(this.SendThankYouEmail);
            this.Name = "CreditCardVerification";
            this.CanModifyActivities = false;

        }

        #endregion

        private Commerce.MVC.Pipelines.Activities.SendEmailActivity SendThankYouEmail;



    }
}
