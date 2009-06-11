using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Commerce.MVC.Pipelines
{
    public sealed partial class CreditCardVerification : SequentialWorkflowActivity {
        public CreditCardVerification() {
            InitializeComponent();
        }

        public static DependencyProperty MailerServiceInterfaceProperty = DependencyProperty.Register("MailerServiceInterface", typeof(Commerce.MVC.Services.IMailerService), typeof(Commerce.MVC.Pipelines.CreditCardVerification));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Commerce.MVC.Services.IMailerService MailerServiceInterface {
            get {
                return ((Commerce.MVC.Services.IMailerService)(base.GetValue(Commerce.MVC.Pipelines.CreditCardVerification.MailerServiceInterfaceProperty)));
            }
            set {
                base.SetValue(Commerce.MVC.Pipelines.CreditCardVerification.MailerServiceInterfaceProperty, value);
            }
        }

        public static DependencyProperty CustomerOrderProperty = DependencyProperty.Register("CustomerOrder", typeof(Commerce.MVC.Data.Order), typeof(Commerce.MVC.Pipelines.CreditCardVerification));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public Commerce.MVC.Data.Order CustomerOrder {
            get {
                return ((Commerce.MVC.Data.Order)(base.GetValue(Commerce.MVC.Pipelines.CreditCardVerification.CustomerOrderProperty)));
            }
            set {
                base.SetValue(Commerce.MVC.Pipelines.CreditCardVerification.CustomerOrderProperty, value);
            }
        }
    }

}
