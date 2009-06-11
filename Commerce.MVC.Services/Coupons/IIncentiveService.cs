using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services {
    public interface IIncentiveService {

        IIncentive GetIncentive(string couponCode);
        void ProcessCoupon(string couponCode, Order order);
    }
}
