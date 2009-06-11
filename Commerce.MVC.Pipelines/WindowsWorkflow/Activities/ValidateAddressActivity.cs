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
    public partial class ValidateAddressActivity : System.Workflow.ComponentModel.Activity {
        public ValidateAddressActivity() {
            InitializeComponent();
        }

        public static DependencyProperty AddressServiceInterfaceProperty =
            DependencyProperty.Register("AddressServiceInterface", typeof(IAddressValidationService),
            typeof(ValidateAddressActivity));


        public IAddressValidationService AddressServiceInterface {
            get {
                return (IAddressValidationService)base.GetValue(AddressServiceInterfaceProperty);
            }
            set {
                base.SetValue(AddressServiceInterfaceProperty, value);
            }
        }


        public static DependencyProperty AddressToVerifyProperty =
            DependencyProperty.Register("AddressToVerify", typeof(Address),
            typeof(ValidateAddressActivity));


        public Address AddressToVerify {
            get {
                return (Address)base.GetValue(AddressToVerifyProperty);
            }
            set {
                base.SetValue(AddressToVerifyProperty, value);
            }
        }


        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext) {

            AddressServiceInterface.VerifyAddress(AddressToVerify);

            return ActivityExecutionStatus.Closed;

        }


    }
}
