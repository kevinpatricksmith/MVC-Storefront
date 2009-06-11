using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public class AmountOffOrderCoupon:ICoupon {

        public decimal AmountOff { get; set; }
        public AmountOffOrderCoupon(decimal amountOff) {
            AmountOff = amountOff;
        }


        public void ApplyCoupon(Order order) {

            //set the discount on the order
            order.DiscountAmount = AmountOff;

        }

    }
}
