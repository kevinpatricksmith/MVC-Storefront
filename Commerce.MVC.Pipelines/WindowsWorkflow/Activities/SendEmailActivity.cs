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
    public partial class SendEmailActivity : System.Workflow.ComponentModel.Activity {
        public SendEmailActivity() {
            InitializeComponent();
        }


        public static DependencyProperty CustomerOrderProperty =
        DependencyProperty.Register("CustomerOrder", typeof(Order),
        typeof(SendEmailActivity));


        public Order CustomerOrder {
            get {
                return (Order)base.GetValue(CustomerOrderProperty);
            }
            set {
                base.SetValue(CustomerOrderProperty, value);
            }
        }


        public static DependencyProperty MailerServiceInterfaceProperty =
        DependencyProperty.Register("MailerServiceInterface", typeof(IMailerService),
        typeof(SendEmailActivity));


        public IMailerService MailerServiceInterface {
            get {
                return (IMailerService)base.GetValue(MailerServiceInterfaceProperty);
            }
            set {
                base.SetValue(MailerServiceInterfaceProperty, value);
            }
        }


        public static DependencyProperty MailTypeProperty =
            DependencyProperty.Register("MailType", typeof(MailerType),
            typeof(SendEmailActivity));


        public MailerType MailType {
            get {
                return (MailerType)base.GetValue(MailTypeProperty);
            }
            set {
                base.SetValue(MailTypeProperty, value);
            }
        }



        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext) {
            

            //send it
            try {
                MailerServiceInterface.SendOrderEmail(CustomerOrder, MailType);
            
            } catch (System.Net.Mail.SmtpException x) {
                //log the failure - don't let this throw the process
                //should setup for retry later
            }

            //log it


            return ActivityExecutionStatus.Closed;
        }


    }
}
