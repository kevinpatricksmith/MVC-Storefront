using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services
{
    public interface IPaymentService
    {

        Transaction Authorize(Order order);
        Transaction Capture(Order order);
        Transaction Refund(Order order);
        
    }
}
