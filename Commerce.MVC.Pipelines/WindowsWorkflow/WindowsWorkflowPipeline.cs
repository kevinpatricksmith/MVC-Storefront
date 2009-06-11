using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;
using System.Workflow.Runtime;
using Commerce.Services;

namespace Commerce.Pipelines {

    public class WindowsWorkflowPipeline : IPipelineEngine {


        IAddressValidationService _addressValidation;
        IPaymentService _paymentService;
        IOrderService _orderService;
        IMailerService _mailerService;
        IInventoryService _inventoryService;
        public WindowsWorkflowPipeline(
            IAddressValidationService addressValidation,
            IPaymentService paymentService,
            IOrderService orderService,
            IMailerService mailerService,
            IInventoryService inventoryService
            ) {
            _addressValidation = addressValidation;
            _paymentService = paymentService;
            _orderService = orderService;
            _mailerService = mailerService;
            _inventoryService = inventoryService;

        }

        public void VerifyOrder(Order order) {

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PaymentServiceInterface", _paymentService);
            parameters.Add("OrderServiceInterface", _orderService);
            parameters.Add("MailerServiceInterface", _mailerService);
            parameters.Add("AddressValidationInterface", _addressValidation);
            parameters.Add("InventoryServiceInterface", _inventoryService);
            parameters.Add("CustomerOrder", order);

            WorkflowInstance instance = WFRuntimeInstance.CreateWorkflow(
                typeof(Commerce.Pipelines.SubmitOrderWorkflow), parameters);

            instance.Start();

        }

        static WorkflowRuntime WFRuntime = null;
        static readonly object padlock = new object();

        public static WorkflowRuntime WFRuntimeInstance {
            get {
                lock (padlock) {
                    if (WFRuntime == null) {
                        WFRuntime = new WorkflowRuntime();
                        WFRuntime.StartRuntime();

                    }
                    return WFRuntime;
                }
            }
        }

        public static void StopWorkflowRuntime() {
            if (WFRuntime != null) {
                WFRuntime.StopRuntime();
            }
        }


        public void ChargeOrder(Order order) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PaymentServiceInterface", _paymentService);
            parameters.Add("OrderServiceInterface", _orderService);
            parameters.Add("MailerServiceInterface", _mailerService);
            parameters.Add("AddressValidationInterface", _addressValidation);
            parameters.Add("InventoryServiceInterface", _inventoryService);
            parameters.Add("CustomerOrder", order);

            WorkflowInstance instance = WFRuntimeInstance.CreateWorkflow(
                typeof(Commerce.Pipelines.ChargeWorkflow), parameters);

            instance.Start();
        }

        public void ShipOrder(Order order,
            string trackingNumber, DateTime estimatedDelivery) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("OrderServiceInterface", _orderService);
            parameters.Add("MailerServiceInterface", _mailerService);
            parameters.Add("CustomerOrder", order);
            parameters.Add("TrackingNumber", trackingNumber);
            parameters.Add("EstimatedDelivery", estimatedDelivery);

            WorkflowInstance instance = WFRuntimeInstance.CreateWorkflow(
                typeof(Commerce.Pipelines.ShipOrderWorkflow), parameters);

            instance.Start();
        }

        public void CancelOrder(Order order) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("OrderServiceInterface", _orderService);
            parameters.Add("MailerServiceInterface", _mailerService);
            parameters.Add("PaymentServiceInterface", _paymentService);
            parameters.Add("InventoryServiceInterface", _inventoryService);
            parameters.Add("CustomerOrder", order);

            WorkflowInstance instance = WFRuntimeInstance.CreateWorkflow(
                typeof(Commerce.Pipelines.CancelOrderWorkflow), parameters);

            instance.Start();
        }
        public void AcceptPalPayment(Order order, string transactionID, decimal payment) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("OrderServiceInterface", _orderService);
            parameters.Add("MailerServiceInterface", _mailerService);
            parameters.Add("PaymentServiceInterface", _paymentService);
            parameters.Add("InventoryServiceInterface", _inventoryService);
            parameters.Add("CustomerOrder", order);
            parameters.Add("PayPalTransactionID", transactionID);
            parameters.Add("AmountPaid", payment);

            WorkflowInstance instance = WFRuntimeInstance.CreateWorkflow(
                typeof(Commerce.Pipelines.WindowsWorkflow.AcceptPayPalWorkflow), parameters);

            instance.Start();
        }

    }
}