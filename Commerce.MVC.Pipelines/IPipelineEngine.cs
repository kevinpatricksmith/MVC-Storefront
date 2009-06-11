using System;
namespace Commerce.Pipelines {
    public interface IPipelineEngine {
        void VerifyOrder(Commerce.Data.Order order);
        void ChargeOrder(Commerce.Data.Order order);
        void ShipOrder(Commerce.Data.Order order, 
            string trackingNumber, DateTime estimatedDelivery);
        void CancelOrder(Commerce.Data.Order order);

        void AcceptPalPayment(Commerce.Data.Order order, string transactionID, decimal payment);
    }
}
