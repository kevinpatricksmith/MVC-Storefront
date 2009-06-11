using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {

    public class PercentOffOrderCoupon:ICoupon {

        public decimal PercentOff { get; set; }
        
        public PercentOffOrderCoupon(decimal percentOff) {
            if (percentOff < 1 || percentOff > 100)
                throw new Exception("Percent off needs to be an integer between 0 and 100");
            PercentOff = percentOff;
        }

        public void ApplyCoupon(Order order) {

            decimal discountRate = PercentOff / 100;
            order.DiscountAmount = order.SubTotal * discountRate;
            

        }
    }
}
