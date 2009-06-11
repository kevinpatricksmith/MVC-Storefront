using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Services;

namespace Commerce.Tests
{
    public class ThrowPaymentService:IPaymentService
    {

        public Commerce.Data.Transaction Authorize(Commerce.Data.Order order)
        {
            throw new InvalidOperationException("Authorization failed");
        }

        public Commerce.Data.Transaction Refund(Commerce.Data.Order order)
        {
            throw new InvalidOperationException("Refund failed");
        }

        public Commerce.Data.Transaction Capture(Commerce.Data.Order order)
        {
            throw new InvalidOperationException("Capture failed");
        }

    }
}
